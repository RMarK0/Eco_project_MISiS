//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Naumenko_Game
//{
//    public interface ISubject
//    {
//        void Attach(IObserver observer);
//        void Detach(IObserver observer);

//        void Notify();
//    }

//    public interface IObserver
//    {
//        void Update(ISubject subject);
//    }

//    public class Subject : ISubject
//    {
//        public int State { get; set; } = -0;

//        private List<IObserver> _observers = new List<IObserver>();

//        public void Attach(IObserver observer)
//        {
//            Console.WriteLine("Subject: Attached new observer");
//            this._observers.Add(observer);
//        }

//        public void Detach(IObserver observer)
//        {
//            this._observers.Remove(observer);
//            Console.WriteLine("Subject: Detached an observer");
//        }

//        public void Notify()
//        {
//            Console.WriteLine("Subject: Notifying observers...");

//            foreach (var observer in _observers)
//            {
//                observer.Update(this);
//            }
//        }

//    }
//}
