using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Exam {
    public class Replace {
        public static IEnumerable<T> MacroExpansion<T>(IEnumerable<T> sequence, T values, IEnumerable<T> newValues) {
            CheckNotNull(sequence, nameof(sequence));
            CheckNotNull(newValues, nameof(newValues));
            foreach (var elem in sequence) {
                if (Equals(elem, values))
                    foreach (var e in newValues) {
                        yield return e;
                    } else yield return elem;
            }
        }


        private static void CheckNotNull<T>(IEnumerable<T> parameter, string parameterName) {
            if (null == parameter) {
                throw new ArgumentNullException(nameof(parameter), " sequence cannot be null");
            }
        }
    }
    public class C {
        public int Key { get; }
        public string S { get; }

        public C(int k, string s) {
            Key = k;
            S = s;
        }

        public override bool Equals(object obj) //resharper puo auto generare i metodi su cui fare override
        {
            var o = obj as C;
            if (null == o) {
                return false;
            }
            return this.Key == o.Key;
        }

        public override int GetHashCode() {
            return Key.GetHashCode();
        }
    }
    [TestFixture]
    public class ReplaceTest {
        [Test]
        public void NullSequenceThrowsANE() {
            Assert.Throws<ArgumentNullException>(() =>
                Replace.MacroExpansion((IEnumerable<int>)null, 3, new int[1]).Count());
        }

        [Test] //NOTA: count e to array sono metodi a caso per far visitare l'enumerable dal compilatore
        public void NullNewValuesThrowsANE() {
            Assert.Throws<ArgumentNullException>(() =>
               Replace.MacroExpansion(new int[1], 3, ((IEnumerable<int>)null).ToArray()));
        }

        [Test]
        public void NothingToReplace() {
            var s = new[] { 1, 2, 3 };
            var nV = new[] { 7, 45 };
            var res = Replace.MacroExpansion(s, 467, nV);
            Assert.That(res, Is.EqualTo(s));
        }

        [Test]
        public void Replace1By1Once() {
            var s = new[] { 1, 2, 3 };
            var expected = new[] { 1, 7, 3 };
            var nV = new[] { 7 };
            var res =Replace.MacroExpansion(s, 2, nV);
            Assert.That(res, Is.EqualTo(expected));
        }

        [Test]
        public void Replace1By1Thrice() {
            var s = new[] { 1, 2, 3, 1, 7, 1 };
            var expected = new[] { 70, 2, 3, 70, 7, 70 };
            var nV = new[] { 70 };
            var res = Replace.MacroExpansion(s, 1, nV);
            Assert.That(res, Is.EqualTo(expected));
        }

        [Test]
        public void Replace1ByManyThrice() {
            var s = new[] { 1, 2, 3, 1, 7, 1 };
            var expected = new[] { 70, 100, 2, 3, 70, 100, 7, 70, 100 };
            var nV = new[] { 70, 100 };
            var res = Replace.MacroExpansion(s, 1, nV);
            Assert.That(res, Is.EqualTo(expected));
        }

        IEnumerable<int> Infinite() //sequenza infinita di numeri interi
        {
            int i = 0;
            while (true) {
                yield return i++;
            }
        }

        IEnumerable<int> InfiniteRes() //ho sbagliato qualcosa qui.. da ricontrollare
        {
            yield return 0;
            yield return 70;
            yield return 100;
            int i = 2;
            while (true) {
                yield return i++;
            }
        }

        [Test]
        public void Replace1ByManyOnceOnInfiniteSeq() //necessito di un enumerable che crei una sequenza infinita(metodo ausiliario)
        {
            var s = Infinite(); //1,2,3,4,5......
            var expected = InfiniteRes();
            var nV = new[] { 70, 100 };
            var res = Replace.MacroExpansion(s, 1, nV);
            Assert.That(res.Take(100), Is.EqualTo(expected)); //take: controllo solo una parte della sequenza infinita
        }

        [Test]
        public void Replace1By1OnceOnObjects() {
            var s = new[] { new C(1, "s"), new C(2, "b"), new C(3, "g") };
            var expected = new[] { new C(7, "h"), new C(2, "b"), new C(3, "g") };
            var nV = new[] { new C(7, "h"), };
            var res = Replace.MacroExpansion(s, new C(1, "s"), nV);
            Assert.That(res, Is.EqualTo(expected));
        }

        [Test]
        public void Replace1By1OnceOnObjectsWithNull() {
            var s = new[] { new C(1, "s"), null, new C(3, "g") };
            var expected = new[] { new C(7, "h"), null, new C(3, "g") };
            var nV = new[] { new C(7, "h"), };
            var res = Replace.MacroExpansion(s, new C(1, "s"), nV);
            Assert.That(res, Is.EqualTo(expected));
        }
    }
}
