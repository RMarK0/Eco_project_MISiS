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
        protected int X { get; set; }
        protected int Y { get; set; }
    }

    class HandProcessor
    {

    }

    class TrashFactory
    {

    }

    class InputProcessor
    {
        
    }

    class Result
    {
        private int Value { get; set; }
        private int CategoryId { get; set; }


    }
}
