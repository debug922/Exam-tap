using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Exam.Exam020218
{
    public static class Es1
    {
        public interface IExam
        {
            string Name { get; set; }
            DateTime Date { get; set; }
            int Grade { get; set; }
            IStudent Student { get; set; }
        }
        public interface IStudent {/* .... */ }
       
        public static IEnumerable<IExam> Career(this IEnumerable<IExam> register,IStudent s)
        {
            if(register==null || s==null)
                throw new ArgumentNullException();
            return from exam in register
                where exam.Student.Equals(s)
                select exam;
        }
        //• Scrivere l’extension-method LastRegistration che seleziona da una sequenza di esami quello sostenuto
        //più recentemente da uno specifico studente e restituisce null se non risultano esami sostenuti dallo studente.
        public static IExam LastRegistration(this IEnumerable<IExam> register,IStudent s)
        {
            var exams = register.Career(s);
            if (!exams.Any())
                return null;
            return exams.OrderByDescending(x => x.Date).First();
        }
        //• È possibile dare implementazioni dei metodi richiesti ai punti precedenti in grado di terminare anche in caso le
        // sequenze sorgente siano infinite? Se un sequenza viene convertita in lista o array direi di si :/

    }

    public class Es2
    {
        //Modificare il seguente codice in modo da renderne facile lo unit testing
        public class B
        {
            public int Y { get; }
            public int X { get; }
            public B()
            {
                X = 3;
                Y = 7;
            }
            public B(int x)
            {
                X = x;
                Y = 2 * x;
            }
            bool M1(int a, int b)
            {
                return a <=X && b>=Y;
            }
        }  
    }

    public class Es3
    {
        public interface ID
        {
            // return the expected value for a D
            int CurrentValue();
        }
        public interface IC
        {
            // An oracle to guess D values
            ID Oracle { get; }
            // returns .... IRRILEVANTE AI FINI DI QUESTO ESERCIZIO
            // throws ArgumentException if x less than zero
            int M(int x);
            // returns true if x is greater than Oracle . CurrentValue ()
            bool MeetsExpectations(int x);
        }
        public class C : IC
        {
            public ID Oracle { get; }
            public C(ID d){Oracle = d;}
            public int M(int x){/* .... */  return 0; }
            public bool MeetsExpectations(int x){/* .... */ return false; }
        }
        public class D : ID
        {
            public int CurrentValue(){/* .... */return 0;}
        }
       
        //1. la chiamata di M su −7 sollevi una ArgumentException;
        private C c;
        private D1 d;
        [SetUp]
        public void Setup()
        {
            d=new D1();
            c = new C(d);
        }
        [Test]
        public void C_M_Throw()
        {  
            Assert.That(()=>c.M(-7),Throws.TypeOf<ArgumentException>());
        }
        // 2. la chiamata di MeetsExpectations su 20 restituisca true se Oracle.CurrentValue() restituisce 10;
        [Test]
        public void C_MeetsExpectations_ValidArg()
        {
            Assert.Multiple(()=>
            {
                Assert.That(c.MeetsExpectations(20), Is.True);
                Assert.That(c.Oracle, Is.EqualTo(10));
            });

        }
        // 3. una chiamata di MeetsExpectations richieda una e una sola chiamata a Oracle.CurrentValue
        [Test]
        public void C_MeetsExpectations_Count()
        {
            c.MeetsExpectations(20);
            Assert.That(d.count,Is.EqualTo(1));
        }
        public class D1:D
        {
            public int count { get; set; }

            new int  CurrentValue()
            {
                count++;
                return base.CurrentValue();
            }
        }
    }
}
