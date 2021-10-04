using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Naumenko_Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Dictionary<int, string> categoryDictionary;
        private Hand mainHand;
        private InputProcessor InputProcessor;
        private TrashItem ActiveTrashItem = null;



        private void Form1_Load(object sender, EventArgs e)
        {
            categoryDictionary = new Dictionary<int, string>();
            categoryDictionary.Add(1, "Стекло");
            categoryDictionary.Add(2, "Пластик");
            categoryDictionary.Add(3, "Бумага");
            categoryDictionary.Add(4, "Металл");
            categoryDictionary.Add(5, "Пищевые отходы");
            categoryDictionary.Add(6, "Отходы жизнедеятельности");

            InputProcessor = new InputProcessor(mainHand);
        }

        private bool moveRight; // false - значение по умолчанию
        private bool moveLeft; // false - значение по умолчанию
        private bool moveUp; // false - значение по умолчанию
        private bool moveDown; // false - значение по умолчанию

        private void Form1_KeyDown(object sender, KeyEventArgs e) // Если клавиша нажата, то двигаться 
        {
            if (e.KeyCode == Keys.Right)
            {
                moveRight = true;
            }

            if (e.KeyCode == Keys.Left)
            {
                moveLeft = true;
            }

            if (e.KeyCode == Keys.Up)
            {
                moveUp = true;
            }

            if (e.KeyCode == Keys.Down)
            {
                moveDown = true;
            }

            try
            {
                InputProcessor.MoveHand(moveLeft, moveRight, moveUp, moveDown);
                if (mainHand.isHoldingTrash && ActiveTrashItem != null)
                {
                    InputProcessor.MoveTrash(moveLeft, moveRight, moveUp, moveDown, ActiveTrashItem);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) // Если клавиша была отжата, то перестать двигаться 
        {
            if (e.KeyCode == Keys.Right)
            {
                moveRight = false;
            }

            if (e.KeyCode == Keys.Left)
            {
                moveLeft = false;
            }

            if (e.KeyCode == Keys.Up)
            {
                moveUp = false;
            }

            if (e.KeyCode == Keys.Down)
            {
                moveDown = false;
            }

            try
            {
                InputProcessor.MoveHand(moveLeft, moveRight, moveUp, moveDown);
                if (mainHand.isHoldingTrash && ActiveTrashItem != null)
                {
                    InputProcessor.MoveTrash(moveLeft, moveRight, moveUp, moveDown, ActiveTrashItem);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ActiveTrashItem == null)
                ActiveTrashItem = TrashFactory.CreateTrashItem(null);


        }
    }
}
