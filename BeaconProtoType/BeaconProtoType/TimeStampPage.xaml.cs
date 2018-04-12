using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.Media;
using Android.Runtime;
using BeaconProtoType;
using Plugin.BluetoothLE;
using Plugin.Messaging;
using SQLite;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeaconProtoType
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeStampPage : ContentPage
    {

        public ObservableCollection<TimeInPunches> myTimeIns;
        public ObservableCollection<TimeInPunches> myTimeOuts;
        public ObservableCollection<IDevice> myDevices;
        public ObservableCollection<String> deviceNames;
        private SQLiteAsyncConnection connection;
        private BluetoothAdapter mBluetoothAdapter;
        public float rssi;
        private object icon;

        public TimeStampPage()
        {
            InitializeComponent();

            connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;

        }

        protected override async void OnAppearing()
        {
            await connection.CreateTableAsync<TimeInPunches>();

            var myTimes = await connection.Table<TimeInPunches>().ToListAsync();

            myTimeIns = new ObservableCollection<TimeInPunches>(myTimes);
            myTimeOuts = new ObservableCollection<TimeInPunches>(myTimes);
            myDevices = new ObservableCollection<IDevice>();
            deviceNames = new ObservableCollection<string>();   //Collects ID, RSSI, Time records

            TimeInList.ItemsSource = myTimeIns;

            DeviceList.ItemsSource = deviceNames;

            if (!mBluetoothAdapter.IsEnabled)
            {
                var result = await DisplayAlert("Bluetooth Acivation", "This device will require bluetooth activation, may I activate bluetooth?", "Ok", "No");

                if (result)
                {
                    mBluetoothAdapter.Enable();
                }

            }

            base.OnAppearing();

        }

        private async void TimeIn_Clicked(object sender, EventArgs e)
        {
            var Punch = new TimeInPunches
            {
                UserName = "test",      
                BeaconID = "test",
                PhoneID = "test",
                TimeIn = DateTime.Now,
                SignalStrength = "close"
            };
            if (myTimeIns.Count == myTimeOuts.Count)
            {
                await connection.InsertAsync(Punch);
                myTimeIns.Add(Punch);
            }
            else
            {
                await DisplayAlert("Invalid TimePunch", "Cannot Punch In Without Punching Out", "Ok");
            }
        }

        private async void TimeOut_Clicked(object sender, EventArgs e)
        {
            var upDatedPunch = myTimeIns[0];
            upDatedPunch.TimeOut = DateTime.Now;

            if (myTimeOuts.Count == myTimeIns.Count - 1)
            {
                await connection.UpdateAsync(upDatedPunch);
            }

            else
            {
                await DisplayAlert("Invalid TimePunch", "Cannot Punch Out Without Punching In", "Ok");
            }

        }

        private async void Convert_Clicked(object sender, EventArgs e)
        {

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;

                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                IWorksheet worksheet = workbook.Worksheets[0];

                //Enabling formula calculation.
                worksheet.EnableSheetCalculations();


                //                worksheet["E1"].Text = "BeaconID";
                //                worksheet["F1"].Text = "SignalStrength";

                worksheet["A1"].Text = "UserID:";          //AHT
                   // worksheet["B1"].Text = TimeInPunches.UserName;  //can't access?  marked private...

                worksheet["A2"].Text = "PhoneID:";       //AHT
                 // worksheet["B2"].Text = TimeInPunches.PhoneID;  //can't access?  marked private...

                worksheet["A3"].Text = "TimeIn:";        //AHT
                  // worksheet["B3"].Text = TimeInPunches.TimeIn;  //can't access?  marked private...
                  // worksheet["B3"].Text = myTimeIns.Count.ToString();
                worksheet["A4"].Text = "TimeOut:";       //AHT
                    // worksheet["B4"].Text = TimeInPunches.TimeOut;  //can't access?  marked private...
                   // worksheet["B4"].Text = myTimeOuts[0].ToString();

                worksheet["A5"].Text = "Shift Dur:";       //AHT
                // worksheet["B5"].Text = Convert.ToString.(TimeInPunches.TimeOut - TimeInPunches.TimeIn).hours;       //AHT

                worksheet["A6"].Text = "Record ";        //AHT
                worksheet["B6"].Text = "BeaconID";      //AHT
                worksheet["C6"].Text = "RSSI";          //AHT
                worksheet["D6"].Text = "Time";          //AHT
                worksheet["E6"].Text = "UUID";          //AHT

                var startRecordsRow = 7;


                for (int i = 0; i < deviceNames.Count; i++)
                {
                    // worksheet["A" + Convert.ToString(i + 2)].Text = myTimeIns[i].UserName;
                    // worksheet["B" + Convert.ToString(i + 2)].Text = myTimeIns[i].PhoneID;
                    // worksheet["C" + Convert.ToString(i + 2)].Text = myTimeIns[i].TimeIn.ToString();
                    // worksheet["D" + Convert.ToString(i + 2)].Text = myTimeIns[i].TimeOut.ToString();
                    // worksheet["E" + Convert.ToString(i + 2)].Text = myTimeIns[i].BeaconID;
                    // worksheet["F" + Convert.ToString(i + 2)].Text = myTimeIns[i].SignalStrength;

                    // worksheet["A" + Convert.ToString(i + 3)].Text = deviceNames[i];  //Orig from Paul


                    //Split comma-separated fields into Excel columns - AHT
                    List<string> deviceNameParsed = deviceNames[i].Split(',').ToList<string>(); //AHT

                    //Populate Excel spreadsheet
                    worksheet["A" + Convert.ToString(i + startRecordsRow)].Text = Convert.ToString(i);    //AHT
                    worksheet["B" + Convert.ToString(i + startRecordsRow)].Text = deviceNameParsed[0];    //AHT
                    worksheet["C" + Convert.ToString(i + startRecordsRow)].Text = deviceNameParsed[1];    //AHT
                    worksheet["D" + Convert.ToString(i + startRecordsRow)].Text = deviceNameParsed[2];    //AHT
                    worksheet["E" + Convert.ToString(i + startRecordsRow)].Text = deviceNameParsed[3];    //AHT

                }

                worksheet["A1:A1"].ColumnWidth = 10;        //AHT
                worksheet["B1:B1"].ColumnWidth = 10;        //AHT
                worksheet["C1:C1"].ColumnWidth = 7;         //AHT
                worksheet["D1:D1"].ColumnWidth = 22;        //AHT
                worksheet["E1:E1"].ColumnWidth = 38;        //AHT

                IRange headingRange = worksheet["A6:E6"];
                headingRange.CellStyle.Font.Bold = true;
                headingRange.CellStyle.ColorIndex = ExcelKnownColors.Light_green;  
                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);

                workbook.Close();

                await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("GettingStared.xlsx", "application/msexcel", stream);
            }
        }

        private async void ClearDatabase_Clicked(object sender, EventArgs e)
        {
            await connection.DropTableAsync<TimeInPunches>();
            await connection.CreateTableAsync<TimeInPunches>();
            myTimeIns.Clear();
            myTimeOuts.Clear();
        }

        private async void TimeInList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var mySelection = e.SelectedItem as TimeInPunches;
            await DisplayAlert("Alert", Convert.ToString(mySelection.TimeOut), "ok");
        }

        private void ScanForBluetooth_Clicked(object sender, EventArgs e)
        {
            CrossBleAdapter.Current.Scan().Subscribe(scanResult =>
            {
               // if (!string.IsNullOrWhiteSpace(scanResult.Device.Name) && !scanResult.Device.Name.Contains("Alta") && !scanResult.Device.Name.Contains("Speaker") && scanResult.Rssi >= -600)
                //if (!string.IsNullOrWhiteSpace(scanResult.Device.Name) && scanResult.Device.Name.Contains("Tz"))  //only Sanwotec emitters
                    if (!string.IsNullOrWhiteSpace(scanResult.Device.Name) )  //No filtering - record all BLE emitters
                    {
                    //deviceNames.Add(String.Format("{0} - RSSI: {1} Time: {2}", scanResult.Device.Name, scanResult.Rssi, DateTime.Now));
                     deviceNames.Add(String.Format("{0}, {1}, {2}, {3}", scanResult.Device.Name,  scanResult.Rssi, DateTime.Now, scanResult.Device.Uuid));

                }
            });
        }

        private object getApplicationContext()
        {
            throw new NotImplementedException();
        }

        private async void TimeOutList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var mySelection = e.SelectedItem as string;
            await DisplayAlert(mySelection, mySelection, "ok");
        }
    }
}


