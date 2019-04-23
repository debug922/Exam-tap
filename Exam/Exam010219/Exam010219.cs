using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
//Con la collaborazione di Davide
namespace Exam.Exam010219 {
    public static class Es1 {
        public static IEnumerable<T> EvenOddSwap<T>(this IEnumerable<T> s) {
            if (s == null)
                throw new ArgumentNullException();

            using (var en = s.GetEnumerator()) {
                if (!en.MoveNext())
                    throw new ArgumentException();

                do {
                    var now = en.Current;
                    if (!en.MoveNext()) {
                        yield return now;
                        yield break;
                    }
                    yield return en.Current;
                    yield return now;

                } while (en.MoveNext());
            }
        }

    }
    [TestFixture]
    public class Es2 {

        IEnumerable<int> Infinite() //sequenza infinita di numeri interi
        {
            int i = 1;
            while (true) {
                yield return i++;
            }
        }

        IEnumerable<int> InfiniteRes() //sequenza infinita di numeri interi
        {
            yield return 2;
            yield return 1;
            int i = 1;
            int j = 2;
            while (true) {
                yield return j += 2;
                yield return i += 2;

            }
        }

        [Test]
        public void Null_sequence_throws() {
            List<int> s = null;
            Assert.That(() => s.EvenOddSwap().ToList(), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void List_swap_returns() {
            var s = new List<string>() { "Donald Duck", "Louie", "Dewey", "Huey", "Scrooge McDuck" };
            var expected = new List<string>() { "Louie", "Donald Duck", "Huey", "Dewey", "Scrooge McDuck" };
            var value = s.EvenOddSwap();
            CollectionAssert.AreEqual(expected, value);
            //Assert.That(s.EvenOddSwap(), Is.EqualTo(expected));
        }

        [Test]
        public void Infinite_swap_approx_returns([Values(10)] int approx) {
            var s = Infinite().Take(approx);
            var expected = InfiniteRes().Take(approx);
            Assert.That(s.EvenOddSwap(), Is.EqualTo(expected));
        }
    }

    public class Es3 {


        public class Client1 {
            public ILib0 MyLib0 { get; }

            public Client1(ILib0Factory factory) {
                MyLib0 = factory.CreateNew();
            }

            public ILib0 CM(ILib0Factory factory) {
                var x = factory.CreateNew();
                x.I++;
                return x;
            }
        }
        public interface ILib0 {
            int I { get; set; }
        }
        public class Lib0 : ILib0 {
            public int I { get; set; }
            public Lib0(ILiB1 liB1) { I = liB1.M1(); }
        }
        public interface ILiB1 {
            int M1();
        }
        public class Lib1 : ILiB1 {
            public int M1() { return 42; }
        }
        public interface ILib0Factory {
            ILib0 CreateNew();
        }
        public class Lib0Factory : ILib0Factory {
            public ILib0 CreateNew() {
                var lib = new Lib1Factory();
                return new Lib0(lib.CreateNew());
            }
        }
        public class Lib1Factory {
            public ILiB1 CreateNew() {
                return new Lib1();
            }
        }
    }
}
