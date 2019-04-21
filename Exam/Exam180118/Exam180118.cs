using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Exam.Exam180118
{

    public static class Es1
    {
        //first version not infinite
        public static int CountUntil1<T>(this IEnumerable<T> seq,Func<T,bool> matchPredicate,Func<T,bool> exitCondition=null)
        {
            if (seq == null || matchPredicate == null)
                throw new ArgumentNullException();
            int count=0;
            var cond = exitCondition ?? ((x) => false);

            foreach (var x in seq)
            {
                if (cond(x))
                    break;
                if (matchPredicate(x))
                    count++;
            }
            return count;
        }
        public static int CountUntil<T>(this IEnumerable<T> seq, Func<T, bool> matchPredicate, Func<T, bool> exitCondition = null)
        {
            if (seq == null || matchPredicate == null)
                throw new ArgumentNullException();
            var cond = exitCondition ?? ((x) => false);
            return seq.TakeWhile(x => !cond(x)).Count(matchPredicate);
        }
    }
    public class Es2
    {
        [Test]
        public void CountUntil_ValidArg()
        {
            Assert.That("cippalippa".CountUntil(char.IsDigit),Is.EqualTo(0));
        }
        
        [Test]
        public void CountUntil_ValidArg1()
        {
            Assert.That(new[]{1,2,3,4,5,6,7,8,9,10}.CountUntil((x)=>x%2==0, x=>x>4), Is.EqualTo(2));
        }
        //2. Sarebbe possibile coprire anche l’esempio (7) con un test? Se sì, darne un’implementazione, altrimenti motivare
        // perché sarebbe impossibile 
        //7. restituisca 2 su seq=tutti gli int positivi quando matchPredicate =“numero pari” ed exitCondition =“numero maggiore di 4”.
    }
    public static class Es3{
        public static int CountPositive(int[] array)
        {
            return array.CountUntil(x => x > 0);
        }
        //• static int StringLength(string s), che calcola (in modo “buffo”/strano) la lunghezza di s, senza usare la
        // proprietà Length
        public static int StringLength(string s)
        {
            return s.CountUntil(x => true);
        }
        //• static int FirstDigitPosition(String s), che restituisce la posizione della prima cifra contenuta in s, oppure
        // la sua lunghezza se non sono presenti cifre. Qui vi potrebbe tornare utile il metodo Char.IsDigit, che potete
        // usare (ma non invocare).
        public static int FirstDigitPosition(String s)
        {
            return s.CountUntil(x => true, char.IsDigit);
        }

    }


}
