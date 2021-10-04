using System;
using System.Collections.Generic;
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
        public bool isHoldingTrash { get; set; }
        public PictureBox HandImage { get; set; }

        public Hand(PictureBox HandImage)
        {
            this.HandImage = HandImage;

            X = 100;
            Y = 100;
            HandImage.Location = new System.Drawing.Point(X, Y);
            HandImage.Load("test");
        }

    }

    class TrashItem : ITrash
    {
        public int CategoryId { get; set; }
        public PictureBox TrashImage { get; set; }
        public int X { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public TrashItem(int CategoryId, PictureBox TrashImage)
        {
            this.CategoryId = CategoryId;
            this.TrashImage = TrashImage;

            X = 0;
            Y = 0;

            TrashImage.Location = new System.Drawing.Point(X, Y);
            TrashImage.Load("test");
        }
    }

    static class TrashFactory
    {
        public static TrashItem CreateTrashItem(PictureBox TrashImage)
        {
            Random rnd = new Random();
            TrashItem trashItem = new TrashItem(rnd.Next(1, 7), TrashImage); // изменить null на нормальную картинку
            trashItem.X = rnd.Next(30, 170);
            trashItem.Y = rnd.Next(30, 170);

            return trashItem;
        }
    }

    class InputProcessor
    {
        Hand inputHand;
        public InputProcessor(Hand hand)
        {
            inputHand = hand;
        }

        public void MoveHand(bool left, bool right, bool up, bool down)
        {
            if (left)
            {
                inputHand.X -= 10;
                inputHand.HandImage.Location = new System.Drawing.Point(inputHand.HandImage.Location.X - 10, inputHand.HandImage.Location.Y);
            }
            if (right)
            {
                inputHand.X += 10;
                inputHand.HandImage.Location = new System.Drawing.Point(inputHand.HandImage.Location.X + 10, inputHand.HandImage.Location.Y);
            }
            if (up)
            {
                inputHand.Y -= 10;
                inputHand.HandImage.Location = new System.Drawing.Point(inputHand.HandImage.Location.X, inputHand.HandImage.Location.Y - 10);
            }
            if (down)
            {
                inputHand.Y += 10;
                inputHand.HandImage.Location = new System.Drawing.Point(inputHand.HandImage.Location.X - 10, inputHand.HandImage.Location.Y + 10);
            }
        }

        public void MoveTrash(bool left, bool right, bool up, bool down, TrashItem trash)
        {
            trash.TrashImage.Location = new System.Drawing.Point(inputHand.X, inputHand.Y);
        }
    }

    class Result
    {
        private int Value { get; set; }
        private int CategoryId { get; set; }


    }
}
