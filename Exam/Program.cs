using Exam.Exam140618;
using System;
using System.Reflection;

namespace Exam
{
    class Program
    {
 
        static void Main(string[] args)
        {
            
            Es3.CheckAttribute(Assembly.GetExecutingAssembly());
            Console.ReadLine();

        }
    }
    
}
