using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        private Dictionary<int, string> _categoryDictionary; // создаем словарь категорий мусора - он понадобится для вычисления результата для каждой категории
        private Hand _mainHand; // создаем объект руки
        private InputProcessor _inputProcessor; // создаем объект, отвечающий за обработку введенных клавиш
        private TrashItem _activeTrashItem; // создаем объект мусора - он может быть только один, и как только он выкидывается, появляется новый
        private readonly string _projectUri = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.FullName;
        // создаем объект, который получает путь к нашему проекту. Впоследствии это понадобится для получения пути для иконок и тд

        public PictureBox[] TrashBins = new PictureBox[6]; // создаем массив мусорок

        private void Form1_Load(object sender, EventArgs e) // код в этом методе выполняется при загрузке программы
        {
            _categoryDictionary = new Dictionary<int, string> // вносим в массив наши категории мусора
            {
                { 1, "Стекло" },
                { 2, "Пластик" },
                { 3, "Бумага" },
                { 4, "Металл" },
                { 5, "Пищевые отходы" },
                { 6, "Отходы жизнедеятельности" }
            };

            _mainHand = new Hand(ClientSize.Width/ 2, ClientSize.Height/ 2); // определяем местоположение руки в момент загрузки
            _inputProcessor = new InputProcessor(_mainHand); // подключаем руку к обработчику введенных клавиш
            Controls.Add(_mainHand.HandImage); // выводим руку на экран

            // эти 3 параметра отвечают за вид окна программы
            FormBorderStyle = FormBorderStyle.FixedSingle; // запрещаем менять размер окна
            Size = new Size(1200, 720); // определяем размер окна
            MaximizeBox = false; // запрещаем растягивать окно на весь экран

            int locX = 0; // временная переменная для следующего цикла
            for (int i = 0; i < 6; i++) // этот цикл отвечает за размещение мусорок
            {
                string url = ""; // временная переменная для получения пути к иконкам
                PictureBox pb = new PictureBox { Size = new Size(200, 150), Location = new Point(locX, 530) };
                // создаем новый объект картинки для мусорки, определяем его размер и местоположение

                locX += 200; // увеличиваем переменную для дальнейшего создания
                switch (i) // в каждый цикл выполняется какое-то одно из нижеописанных действий
                {
                    case (1):
                        url = _projectUri + "\\Resources\\bin-food.png"; // задаем путь к иконке мусорки
                        break;
                    case (2):
                        url = _projectUri + "\\Resources\\bin-glass.png";
                        break;
                    case (3):
                        url = _projectUri + "\\Resources\\bin-metal.png";
                        break;
                    case (4):
                        url = _projectUri + "\\Resources\\bin-paper.png";
                        break;
                    case (5):
                        url = _projectUri + "\\Resources\\bin-plastic.png";
                        break;
                    case (0):
                        url = _projectUri + "\\Resources\\bin-bio.png";
                        break;
                }
                pb.Load(url); // загружаем иконку
                Controls.Add(pb); // добавляем мусорку на экран
                TrashBins[i] = pb; // добавляем мусорку в массив мусорок
            }
        }

        // эти 4 переменные отвечают за движение по осям X и Y
        private bool moveRight; 
        private bool moveLeft; 
        private bool moveUp; 
        private bool moveDown; 

        private void Form1_KeyDown(object sender, KeyEventArgs e) // Если клавиша была нажата
        {
            if (e.KeyCode == Keys.Right) // если нажата стрелка вправо
                moveRight = true;
            
            if (e.KeyCode == Keys.Left) // если нажата стрелка влево
                moveLeft = true;
            
            if (e.KeyCode == Keys.Up) // если нажата стрелка вверх
                moveUp = true;

            if (e.KeyCode == Keys.Down) // если нажата стрелка вниз
                moveDown = true;

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) // Если клавиша была отжата
        {
            if (e.KeyCode == Keys.Right) // если нажата стрелка вправо
                moveRight = false;

            if (e.KeyCode == Keys.Left) // если нажата стрелка влево
                moveLeft = false;

            if (e.KeyCode == Keys.Up) // если нажата стрелка вверх
                moveUp = false;

            if (e.KeyCode == Keys.Down) // если нажата стрелка вниз
                moveDown = false;

            if (e.KeyCode == Keys.Enter && _activeTrashItem != null) // обработка захвата мусора рукой
                if (!_mainHand.IsHoldingTrash) // если рука не держит мусор
                    _inputProcessor.GrabTrash(_activeTrashItem); // взять мусор в руку
                else                           // если рука держит мусор
                    _inputProcessor.ThrowTrash(ref _activeTrashItem, this); // выбросить мусор из руки
        }

        private void timer1_Tick(object sender, EventArgs e) // метод, вызывающийся каждый tick таймера
        {
            if (_activeTrashItem == null) // если объект мусора был обнулен
            {
                _activeTrashItem = TrashFactory.CreateTrashItem(null); // создаем новый объект
                Controls.Add(_activeTrashItem.TrashImage); // отображаем его на экране
            }
            
            try
            {
                _inputProcessor.MoveHand(moveLeft, moveRight, moveUp, moveDown); // двигать руку по осям, исходя из тех 4 переменных
                if (_mainHand.IsHoldingTrash && _activeTrashItem != null) // если рука держит мусор, и если объект мусора существует
                {
                    _inputProcessor.MoveTrash(_activeTrashItem); // двигать мусор вместе с рукой
                }
            }
            catch (Exception) { } // игнорировать ошибки 
        }
    }
}
