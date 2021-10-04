using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Naumenko_Game
{
    interface ITrash
    {
        int CategoryId { get; set; }
        PictureBox TrashImage { get; set; }
        int X { get; set; }
        int Y { get; set; }
    }

    class Hand
    {
        internal int X { get; set; }
        internal int Y { get; set; }
        public bool IsHoldingTrash { get; set; }
        public PictureBox HandImage { get; set; }

        public Hand(int X, int Y)
        {
            HandImage = new PictureBox();
            this.HandImage = HandImage;
            this.X = X;
            this.Y = Y;
            HandImage.Location = new System.Drawing.Point(X, Y);
            HandImage.Size = new Size(150,150);
            HandImage.Load("C:\\Users\\rybal\\Documents\\Visual Studio 2017\\Projects\\Eco_project_MISiS\\Eco_project_MISiS\\Resources\\hand-icon.png");
        }

    }

    class TrashItem : ITrash
    {
        public int CategoryId { get; set; }
        public PictureBox TrashImage { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public TrashItem(int categoryId, PictureBox trashImage)
        {
            this.CategoryId = categoryId;
            this.TrashImage = trashImage;

            X = 0;
            Y = 0;

            trashImage.Location = new System.Drawing.Point(X, Y);
        }
    }

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
                    icon.Load("C:\\Users\\rybal\\Documents\\Visual Studio 2017\\Projects\\Eco_project_MISiS\\Eco_project_MISiS\\Resources\\glass-icon.png");
                    break;  // "Стекло" 
                case (2):
                    icon.Load("C:\\Users\\rybal\\Documents\\Visual Studio 2017\\Projects\\Eco_project_MISiS\\Eco_project_MISiS\\Resources\\plastic-icon.png");
                    break;  // "Пластик" 
                case (3):
                    icon.Load("C:\\Users\\rybal\\Documents\\Visual Studio 2017\\Projects\\Eco_project_MISiS\\Eco_project_MISiS\\Resources\\paper-icon.png");
                    break; // "Бумага" 
                case (4):
                    icon.Load("C:\\Users\\rybal\\Documents\\Visual Studio 2017\\Projects\\Eco_project_MISiS\\Eco_project_MISiS\\Resources\\metal-icon.png");
                    break; // "Металл" 
                case (5):
                    icon.Load("C:\\Users\\rybal\\Documents\\Visual Studio 2017\\Projects\\Eco_project_MISiS\\Eco_project_MISiS\\Resources\\food-icon.png");
                    break; // "Пищевые отходы" 
                case (6):
                    icon.Load("C:\\Users\\rybal\\Documents\\Visual Studio 2017\\Projects\\Eco_project_MISiS\\Eco_project_MISiS\\Resources\\bio-icon.png");
                    break; // "Отходы жизнедеятельности"
            }
            icon.Size = new Size(150,150);

            TrashItem trashItem = new TrashItem(category, icon);
            trashItem.X = rnd.Next(30, 170);
            trashItem.Y = rnd.Next(30, 170);

            return trashItem;
        }
    }

    class InputProcessor
    {
        readonly Hand _inputHand;
        public InputProcessor(Hand hand)
        {
            _inputHand = hand;
        }

        public void MoveHand(bool left, bool right, bool up, bool down)
        {
            if (left)
            {
                _inputHand.X -= 10;
                _inputHand.HandImage.Location = new System.Drawing.Point(_inputHand.HandImage.Location.X - 5, _inputHand.HandImage.Location.Y);
            }
            if (right)
            {
                _inputHand.X += 10;
                _inputHand.HandImage.Location = new System.Drawing.Point(_inputHand.HandImage.Location.X + 5, _inputHand.HandImage.Location.Y);
            }
            if (up)
            {
                _inputHand.Y -= 10;
                _inputHand.HandImage.Location = new System.Drawing.Point(_inputHand.HandImage.Location.X, _inputHand.HandImage.Location.Y - 5);
            }
            if (down)
            {
                _inputHand.Y += 10;
                _inputHand.HandImage.Location = new System.Drawing.Point(_inputHand.HandImage.Location.X, _inputHand.HandImage.Location.Y + 5);
            }
        }

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
