using System;
using System.Collections.Generic;
using System.Drawing;
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

    /// <summary>
    /// Класс, определяющий взаимодействие и свойства руки
    /// </summary>
    class Hand
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
            HandImage.Location = new System.Drawing.Point(x, y);
            HandImage.Size = new Size(150,150);
            HandImage.Load(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.FullName + "\\Resources\\hand-icon.png");
        }
    }

    /// <summary>
    /// Класс, определяющий взаимодействие и свойства объекта мусора
    /// </summary>
    class TrashItem : ITrash
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

            trashImage.Location = new System.Drawing.Point(X, Y);
        }
    }

    /// <summary>
    /// Класс, отвечающий за появление объектов мусора на экране
    /// </summary>
    static class TrashFactory
    {
        public static TrashItem CreateTrashItem(PictureBox trashImage)
        {
            Random rnd = new Random();
            int category = rnd.Next(1, 7);
            PictureBox icon = new PictureBox();
            switch (category)
            {
                case (1):
                    icon.Load(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.FullName + "\\Resources\\glass-icon.png");
                    break;  // "Стекло" 
                case (2):
                    icon.Load(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.FullName + "\\Resources\\plastic-icon.png");
                    break;  // "Пластик" 
                case (3):
                    icon.Load(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.FullName + "\\Resources\\paper-icon.png");
                    break; // "Бумага" 
                case (4):
                    icon.Load(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.FullName + "\\Resources\\metal-icon.png");
                    break; // "Металл" 
                case (5):
                    icon.Load(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.FullName + "\\Resources\\food-icon.png");
                    break; // "Пищевые отходы" 
                case (6):
                    icon.Load(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.FullName + "\\Resources\\bio-icon.png");
                    break; // "Отходы жизнедеятельности"
            }
            icon.Size = new Size(150,150);

            TrashItem trashItem = new TrashItem(category, icon) { X = rnd.Next(30, 170), Y = rnd.Next(30, 170) };

            return trashItem;
        }
    }

    /// <summary>
    /// Класс, отвечающий за обработку введенных клавиш
    /// </summary>
    class InputProcessor
    {
        readonly Hand _inputHand;
        public InputProcessor(Hand hand)
        {
            _inputHand = hand;
        }

        public void MoveHand(bool left, bool right, bool up, bool down)
        {
            int speed = 3;

            if (left)
                _inputHand.HandImage.Location = new System.Drawing.Point(_inputHand.HandImage.Location.X - speed, _inputHand.HandImage.Location.Y);
            
            if (right)
                _inputHand.HandImage.Location = new System.Drawing.Point(_inputHand.HandImage.Location.X + speed, _inputHand.HandImage.Location.Y);
            
            if (up)
                _inputHand.HandImage.Location = new System.Drawing.Point(_inputHand.HandImage.Location.X, _inputHand.HandImage.Location.Y - speed);
            
            if (down)
                _inputHand.HandImage.Location = new System.Drawing.Point(_inputHand.HandImage.Location.X, _inputHand.HandImage.Location.Y + speed);
            
        }

        /// <summary>
        /// Метод, позволяющий активной руке схватить объект мусора
        /// </summary>
        /// <param name="trash">Объект мусора, который необходимо схватить</param>
        public void GrabTrash(TrashItem trash)
        {
            if (Math.Abs(trash.TrashImage.Location.X - _inputHand.HandImage.Location.X) < 30 &&
                Math.Abs(trash.TrashImage.Location.Y - _inputHand.HandImage.Location.Y) < 30)
            {
                trash.X = _inputHand.X;
                trash.Y = _inputHand.Y;
                trash.TrashImage.Location = _inputHand.HandImage.Location;

                _inputHand.IsHoldingTrash = true;
            }
        }

        public void ThrowTrash(ref TrashItem trash, Form1 form)
        {
            _inputHand.IsHoldingTrash = false;
            foreach (PictureBox pb in form.TrashBins)
            {
                if (trash.TrashImage.Bounds.IntersectsWith(pb.Bounds))
                {
                    form.Controls.Remove(trash.TrashImage);
                    trash = null;
                    break;
                }
            }
        }

        public void MoveTrash(TrashItem trash)
        {
            trash.TrashImage.Location = _inputHand.HandImage.Location;
        }
    }

    class Result
    {
        private int Value { get; set; }
        private int CategoryId { get; set; }


    }
}
