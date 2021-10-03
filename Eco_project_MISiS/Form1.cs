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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Блок кода, отвечающий за регистрацию клавиш
        /// </summary>

        private bool moveRight; // false - значение по умолчанию
        private bool moveLeft; // false - значение по умолчанию

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
        }
    }
}
