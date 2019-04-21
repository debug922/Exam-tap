using Exam.Exam180118;
using System;

namespace Exam
{
    class Program
    {
 
        static void Main(string[] args)
        {
            var seq = "cippalippa";
            if(seq.CountUntil(char.IsDigit,null)==0)
                Console.WriteLine("fuck");
            var seq1 = "sottocoppe di peltro";
            if (Es3.FirstDigitPosition("aaa1")==3)
                Console.WriteLine("fuck1");
            Console.ReadLine();

        }
    }
    
}
