using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Naumenko_Game
{
    /// <summary>
    /// Интерфейс, отвечающий за определение общих свойств объектов мусора
    /// </summary>
    interface ITrash 
    {
        int CategoryId { get; set; }
        PictureBox TrashImage { get; set; }
        int X { get; set; }
        int Y { get; set; }
    }

    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);

        void Notify();
    }

    public interface IObserver
    {
        void Update(ISubject subject);
    }

    public class TrashMovementSubject : ISubject
    {
        internal bool IsMovingTrash;
        internal bool IsThrowingTrash;
        internal GameForm GameForm;
        public TrashMovementSubject(GameForm form)
        {
            GameForm = form;
        }

        public void UpdateTrashMovement(bool isThrowingTrash, bool isMovingTrash)
        {
            IsMovingTrash = isMovingTrash;
            IsThrowingTrash = isThrowingTrash;
            Notify();
        }

        // Ниже идет часть кода, отвечающая за реализацию паттерна Observer
        private readonly List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (IObserver observer in _observers)
            {
                observer.Update(this);
            }
        }
    }
    public class HandMovementSubject : ISubject
    {
        internal readonly Hand ActiveHand;
        internal GameForm ActiveForm;

        public bool MoveLeftTrigger;
        public bool MoveRightTrigger;
        public bool MoveUpTrigger;
        public bool MoveDownTrigger;

        public HandMovementSubject(GameForm form)
        {
            ActiveForm = form;
            ActiveHand = ActiveForm.MainHand;
        }

        public void UpdateMovementTriggers(bool moveLeft, bool moveRight, bool moveUp, bool moveDown)
        {
            MoveDownTrigger = moveDown;
            MoveLeftTrigger = moveLeft;
            MoveRightTrigger = moveRight;
            MoveUpTrigger = moveUp;

            Notify();
        }

        // Ниже идет часть кода, отвечающая за реализацию паттерна Observer
        private readonly List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (IObserver observer in _observers)
            {
                observer.Update(this);
            }
        }
    }

    public class HandMovementObserver : IObserver
    {
        private readonly int _speed = 3;

        public void Update(ISubject sbj)
        {
            if (((HandMovementSubject)sbj).MoveDownTrigger)
                ((HandMovementSubject)sbj).ActiveHand.HandImage.Location = new Point(
                    ((HandMovementSubject)sbj).ActiveHand.HandImage.Location.X,
                    ((HandMovementSubject)sbj).ActiveHand.HandImage.Location.Y + _speed);
            
            if (((HandMovementSubject)sbj).MoveLeftTrigger)
                ((HandMovementSubject)sbj).ActiveHand.HandImage.Location = new Point(
                    ((HandMovementSubject)sbj).ActiveHand.HandImage.Location.X - _speed,
                    ((HandMovementSubject)sbj).ActiveHand.HandImage.Location.Y);
            
            if (((HandMovementSubject)sbj).MoveRightTrigger)
                ((HandMovementSubject)sbj).ActiveHand.HandImage.Location = new Point(
                    ((HandMovementSubject)sbj).ActiveHand.HandImage.Location.X + _speed,
                    ((HandMovementSubject)sbj).ActiveHand.HandImage.Location.Y);
            
            if (((HandMovementSubject)sbj).MoveUpTrigger)
                ((HandMovementSubject)sbj).ActiveHand.HandImage.Location = new Point(
                    ((HandMovementSubject)sbj).ActiveHand.HandImage.Location.X,
                    ((HandMovementSubject)sbj).ActiveHand.HandImage.Location.Y - _speed);
        }
    }

    public class TrashMovementObserver : IObserver
    {
        public void Update(ISubject subj)
        {
            TrashMovementSubject subject = (TrashMovementSubject) subj;
            Hand activeHand = subject.GameForm.MainHand;

            if (subject.IsMovingTrash)
            {
                subject.GameForm.ActiveTrashItem.TrashImage.Location = subject.GameForm.MainHand.HandImage.Location;
                subject.GameForm.ActiveTrashItem.X = subject.GameForm.MainHand.X;
                subject.GameForm.ActiveTrashItem.Y = subject.GameForm.MainHand.Y;
            }
            else
            {
                if (!subject.IsThrowingTrash)
                {
                    if (subject.GameForm.ActiveTrashItem.TrashImage.Bounds.IntersectsWith(activeHand.HandImage.Bounds))
                    {
                        subject.GameForm.ActiveTrashItem.TrashImage.Location = activeHand.HandImage.Location;
                        subject.GameForm.ActiveTrashItem.X = activeHand.X;
                        subject.GameForm.ActiveTrashItem.Y = activeHand.Y;

                        subject.GameForm.MainHand.IsHoldingTrash = true;
                    }
                }
                else
                {
                    subject.GameForm.MainHand.IsHoldingTrash = false;

                    // 
                    foreach (PictureBox pb in subject.GameForm.TrashBins)
                    {
                        if (subject.GameForm.ActiveTrashItem.TrashImage.Bounds.IntersectsWith(pb.Bounds))
                        {
                            subject.GameForm.Controls.Remove(subject.GameForm.ActiveTrashItem.TrashImage);
                            subject.GameForm.ActiveTrashItem = null;
                            subject.GameForm.MainHand.IsHoldingTrash = false;

                            break;
                        }

                    }
                }
            }
        }
    }

    /// <summary>
    /// Класс, определяющий взаимодействие и свойства руки
    /// </summary>
    public class Hand
    {
        internal int X { get; set; }
        internal int Y { get; set; }
        public bool IsHoldingTrash { get; set; }
        public PictureBox HandImage { get; set; }

        public Hand(int x, int y) // конструктор для объекта руки
        {
            HandImage = new PictureBox();
            
            X = x;
            Y = y;
            HandImage.Location = new Point(x, y);
            HandImage.Size = new Size(150,150);
            HandImage.Load(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.FullName + "\\Resources\\hand-icon.png");
        }
    }

    /// <summary>
    /// Класс, определяющий взаимодействие и свойства объекта мусора
    /// </summary>
    public class TrashItem : ITrash
    {
        public int CategoryId { get; set; }
        public PictureBox TrashImage { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public TrashItem(int categoryId, PictureBox trashImage) // конструктор для объекта мусора
        {
            CategoryId = categoryId;
            TrashImage = trashImage;

            X = 0;
            Y = 0;

            trashImage.Location = new Point(X, Y);
        }
    }

    /// <summary>
    /// Класс, отвечающий за появление объектов мусора на экране
    /// </summary>
    static class TrashFactory
    {
        private static readonly string _projectUrl = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.FullName;
        // Путь к папке проекта
        public static TrashItem CreateTrashItem()
        {
            Random rnd = new Random();
            int category = rnd.Next(1, 7);
            PictureBox icon = new PictureBox();
            switch (category)
            {
                case (1):
                    icon.Load(_projectUrl + "\\Resources\\glass-icon.png");
                    break;  // "Стекло" 
                case (2):
                    icon.Load(_projectUrl + "\\Resources\\plastic-icon.png");
                    break;  // "Пластик" 
                case (3):
                    icon.Load(_projectUrl + "\\Resources\\paper-icon.png");
                    break; // "Бумага" 
                case (4):
                    icon.Load(_projectUrl + "\\Resources\\metal-icon.png");
                    break; // "Металл" 
                case (5):
                    icon.Load(_projectUrl + "\\Resources\\food-icon.png");
                    break; // "Пищевые отходы" 
                case (6):
                    icon.Load(_projectUrl + "\\Resources\\bio-icon.png");
                    break; // "Отходы жизнедеятельности"
            }
            icon.Size = new Size(150,150);

            TrashItem trashItem = new TrashItem(category, icon) { X = rnd.Next(30, 170), Y = rnd.Next(30, 170) };

            return trashItem;
        }
    }

    /// <summary>
    /// Класс, определяющий функционал и свойства объекта результата
    /// </summary>
    class Result
    {
        private int Value { get; set; }
        private int CategoryId { get; set; }



    }
}
