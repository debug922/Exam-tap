using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Exam.Exam07092018 {
    public static class Es1 {
        /*
              var list =new List<string>();
           var list1=new List<string>();
           for (int i = 0; i < 4; i++)
           {
           list.Add("aa");
           list1.Add("bb");
           }
           list1.Add("cc");
           var result=list.Apply(list1,(i, i1) =>i+i1 );
           foreach (var i in result)
           {
           Console.WriteLine(i);
           }
           for testing on the main
         */

        public static IEnumerable<T> Apply<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, T> f) {
            if (first == null || second == null || f == null)
                throw new ArgumentNullException();

            using (var enumerable = first.GetEnumerator()) {
                using (var enumerable1 = second.GetEnumerator()) {
                    do {
                        T current1 = default;
                        T current2 = default;
                        var exit1 = enumerable.MoveNext();
                        if (exit1)
                            current1 = enumerable.Current;
                        var exit2 = enumerable1.MoveNext();
                        if (exit2)
                            current2 = enumerable1.Current;
                        if (!(exit1 || exit2))
                            yield break;
                        yield return f(current1, current2);

                    } while (true);
                }
            }
        }
    }

    public class Es2 {
        /*
         1. Input della chiamata sotto test: first deve essere la sequenza 1, 2, 3, second deve essere la
           sequenza 10, 20, 30 e la funzione deve essere nulla.
         */
        [Test]
        public void Apply_NullF_Throws() {
            int[] array = { 1, 2, 3 };
            int[] array1 = { 10, 20, 30 };
            Assert.That(() => array.Apply(array1, null).ToList(), Throws.TypeOf<ArgumentNullException>());
        }
        /*
         2. Input della chiamata sotto test: first deve essere la sequenza "Hello ", "Ciao ", "Bonjour ",
            "Hallo ", second deve essere la sequenza "World", "Mondo", "le Monde", "Welt" e la funzione
            deve essere la concatenazione di stringhe.
            Output atteso: la sequenza "Hello World", "Ciao Mondo", "Bonjour le Monde", "Hallo Welt".
         */
        [Test]
        public void Apply_ValidArgs() {
            string[] array = { "Hello ", "Ciao ", "Bonjour ", "Hallo " };
            string[] array1 = { "World", "Mondo", "le Monde", "Welt" };
            string[] expect = { "Hello World", "Ciao Mondo", "Bonjour le Monde", "Hallo Welt" };
            CollectionAssert.AreEqual(expect, array.Apply(array1, (i, i1) => i + i1));
        }
        /*
         3. Test parametrico con parametro howMany che indica quanti valori del risultato devono essere
           verificati.
           Input della chiamata sotto test: first deve essere una sequenza non nulla e non vuota e second
           una sequenza infinita a vostra scelta, purché generino come risultato di apply una sequenza
           di elementi tutti diversi fra loro.
           Il test deve verificare che i primi howMany elementi del risultato coincidano con quelli attesi (che
           dipendono dalla scelta di argomenti che avete fatto).
         */

    }

    public class Es3 {
        /*
         Implementare la classe ComplexNumber che rappresenta i numeri complessi con le quattro operazioni
           (somma, sottrazione, moltiplicazione e divisione), l’uguaglianza e conversione implicita da reale a
           complesso ed esplicita da complesso a reale (corretta solo se la parte immaginaria è zero), in modo
           tale che il seguente frammento di codice sia staticamente corretto
           var c1 = new Es3.ComplexNumber(3, 10);
            double x = c1.Re;
            double y = c1.Im;
            Es3.ComplexNumber c2 = c1 + c1;
            double err = (double)c1;
            Console.WriteLine(err);
            Es3.ComplexNumber c3 = 6.8;
            c1 = (c2 - c1) / c3;
            c2 = c1 * c3;
         */
        public class ComplexNumber {
            public int Re { get; set; }
            public int Im { get; set; }

            public ComplexNumber(int re, int im) {
                Re = re;
                Im = im;
            }
            public override bool Equals(object obj) {
                if (obj == null || GetType() != obj.GetType())
                    return false;
                var o = (ComplexNumber)obj;
                return Re == o.Re && Im == o.Im;
            }

            public override int GetHashCode() {
                return Re.GetHashCode() + Im.GetHashCode();
            }

            public static ComplexNumber operator +(ComplexNumber one, ComplexNumber two) {
                //• (a + bi) + (c + di) == (a + c) + (b + d)i
                return new ComplexNumber(one.Re + two.Re, one.Im + two.Im);
            }
            public static ComplexNumber operator -(ComplexNumber one, ComplexNumber two) {
                //• (a + bi) − (c + di) == (a − c) + (b − d)i
                return new ComplexNumber(one.Re - two.Re, one.Im - two.Im);
            }
            public static ComplexNumber operator *(ComplexNumber one, ComplexNumber two) {
                //• (a + bi) ∗ (c + di) == (ac − bd) + (ad + bc)i
                return new ComplexNumber((one.Re * two.Re - one.Im - two.Im), (one.Re * two.Im + one.Im * two.Re));
            }
            public static ComplexNumber operator /(ComplexNumber one, ComplexNumber two) {
                //(a+bi)/(c+di) 
                if (two.Re == 0 || two.Im == 0)
                    throw new DivideByZeroException();
                int div = two.Re * two.Re + two.Im * two.Im;
                return new ComplexNumber((one.Re * two.Re + one.Im - two.Im) / div, (one.Re * two.Im + one.Im * two.Re) / div);
            }
            public static explicit operator double(ComplexNumber complex) {
                if (complex.Im != 0)
                    throw new InvalidOperationException();
                string combinedLeftRight = complex.Re + "." + complex.Im;
                return Convert.ToDouble(combinedLeftRight);
            }
            public static implicit operator ComplexNumber(double complex) {

                return new ComplexNumber((int)complex, (int)(((decimal)complex % 1) * 100));
            }
        }
    }
}
