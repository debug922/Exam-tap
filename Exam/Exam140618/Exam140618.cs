using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Exam.Exam140618
{
    public static class Es1
    {
        public static IEnumerable<IEnumerable<int>> FibonacciProvider(this IEnumerable<int> intSeq)
        {
            if(intSeq==null)
                throw new ArgumentNullException();
            var count = intSeq.Count();
            if (count<2)
                throw new ArgumentException();
          
            for (var i = 0; i < intSeq.Count(); i++)
            {
                int a = i - 1, b = i;
                if (i == 0)
                {
                    a = i;
                    b = ++i;
                }
                var fib = Fibonacci(intSeq.ElementAtOrDefault(a), intSeq.ElementAtOrDefault(b));
                yield return fib;
            }
        }

        private static IEnumerable<int> Fibonacci(int f1, int f2)
        { 
            for (int i = 0; i < int.MaxValue; i++)
            {
                yield return f1;
                var fResult=f1 + f2;
                f1 = f2;
                f2 = fResult;
             
            }
        }
    }

    public class Es2
    {
        [Test]
        public void FibonacciProvider_intSeqNull_Throw()
        {
            List<int> x = null;
            Assert.That(() => x.FibonacciProvider().ToList(), Throws.TypeOf<ArgumentNullException>());
        }
        //fail a causa di yeld
        [Test]
        public void FibonacciProvider_validArg()
        {
            int[] seq = {1, 1};
            int[] expect = {1, 1, 2, 3, 5, 8, 13, 21, 34, 55};
            var values = seq.FibonacciProvider();
            Assert.That(values.Count(), Is.EqualTo(1));
            var first = new List<int>();
            foreach (var value in values)
            {
                for (int j = 0; j < 10; j++)
                    first.Add(value.ElementAtOrDefault(j));

            }

            CollectionAssert.AreEqual(expect, first);

        }
        [Test]
        [TestCase(3, 1)]
        public void FibonacciProvider_validArg1(int approx, int whichSeries)
        {
            IEnumerable<int> intSeq = Enumerable.Range(0, int.MaxValue);
            int[] series = {intSeq.ElementAtOrDefault(whichSeries), intSeq.ElementAtOrDefault(whichSeries + 1)};
            var values = series.FibonacciProvider();
            var list = new List<int>();
            foreach (var value in values)
                for (int i = 0; i < approx; i++)
                    list.Add(value.ElementAtOrDefault(i));
            int[] expected = {1, 2, 3};
            CollectionAssert.AreEqual(expected, list);

        }

    }

        public class Es3
        {
            //attributo
            //1. Definire un custom attribute che si possa applicare (al più una volta) solo a interfacce e permetta di associare ad
            // un’interfaccia il nome (cioè una String) della sua implementazione di default, ed eventualmente ulteriori possibili
            // implementazioni. In particolare, definire costruttore e meccanismi per memorizzare ed accedere ai nomi delle
            // classi indicate come implementazione.
            [AttributeUsage(AttributeTargets.Interface,AllowMultiple = false)]
            public class ExecuteMeAttribute : Attribute
            {
                public ExecuteMeAttribute(string implement,params object[] list)
                {
                    Implement = implement;
                    Param = list;
                }

                public string Implement { get; }
                public object[] Param { get; }
            }
            /*
         2. Scrivere un metodo che verifica che tutti gli usi del custom attribute al punto precedente, in un assembly passato
         come parametro, siano corretti
       */
            public static bool CheckAttribute(Assembly assembly)
            {
                if(assembly==null)
                    throw new ArgumentNullException();
                var types = (from type in assembly.GetTypes()
                    where Attribute.IsDefined(type, typeof(ExecuteMeAttribute))
                    select type).SingleOrDefault();

                if (types == null)
                throw new InvalidOperationException();

                var customAttribute = types.GetCustomAttributes(typeof(ExecuteMeAttribute), false).SingleOrDefault();
                ExecuteMeAttribute attribute = customAttribute as ExecuteMeAttribute ?? throw new InvalidOperationException();

                var useInterface = from type in assembly.GetTypes()
                    where type.GetInterface(types.Name)!=null
                    select type;
                int count=0;
                foreach (var type in useInterface)
                {
                    if(type.Name.Equals(attribute.Implement))
                        count++;
                    foreach (var param in attribute.Param)
                        if (type.Name.Equals(param))
                            count++;
                }
                return count == attribute.Param.Length + 1;
            }
            [Es3.ExecuteMe(nameof(Test))]
            public interface ITest { }

            public class Test : ITest
            {

            }
    }
    
}
