using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;


namespace ConsoleApp2
{
    class Program
    {
        public static void Main(string[] args)
        {
//            List<string> rowUnparsed = "Tz14892, -75, 3/4/2018 11:51:28 AM".Split(',').ToList<string>();
//            Console.WriteLine(rowUnparsed.Reverse());

            List<string> names = "Tz14892, -75, 3/4/2018 11:51:28 AM".Split(',').ToList<string>();
            Console.WriteLine(names[0] + names[1] + names[2]);
        }
    }
}



//        {
//            var numbers = new List<int>{1, 2, 3, 4, 5, 6};
//            var smallests = GetSmallests(numbers, 3);
//
//            foreach (var number in smallests)
//                Console.WriteLine(number);
//        }
//
//        public static List<int> GetSmallests(List<int> list, int count)
//        {
//            var smallests = new List<int>();
//
//            while (smallests.Count < count)
//            {
//                var min = GetSmallest(list);
//                smallests.Add(min);
//                list.Remove(min);
//            }
//
//            return smallests;
//        }
//
//        public static int GetSmallest(List<int> list)
//            {
//                var min = list[0];
//                for (var i = 1; i < list.Count; i++)
//                {
//                    if (list[i] < min)
//                        min = list[i];
//                }
//                return min;
//            }
//
//        }
//    }
