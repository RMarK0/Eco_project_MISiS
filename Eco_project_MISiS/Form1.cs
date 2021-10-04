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
        private TrashItem ActiveTrashItem;

        public PictureBox[] TrashBins = new PictureBox[6];

        private void Form1_Load(object sender, EventArgs e)
        {
            categoryDictionary = new Dictionary<int, string>
            {
                { 1, "Стекло" },
                { 2, "Пластик" },
                { 3, "Бумага" },
                { 4, "Металл" },
                { 5, "Пищевые отходы" },
                { 6, "Отходы жизнедеятельности" }
            };

            mainHand = new Hand((ClientSize.Width - 100) / 2, (ClientSize.Height - 100) / 2);
            InputProcessor = new InputProcessor(mainHand);
            Controls.Add(mainHand.HandImage);

            FormBorderStyle = FormBorderStyle.FixedSingle;
            Size = new Size(1280, 720);
            MaximizeBox = false;

            int locX = 0;
            for (int i = 0; i < 6; i++)
            {
                string url = "";
                PictureBox pb = new PictureBox { Size = new Size(200, 150), Location = new Point(locX, 530) };
                locX += 200;
                switch (i)
                {
                    case (1):
                        url =
                            "C:\\Users\\rybal\\Documents\\Visual Studio 2017\\Projects\\Eco_project_MISiS\\Eco_project_MISiS\\Resources\\bin-food.png";
                        break;
                    case (2):
                        url =
                            "C:\\Users\\rybal\\Documents\\Visual Studio 2017\\Projects\\Eco_project_MISiS\\Eco_project_MISiS\\Resources\\bin-glass.png";
                        break;
                    case (3):
                        url =
                            "C:\\Users\\rybal\\Documents\\Visual Studio 2017\\Projects\\Eco_project_MISiS\\Eco_project_MISiS\\Resources\\bin-metal.png";
                        break;
                    case (4):
                        url =
                            "C:\\Users\\rybal\\Documents\\Visual Studio 2017\\Projects\\Eco_project_MISiS\\Eco_project_MISiS\\Resources\\bin-paper.png";
                        break;
                    case (5):
                        url =
                            "C:\\Users\\rybal\\Documents\\Visual Studio 2017\\Projects\\Eco_project_MISiS\\Eco_project_MISiS\\Resources\\bin-plastic.png";
                        break;
                    case (0):
                        url =
                            "C:\\Users\\rybal\\Documents\\Visual Studio 2017\\Projects\\Eco_project_MISiS\\Eco_project_MISiS\\Resources\\bin-bio.png";
                        break;
                }
                pb.Load(url);
                Controls.Add(pb);
                TrashBins[i] = pb;
            }
        }

        private bool moveRight; // false - значение по умолчанию
        private bool moveLeft; // false - значение по умолчанию
        private bool moveUp; // false - значение по умолчанию
        private bool moveDown; // false - значение по умолчанию

        private void Form1_KeyDown(object sender, KeyEventArgs e) // Если клавиша нажата, то двигаться 
        {
            if (e.KeyCode == Keys.Right)
                moveRight = true;
            
            if (e.KeyCode == Keys.Left)
                moveLeft = true;
            
            if (e.KeyCode == Keys.Up)
                moveUp = true;

            if (e.KeyCode == Keys.Down)
                moveDown = true;

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) // Если клавиша была отжата, то перестать двигаться 
        {
            if (e.KeyCode == Keys.Right)
                moveRight = false;
            
            if (e.KeyCode == Keys.Left)
                moveLeft = false;

            if (e.KeyCode == Keys.Up)
                moveUp = false;

            if (e.KeyCode == Keys.Down)
                moveDown = false;

            if (e.KeyCode == Keys.Enter && ActiveTrashItem != null)
                if (!mainHand.IsHoldingTrash)
                    InputProcessor.GrabTrash(ActiveTrashItem);
                else
                    InputProcessor.ThrowTrash(ref ActiveTrashItem, this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ActiveTrashItem == null)
            {
                ActiveTrashItem = TrashFactory.CreateTrashItem(null);
                Controls.Add(ActiveTrashItem.TrashImage);
            }
            

            try
            {
                InputProcessor.MoveHand(moveLeft, moveRight, moveUp, moveDown);
                if (mainHand.IsHoldingTrash && ActiveTrashItem != null)
                {
                    InputProcessor.MoveTrash(ActiveTrashItem);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
