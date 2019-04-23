using System;
using System.Collections.Generic;
using Exam.Exam07092018;
using NUnit.Framework;

namespace Exam
{
    class Program
    {
 
        static void Main(string[] args)
        {
           // int[] ints = {1,2,3,4,5,6,7,8,9,10,11 };

            var list = new List<string>();
            var list1 = new List<string>();
            for (int i = 0; i < 4; i++) {
                list.Add("aa");
                list1.Add("bb");
            }
            list1.Add("cc");
            var result = list.Apply(list1, (i, i1) => i + i1);
            foreach (var i in result) {
                Console.WriteLine(i);
            }
            Console.ReadLine();

        }
    }
    
}
