using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Игра
{
    public partial class Form1 : Form
    {
        public Game newGame; // Объект - Игра

        public bool flag = false; // Флаг состояния кликов (False - не было первого клика, True - был первый клик)
        
        int FposX = 0, FposY = 0; // Координаты клика

        /// <summary>
        ///  Конструктор - инициализация формы
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  Отрисовка формы
        /// </summary>
        /// <param name="e">Событие - рисование</param>
        /// <param name="sender">Объект обращения</param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.StripScore.Text = "Score: _" + Convert.ToString(newGame.newBoard.Score); // Выводит кол-во очков в поле Score
            newGame.newBoard.Draw(e); // Рисует игровое поле - обращение к методу Draw из класса Board
        }

        /// <summary>
        ///  Обработка клика мыши
        /// </summary>
        /// <param name="e">Событие - клик мыши</param>
        /// <param name="sender">Объект обращения</param>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if(!flag) // проверяем состояние флана
            {
                if(flag = newGame.newBoard.FirstClick(e)) // проверяем характер первого клика - True - ожидание 2 клика, False - была активация
                {
                    FposX = (int)(e.X / 36); // сохраняем коодинаты первого клика
                    FposY = (int)(e.Y / 36); 
                    this.Score2.Text = "Ждем клик" + Convert.ToString(FposX) + Convert.ToString(FposY); // выводим в поле Score2 текст
                    this.Refresh(); // обновляем форму
                }
                else
                {
                    this.Score2.Text = "Активация!"; // выводим в поле Score2 текст
                    this.Refresh(); // обновляем форму
                    flag = false; // снимаем флаг (False - не было первого клика)
                }
            }
            else // если находимся в состоянии ожидании второго клика (flag = True)
            {
                this.Score2.Text = "Второй клик"; // выводим в поле Score2 текст
                if (!newGame.newBoard.SecondClick(FposX, FposY, e))
                {
                    this.Score2.Text = Convert.ToString(newGame.newBoard.Matrix[FposY, FposX].GetType()); // выводим в поле Score2 текст
                }// Проверяем возможные действия при втором клике
                
                this.Refresh(); // обновляем форму
                flag = false; // снимаем флаг (False - не было первого клика)
            }
        }

        /// <summary>
        ///  Метод при загрузке формы
        /// </summary>
        /// <param name="e">Аргументы события</param>
        /// <param name="sender">Объект обращения</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            newGame = new Game(); // создаем новую игру
            newGame.Initialization(); // запускаем игровой процесс

            do // доводим игровое поле до состояния готовности путем обнуления очков на игровом поле
            {
                newGame.newBoard.Score = 0;
                newGame.newBoard.Scoring();
            } while (newGame.newBoard.Score != 0);

            this.ClientSize = new System.Drawing.Size(37 * newGame.newBoard.SizeM, 37 * newGame.newBoard.SizeN + 21); // корректируем размеры формы
        }
    }
}
