using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Игра
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public class Game
    {
        // Затраченное время
        public int Time;
        // Игровое поле
        public Board newBoard;       

        // Запускает игровой процесс
        public void Initialization() 
        {
            newBoard = new Board();
            // размеры игрового поля
            newBoard.SizeN = 10;
            newBoard.SizeM = 8;

            newBoard.Generate();
        }
    }

    public class Board
    {
        //Кол-во столбцов игрового поля
        public int SizeM;
        //Кол-во строк игрового поля
        public int SizeN;
        //Матрица объектов класса Cell
        public Cell[,] Matrix;
        // Количество набранных очков
        public int Score;

        // Генерирует элементы игрового поля
        public void Generate() 
        {
            Matrix = new Cell[SizeN, SizeM];
            Cell newCell = new Cell();
            for (int i=0; i < SizeN; i++)
                for (int j=0; j < SizeM; j++)
                {
                    Matrix[i, j] = newCell.RandElement(0);
                }
        }

        // Рисует игровое поле
        public void Draw(PaintEventArgs e) 
        {
            for (int i = 0; i < this.SizeN; i++)
                for (int j = 0; j < this.SizeM; j++)
                    e.Graphics.DrawImage(this.Matrix[i, j].ImgSource, j * 37, i * 37);
        }

        //Подсчитывает количество набранных очков
        public void Scoring()
        {
            // Инициализируем генератор случайных чисел
            Cell ScNewCell = new Cell();

            int k = 1;

            int StScore = Score;

            for (int i = 0; i < SizeN; i++)
            {
                for (int j = 1; j < SizeM - 1; j++)
                {
                    if (((Matrix[i, j - 1].ObjectType == Matrix[i, j].ObjectType) || (Matrix[i, j - 1].ObjectType == 5))
                        && ((Matrix[i, j + 1].ObjectType == Matrix[i, j].ObjectType) || (Matrix[i, j + 1].ObjectType == 5)))
                    {
                        k = 1;
                        while ((j + 1 + k < SizeM) && ((Matrix[i, j + 1 + k].ObjectType == Matrix[i, j].ObjectType) || (Matrix[i, j + 1 + k].ObjectType == 5)))
                            k++;

                        Matrix[i, j - 1] = ScNewCell.RandElement(5);
                        Matrix[i, j] = ScNewCell.RandElement(5);
                        Matrix[i, j + 1] = ScNewCell.RandElement(5);

                        if (k != 1)
                        {
                            for (int l = 1; l < k; l++)
                                Matrix[i, j + 1 + l] = ScNewCell.RandElement(5);
                            j += k;
                        }

                        Score++;
                    }
                }
            }
            if (Score == StScore)
            {
                for (int i = 1; i < SizeN - 1; i++)
                {
                    for (int j = 0; j < SizeM; j++)
                    {
                        if (((Matrix[i - 1, j].ObjectType == Matrix[i, j].ObjectType) || (Matrix[i - 1, j].ObjectType == 5))
                            && ((Matrix[i + 1, j].ObjectType == Matrix[i, j].ObjectType) || (Matrix[i + 1, j].ObjectType == 5)))
                        {
                            k = 1;
                            while ((i + 1 + k < SizeM) && ((Matrix[i + 1 + k, j].ObjectType == Matrix[i, j].ObjectType) || (Matrix[i + 1 + k, j].ObjectType == 5)))
                                k++;

                            Matrix[i - 1, j] = ScNewCell.RandElement(5);
                            Matrix[i, j] = ScNewCell.RandElement(5);
                            Matrix[i + 1, j] = ScNewCell.RandElement(5);

                            if (k != 1)
                            {
                                for (int l = 1; l < k; l++)
                                    Matrix[i + 1 + l, j] = ScNewCell.RandElement(5);
                                i += k;
                            }

                            Score++;
                        }
                    }
                }
            }
        }

        // Анализирует клик по первому элементу игрового поля (квадратику)
        public bool FirstClick(MouseEventArgs e) 
        {
            int posX, posY;
            if ((e.X < 37 * this.SizeM) && (e.Y < 37 * this.SizeN))
            {
                posX = (int)(e.X / 36);
                posY = (int)(e.Y / 36);

                if (!Activation(posX, posY))
                {
                    Matrix[posY, posX].SelectElement();
                    return true;
                }
            }
            return false;
        }

        // Анализирует возможность активации методов Squares, Intermix
        public bool Activation(int posX, int posY)
        {
            int StScore = Score;
            
            switch (Matrix[posY, posX].ObjectType)
            {
                case 4: 
                    Squares(posX, posY); 
                    break;
                case 6:
                    Intermix();
                    break;
                default: 
                    return false;
            }

            do
            {
                StScore = Score;
                Scoring();
            } while (StScore != Score);

            return true;
        }

        // Уничтожает квадраты, которые стоят вокруг элемента (т.е. поле размеров 3х3 клетки) и удаляется сам
        public void Squares(int posX, int posY)
        {
            // Инициализируем генератор случайных чисел
            Cell SqNewCell = new Cell();
            int lb = posX - 1, rb = posX + 1, tb = posY - 1, bb = posY + 1;

            if (posX == 0) lb = posX;
            if (posX == SizeM - 1) rb = posX;
            if (posY == 0) tb = posY;
            if (posY == SizeN - 1) bb = posY;
            for (int i = tb; i <= bb; i++)
                for (int j = lb; j <= rb; j++)
                {
                    Score++;
                    Matrix[i, j] = SqNewCell.RandElement(8);
                }
        }

        // Перемешивает элементы игрового поля
        public void Intermix() 
        {
            Generate(); 
        }

        // Анализирует клик по второму элементу игрового поля (квадратику)
        public bool SecondClick(int FposX, int FposY, MouseEventArgs e) 
        {
            int posX, posY;
            if ((e.X < 37 * this.SizeM) && (e.Y < 37 * this.SizeN))
            {
                posX = (int)(e.X / 36);
                posY = (int)(e.Y / 36);

                if ((this.Matrix[posY, posX].ObjectType < 4)&&((Math.Abs(posX-FposX)==1)&&(Math.Abs(posY-FposY)==0)) || ((Math.Abs(posY-FposY)==1)&&(Math.Abs(posX-FposX)==0)))
                {
                    Chain(FposX, FposY, posX, posY);
                }
                Matrix[FposY, FposX].CreateElement();
            }
            return true;
        }

        // Уничтожает цепочки из 3 и более элементов одинаковых по цвету (в том числе Радужного квадрата) по столбцам или строкам игрового поля
        public void Chain(int FposX, int FposY, int SposX, int SposY)
        {
            int StScore = Score;
            Swap(FposX, FposY, SposX, SposY);
            Scoring();
            if (Score == StScore)
            {
                Swap(SposX, SposY, FposX, FposY);
                Matrix[FposY, FposX].CreateElement();
            }
            else
            {
                Matrix[SposY, SposX].CreateElement();
                do
                {
                    StScore = Score;
                    Scoring();
                } while (StScore != Score);
            }
        }

        // Перестановка местами двух элементов игрового поля
        public void Swap(int FposX, int FposY, int posX, int posY) 
        {
            Cell SwCell = new Cell();
            SwCell = Matrix[FposY, FposX];
            Matrix[FposY, FposX] = Matrix[posY, posX];
            Matrix[posY, posX] = SwCell;
        }
    }

    public class Cell
    {
        // Тип элемента игрового поля
        // 0 - «Базовый желтый», 
        // 1 - «Базовый красный», 
        // 2 - «Базовый синий», 
        // 3 - «Базовый зеленый», 
        // 4 - «Бомба», 
        // 5 - «Радужный квадрат», 
        // 6 - «Молния»
        public int ObjectType;
        // Хранит изображение объекта
        public Image ImgSource;
        // Инициализируем генератор случайных чисел
        Random rnd = new Random();
        // Счетчик
        int rand = 0;

        // Генерирует случайный элемент игрового поля
        public Cell RandElement(int prand)
        {
            Cell mynewCell = new Cell();
            rand++;

            rand += prand;

            if (rand <= 15)
                ObjectType = rnd.Next(4);
            else
            {
                ObjectType = rnd.Next(7);
                rand = 0;
            }

            mynewCell.ObjectType = ObjectType;

            mynewCell.CreateElement();

            return mynewCell;
        }

        // Генерирует элемент игрового поля
        public void CreateElement() 
        {
            switch (this.ObjectType)
            {
                case 0: this.ImgSource = Properties.Resources.yellow; break;
                case 1: this.ImgSource = Properties.Resources.red; break;
                case 2: this.ImgSource = Properties.Resources.blue; break;
                case 3: this.ImgSource = Properties.Resources.green; break;
                case 4: this.ImgSource = Properties.Resources.bomb; break;
                case 5: this.ImgSource = Properties.Resources.rainbow; break;
                case 6: this.ImgSource = Properties.Resources.zip; break;
            }
        }

        //
        public void SelectElement()
        {
            switch (this.ObjectType)
            {
                case 0: this.ImgSource = Properties.Resources.yellow_1; break;
                case 1: this.ImgSource = Properties.Resources.red_1; break;
                case 2: this.ImgSource = Properties.Resources.blue_1; break;
                case 3: this.ImgSource = Properties.Resources.green_1; break;
            }
        }
    }
}
