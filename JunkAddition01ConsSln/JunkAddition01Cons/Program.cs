using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunkAddition01Cons
{
    class Program
    {
        static void Main(string[] args) { 
            Console.Write( "Please provide a number: " );
            Double theFirstValue = 0;
            theFirstValue = Convert.ToDouble( Console.ReadLine() );
            Console.WriteLine($"The value that you just entered is: {theFirstValue}" );
            Console.WriteLine();

            Console.Write("Please provide a second number: ");
            Double theSecondValue = 0;
            theSecondValue = Convert.ToDouble( Console.ReadLine() );
            Console.WriteLine($"The value that you just entered is: {theSecondValue}");
            Console.WriteLine();
            Console.WriteLine();

            var theSum = theFirstValue + theSecondValue;

            Console.WriteLine($"The sum is: {theSum}");
            Console.WriteLine();

        }
    }
}
