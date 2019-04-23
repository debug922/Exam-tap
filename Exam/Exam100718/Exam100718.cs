using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Exam.Exam100718 {
    public static class Es1 {
        public static IEnumerable<Func<int, int>> LineProvider(this IEnumerable<int> slopes, IEnumerable<int> yIntercepts) {
            if (slopes == null || yIntercepts == null)
                throw new ArgumentNullException();
            using (var enumerable = slopes.GetEnumerator()) {
                using (var enumerable1 = yIntercepts.GetEnumerator()) {
                    var i = 0;
                    var j = 0;
                    do {
                        var currentJ = 0;
                        if (enumerable1.MoveNext()) {
                            currentJ = enumerable1.Current;
                            j++;
                        }
                        if (!enumerable.MoveNext())
                            break;
                        var currentI = enumerable.Current;
                        i++;
                        yield return x => currentI * x + currentJ;
                    } while (true);
                    if (j > i)
                        throw new ArgumentException();
                }
            }
        }
    }


    public class Es2 {
        [Test]
        public void LineProvider_NullSlopes_Throw() {
            IEnumerable<int> slopes = null;
            var y = new List<int>();
            y.Add(1);
            Assert.That(() => slopes.LineProvider(y).ToList(), Throws.TypeOf<ArgumentNullException>());
        }
        /*
         2. Input della chiamata sotto test: slopes deve essere la sequenza 1, 0, −7, yIntercepts deve essere la sequenza 5, 4.
           Output atteso: la sequenza che contiene tre elementi, la rappresentazione di y = x + 5, y = 4 e y = −7 ∗
           x. Verificare che l’elemento i-esimo effettivamente sia una rappresentazione corretta della funzione richiesta
           controllando solo i 4 punti per cui x = 0, x = 100, x = 1 e x = −1.
         */
        [Test]
        public void LineProvider_ValidArgs() {
            int[] array = { 1, 0, -7 };
            int[] array1 = { 5, 4 };
            int[] expected = { 5, 4, 0, 105, 4, -700, 6, 4, -7, 4, 4, 7 };
            var fun = array.LineProvider(array1);
            var values = new List<int>();
            var enumerable = fun.ToList();
            ResultFunc(0, enumerable, ref values);
            ResultFunc(100, enumerable, ref values);
            ResultFunc(1, enumerable, ref values);
            ResultFunc(-1, enumerable, ref values);
            CollectionAssert.AreEqual(expected, values);
        }
        /*
         3. Test parametrico con parametri:
           Il test deve verificare (parzialmente) la correttezza della funzione di posizione whichLine nel risultato della
           chiamata, controllando che sugli interi in [minX, maxX ] la funzione restituisca i valori in expected.
           LineProvider rappresenti la funzione 10 ∗ x + 100 controllando che il suo risultato su x in [0, 7] 
         */
        [Test]
        [TestCase(10, 0, 7, new[] { 100, 110, 120, 130, 140, 150, 160, 170 })]
        public void LineProvider_ValidArgs1(int whichLine, int minX, int maxX, int[] expected) {
            IEnumerable<int> slopes = Enumerable.Range(0, int.MaxValue);
            int[] yIntercepts = { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110 };
            var fun = new List<int> { slopes.ElementAtOrDefault(whichLine) }
                .LineProvider(new List<int> { yIntercepts.ElementAtOrDefault(whichLine) });
            var values = new List<int>();
            for (var i = minX; i <= maxX; i++) {
                foreach (var func in fun) {
                    values.Add(func(i));
                }
            }
            CollectionAssert.AreEqual(expected, values);
        }

        private void ResultFunc(int arg, IEnumerable<Func<int, int>> fun, ref List<int> values) {
            // values.AddRange(fun.Select(func => func(arg)));
            foreach (var func in fun) {
                values.Add(func(arg));
            }
        }
    }

    public class Es3 {
        public interface IPeople {
            int Id { get; }
            string Name { get; }
        }
        public interface IThing {
            int Id { get; }
            string Description { get; }
            int OwnerId { get; }
        }

        public interface ILoan {
            IPeople People { get; }
            IThing Thing { get; }
            DateTime DateTime { get; }
        }
        public interface ICommunityLoan<T, TU>
            where T : IPeople
            where TU : IThing {
            void AddObj(in TU thing, in T person);
            void RemoveObj(in TU thing);
            TU TakeObj(in T person, in int id);
            void ReturnAnObj(in T person, in TU obj);
            IEnumerable<T> AllPeople();
            IEnumerable<ILoan> AllLoansNow();
            IEnumerable<ILoan> AllLoansOne(in TU thing, in bool onlyNotEnded);
            bool SearchObj(in string description);

            /*     
              2. I parametri nell’interfaccia al punto precedente possono essere dichiarati co/contro varianti? perché?
              perche sono parametri solo in lettura
               */
        }

        public class CommunityLoan : ICommunityLoan<IPeople, IThing> {
            private List<IPeople> PeopleOkShare { get; }
            private List<IThing> Things { get; }
            private List<ILoan> Loans { get; }
            public CommunityLoan() {
                PeopleOkShare = new List<IPeople>();
                Things = new List<IThing>();
                Loans = new List<ILoan>();
            }

            public void AddObj(in IThing thing, in IPeople person) {
                if (thing == null || person == null)
                    throw new ArgumentNullException();
                if (SearchThing(thing.Id) != null)
                    throw new InvalidOperationException();
                Things.Add(thing);
                if (!SearchPerson(thing.OwnerId))
                    PeopleOkShare.Add(person);
            }

            public void RemoveObj(in IThing thing) {
                throw new NotImplementedException();
            }
            public IThing TakeObj(in IPeople person, in int id) {
                if (person == null)
                    throw new ArgumentNullException();
                var obj = SearchThing(id);
                if ((!SearchPerson(person.Id)) || SearchLoan(id) || obj == null)
                    throw new InvalidOperationException();
                ///Loans.Add();
                return obj;
            }

            public void ReturnAnObj(in IPeople person, in IThing obj) {
                throw new NotImplementedException();
            }

            public IEnumerable<IPeople> AllPeople() {
                return PeopleOkShare;
            }

            public IEnumerable<ILoan> AllLoansNow() {
                foreach (var loan in Loans) {
                    if (DateTime.Now > loan.DateTime)
                        continue;
                    yield return loan;
                }
            }

            public IEnumerable<ILoan> AllLoansOne(in IThing thing, in bool onlyNotEnded) {
                List<ILoan> list = new List<ILoan>();
                foreach (var loan in Loans) {
                    if (thing.Id == loan.Thing.Id) {
                        if (onlyNotEnded && DateTime.Now > loan.DateTime)
                            continue;
                        list.Add(loan);
                    }
                }

                return list;
            }

            public bool SearchObj(in string description) {
                throw new NotImplementedException();
            }

            private IThing SearchThing(int thing) {
                foreach (var thing1 in Things)
                    if (thing1.Id == thing)
                        return thing1;
                return null;
            }

            private bool SearchPerson(int id) {
                foreach (var people in PeopleOkShare)
                    if (people.Id == id)
                        return true;
                return false;
            }

            private bool SearchLoan(int id) {
                foreach (var loan in Loans) {
                    if (loan.Thing.Id == id && loan.DateTime > DateTime.Now)
                        return true;
                }
                return false;
            }
        }

    }
}

