using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esame_20190117 {

    static class MyExtensions {
        public static IEnumerable<IEnumerable<T>> RecApply<T>(this IEnumerable<IEnumerable<T>> s, Func<T, T> f) {
            if (s == null)
                throw new ArgumentNullException();
            foreach (var sequence in s) {
                if (sequence == null)
                    throw new ArgumentNullException();
                yield return ApplySequence(sequence, f);
            }
        }

        private static IEnumerable<T> ApplySequence<T>(IEnumerable<T> sequence, Func<T, T> f) {
            foreach (var e in sequence) {
                if (e == null)
                    throw new ArgumentNullException();
                yield return f(e);
            }
        }
    }

    [TestFixture]
    public class Test {
        IEnumerable<int> Infinite() //sequenza infinita di numeri interi
        {
            int i = 0;
            while (true) {
                yield return i++;
            }
        }

        IEnumerable<int> InfiniteRes() //sequenza infinita di numeri interi
        {
            int i = 1;
            while (true) {
                yield return i++;
            }
        }

        [Test]
        public void Identity_of_sequence_with_null_sequence_throws() {
            var s = new List<List<int>>() { new List<int>() { 1, -6, 7 }, null, new List<int>() { -3, 4, -4 }, new List<int>() };
            Func<int, int> f = e => e;
            Assert.That(() => s.RecApply(f).ToList(), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Concat_string_function_on_string_sequence_returns_concat() {
            var s = new List<List<string>>() { new List<string>() { "cane", "gatto" }, new List<string>(), new List<string>() { "merlo", "passero", "piccione" }, new List<string>(), new List<string>() };
            Func<string, string> f = e => "bel " + e;
            var expected = new List<List<string>>() { new List<string>() { "bel cane", "bel gatto" }, new List<string>(), new List<string>() { "bel merlo", "bel passero", "bel piccione" }, new List<string>(), new List<string>() };
            Assert.That(s.RecApply(f), Is.EqualTo(expected));
        }

        [Test]
        public void List_of_3_elements_with_infinite_element_function_increment_returns([Values(4)] int approx) {
            var s = new List<IEnumerable<int>>() { new List<int>(), new List<int>() { 6, 7, 8 }, Infinite().Take(approx) };
            Func<int, int> f = e => e + 1;
            var expected = new List<IEnumerable<int>>() { new List<int>(), new List<int>() { 7, 8, 9 }, InfiniteRes().Take(approx) };
            Assert.That(s.RecApply(f), Is.EqualTo(expected));
        }
    }
}