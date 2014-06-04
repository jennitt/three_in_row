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

    /// <summary>
    ///  Класс Game - запускает игру
    /// </summary>
    public class Game
    {    
        // параметр - Игровое поле
        public Board newBoard;

        /// <summary>
        ///  Запускает игровой процесс
        /// </summary>
        public void Initialization() 
        {
            newBoard = new Board(); // создаем новое игровое поле (объект класса Board)

            newBoard.SizeN = 10; // размеры игрового поля кол-во строк
            newBoard.SizeM = 8; // кол-во столбцов

            newBoard.Generate(); // генерируем случайную матрицу элементов игрового поля
        }
    }

    /// <summary>
    ///  Класс Board - создает игровое поле
    /// </summary>
    public class Board
    {
        public int SizeM; //Кол-во столбцов игрового поля

        public int SizeN; //Кол-во строк игрового поля

        public Cell[,] Matrix; // Матрица объектов класса Cell

        public int Score; // Количество набранных очков

        /// <summary>
        ///  Генерирует элементы игрового поля
        /// </summary>
        public void Generate() 
        {
            Matrix = new Cell[SizeN, SizeM]; // создает матрицу размером SizeN x SizeM из объектов класса Cell
            Cell newCell = new Cell(); // создает объект класса Cell
            for (int i = 0; i < SizeN; i++) // заполняем матрицу объектами класса Cell
                for (int j = 0; j < SizeM; j++)
                {
                    Matrix[i, j] = newCell.RandElement(0); // ячейке матрицы присваиваем случайный объект класса Cell
                }
        }    
        
        /// <summary>
        ///  Рисует игровое поле
        /// </summary>
        /// <param name="e">Событие - рисование</param>
        public void Draw(PaintEventArgs e) 
        {
            for (int i = 0; i < this.SizeN; i++) // рисуем на форме объекты матрицы 
                for (int j = 0; j < this.SizeM; j++)
                    e.Graphics.DrawImage(this.Matrix[i, j].ImgSource, j * 37, i * 37); // рисуем на форме рисунок элемента
        }

        /// <summary>
        /// Подсчитывает количество набранных очков
        /// </summary>
        public void Scoring()
        {
            Cell ScNewCell = new Cell(); // Создаем объект класса Cell

            int k = 1; // счетчик доп. ячеек

            int StScore = Score; // запоминает текущее кол-во очков

            for (int i = 0; i < SizeN; i++) // подсчет очков по горизонтали
            {
                for (int j = 1; j < SizeM - 1; j++) // от первого до предпоследнего, т.к. у них нет соседа слева (справа)
                {
                    // проверяет соседей справа и слева от текущ. ячейки на одинаковость (или на радужный квадрат)
                    if (((Matrix[i, j - 1].ObjectType == Matrix[i, j].ObjectType) || (Matrix[i, j - 1].ObjectType == 5))
                        && ((Matrix[i, j + 1].ObjectType == Matrix[i, j].ObjectType) || (Matrix[i, j + 1].ObjectType == 5)))
                    {
                        k = 1;
                        // проверяем ещё соседей справа
                        while ((j + 1 + k < SizeM) && ((Matrix[i, j + 1 + k].ObjectType == Matrix[i, j].ObjectType) || (Matrix[i, j + 1 + k].ObjectType == 5)))
                            k++;
                        
                        Matrix[i, j - 1] = ScNewCell.RandElement(5); // заменяем найденные одинаковые ячейки (соседей)
                        Matrix[i, j] = ScNewCell.RandElement(5);
                        Matrix[i, j + 1] = ScNewCell.RandElement(5);

                        if (k != 1) // если были найдены доп. соседи справа
                        {
                            for (int l = 1; l < k; l++)
                                Matrix[i, j + 1 + l] = ScNewCell.RandElement(5); // меняем доп. соседи на новые
                        }

                        Score++; // увеличиваем счетчик очков на 1
                    }
                }
            }
            
            if (Score == StScore)
            {
                for (int i = 1; i < SizeN - 1; i++) // подсчет очков по вертикали
                {
                    for (int j = 0; j < SizeM; j++)
                    {
                        // проверяет соседей сверху и снизу от текущ. ячейки на одинаковость (или на радужный квадрат)
                        if (((Matrix[i - 1, j].ObjectType == Matrix[i, j].ObjectType) || (Matrix[i - 1, j].ObjectType == 5))
                            && ((Matrix[i + 1, j].ObjectType == Matrix[i, j].ObjectType) || (Matrix[i + 1, j].ObjectType == 5)))
                        {
                            // проверяем ещё соседей снизу
                            k = 1;
                            while ((i + 1 + k < SizeM) && ((Matrix[i + 1 + k, j].ObjectType == Matrix[i, j].ObjectType) || (Matrix[i + 1 + k, j].ObjectType == 5)))
                                k++;

                            Matrix[i - 1, j] = ScNewCell.RandElement(5);  // заменяем найденные одинаковые ячейки (соседей)
                            Matrix[i, j] = ScNewCell.RandElement(5);
                            Matrix[i + 1, j] = ScNewCell.RandElement(5);

                            if (k != 1) // если были найдены доп. соседи снизу
                            {
                                for (int l = 1; l < k; l++)
                                    Matrix[i + 1 + l, j] = ScNewCell.RandElement(5); // меняем доп. соседи на новые
                            }

                            Score++; // увеличиваем счетчик очков на 1
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Анализирует клик по первому элементу игрового поля (квадратику)
        /// </summary>
        /// <param name="e">Событие - клик мыши</param>
        /// <returns>True - не было активации, False - была активация</returns> 
        public bool FirstClick(MouseEventArgs e) 
        {
            int posX, posY;
            int StScore; // запоминает текущее кол-во очков

            if ((e.X < 37 * this.SizeM) && (e.Y < 37 * this.SizeN)) // защита от кликов вне игрового поля
            {
                posX = (int)(e.X / 36); // находит номер ячейки матрицы
                posY = (int)(e.Y / 36);

                if (!Matrix[posY, posX].Activation(posX, posY, this)) // проверка на возможность активации при однократном клике
                {
                    Matrix[posY, posX].SelectElement(); // выделяем данную ячейку (затемняем)
                    return true;
                }
                else
                {
                    do // подсчитываем очки после активации 
                    {
                        StScore = Score;
                        Scoring();
                    } while (StScore != Score); // до тех пор, пока не останется неподсчитанных очков
                }
            }
            return false;
        }
       
        /// <summary>
        ///   Анализирует клик по второму элементу игрового поля (квадратику)
        /// </summary>
        /// <param name="FposX">Номер столбца ячейки из первого клика</param>
        /// <param name="FposY">Номер строки ячейки из первого клика</param>
        /// <param name="e">Событие - клик мыши</param>
        public void SecondClick(int FposX, int FposY, MouseEventArgs e) 
        {
            int posX, posY;
            if ((e.X < 37 * this.SizeM) && (e.Y < 37 * this.SizeN)) // защита от клика вне границ игрового поля
            {
                posX = (int)(e.X / 36); // получает номер столбца и строки ячейки, по которой кликнули
                posY = (int)(e.Y / 36);

                // проверяет корректность второго клика
                if ((this.Matrix[posY, posX].ObjectType < 4)
                    &&((Math.Abs(posX-FposX)==1)&&(Math.Abs(posY-FposY)==0)) || ((Math.Abs(posY-FposY)==1)&&(Math.Abs(posX-FposX)==0)))
                {
                    Chain(FposX, FposY, posX, posY); // проверяем возможность образования цепочки в реультате перемены мест
                }
                Matrix[FposY, FposX].CreateElement(); // снимаем выделение с ячейки из первого клика
            }
        }
        
       /// <summary>
       ///Уничтожает цепочки из 3 и более элементов одинаковых по цвету (в том числе Радужного квадрата) по столбцам или строкам игрового поля
       /// </summary>
       /// <param name="FposX">Номер столбца ячейки из первого клика</param>
       /// <param name="FposY">Номер строки ячейки из первого клика</param>
       /// <param name="SposX">Номер столбца ячейки из второго клика</param>
       /// <param name="SposY">Номер строки ячейки из второго клика</param>
        public void Chain(int FposX, int FposY, int SposX, int SposY)
        {
            int StScore = Score; // запоминаем кол-во очков на текущий момент
            Swap(FposX, FposY, SposX, SposY); // меняем местами 2 ячейки с координатами из вх. аргументов
            Scoring(); // считаем кол-во очков на игровом поле
            if (Score == StScore) // если кол-во очков не изменилось
            {
                Swap(SposX, SposY, FposX, FposY); // меняем 2 ячейки местами обратно
                Matrix[FposY, FposX].CreateElement(); // снимаем выделение с первой ячейки
            }
            else // если кол-во очков изменилось, сохраняем перемену мест
            {
                Matrix[SposY, SposX].CreateElement(); // снимаем выделение со второй ячейки
                do // производим подсчет очков до тех пор, пока не подсчитаем все
                {
                    StScore = Score;
                    Scoring();
                } while (StScore != Score);
            }
        }
        
        /// <summary>
        /// Перестановка местами двух элементов игрового поля
        /// </summary>
        /// <param name="FposX">Номер столбца ячейки из первого клика</param>
        /// <param name="FposY">Номер строки ячейки из первого клика</param>
        /// <param name="posX">Номер столбца ячейки из второго клика</param>
        /// <param name="posY">Номер строки ячейки из второго клика</param>
        public void Swap(int FposX, int FposY, int posX, int posY) 
        {
            Cell SwCell = new Cell(); // создаем ячейку-буфер
            SwCell = Matrix[FposY, FposX]; // сохраняем в буфер первую ячейку
            Matrix[FposY, FposX] = Matrix[posY, posX]; // меняем первую ячейку на вторую
            Matrix[posY, posX] = SwCell; // меняем вторую ячейку на первую (из буфера)
        }
    }

    /// <summary>
    /// Класс Cell - ячейка
    /// </summary>
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

        public Image ImgSource; // Хранит изображение объекта

        Random rnd = new Random(); // Инициализируем генератор случайных чисел

        int rand = 0; // Счетчик итераций генератора случайных чисел
        
        /// <summary>
        /// Генерирует случайный элемент игрового поля
        /// </summary>
        /// <param name="prand">Увеличивает счетчик итераций генератора случайных чисел на значение prand</param>
        /// <returns>Сгенерированный объект класса Cell (ячейку)</returns>
        public Cell RandElement(int prand)
        {
            Cell mynewCell = new Cell(); // создаем новый объект класса Cell
            rand++; // увеличиваем счетчик итераций на 1

            rand += prand; // увеличиваем счетчик итераций на значение prand

            if (rand <= 15) // пока счетчик итераций < 15
                ObjectType = rnd.Next(4); // генерируем только базовые элементы (0, 1, 2, 3)
            else // счетчик достигает 15
            {
                ObjectType = rnd.Next(7); // генерируем любой случайный элемент
                rand = 0; // обнуляем счетчик итераций
            }

            switch (ObjectType)
            {
                case 0: 
                case 1:
                case 2:
                case 3: mynewCell = new Basic(); break;
                case 4: mynewCell = new Bomb(); break;
                case 5: mynewCell = new Rainbow(); break;
                case 6: mynewCell = new Zip(); break;
            }

            mynewCell.ObjectType = ObjectType;
            mynewCell.CreateElement();

            return mynewCell; // возвращаем сформированный новый объект
        }

        /// <summary>
        /// Создает изображение элемента игрового поля
        /// </summary>
        public virtual void CreateElement() { }

        /// <summary>
        /// Выделяет изображение элемента игрового поля
        /// </summary>
        public virtual void SelectElement() { }

        /// <summary>
        /// Активация
        /// </summary>
        public virtual bool Activation(int posX, int posY, Board myBoard) { return false; }
    }

    public class Basic : Cell
    {
        public override void CreateElement()
        {
            switch (this.ObjectType) // в зависимости от типа (его номера) ячейки выделяется своё изображение
            {
                case 0: this.ImgSource = Properties.Resources.yellow; break;
                case 1: this.ImgSource = Properties.Resources.red; break;
                case 2: this.ImgSource = Properties.Resources.blue; break;
                case 3: this.ImgSource = Properties.Resources.green; break;
            }
        }

        public override void SelectElement()
        {
            switch (this.ObjectType) // в зависимости от типа (его номера) ячейки выделяется своё изображение
            {
                case 0: this.ImgSource = Properties.Resources.yellow_1; break;
                case 1: this.ImgSource = Properties.Resources.red_1; break;
                case 2: this.ImgSource = Properties.Resources.blue_1; break;
                case 3: this.ImgSource = Properties.Resources.green_1; break;
            }
        }
    }

    public class Rainbow : Cell
    {
        public override void CreateElement()
        {
            ImgSource = Properties.Resources.rainbow;
        }
    }

    public class Bomb : Cell
    {
        public override void CreateElement()
        {
            ImgSource = Properties.Resources.bomb;
        }

        public override bool Activation(int posX, int posY, Board myBoard)
        {
            Cell SqNewCell = new Cell(); // создает новый объект класса Cell
            int lb = posX - 1, rb = posX + 1, tb = posY - 1, bb = posY + 1; // запоминаем координаты границ вокруг ячейки

            if (posX == 0) lb = posX; // корректируем границ, чтобы избежать выхода за границы поля
            if (posX == myBoard.SizeM - 1) rb = posX;
            if (posY == 0) tb = posY;
            if (posY == myBoard.SizeN - 1) bb = posY;

            for (int i = tb; i <= bb; i++) // проходим по всем ячейкам вокруг ячейки
                for (int j = lb; j <= rb; j++)
                {
                    myBoard.Score++; // увелииваем счетчик очков на 1
                    myBoard.Matrix[i, j] = SqNewCell.RandElement(8); // заменяем на новый
                }
            return true;
        }
    }

    public class Zip : Cell
    {
        public override void CreateElement()
        {
            ImgSource = Properties.Resources.zip;
        }

        public override bool Activation(int posX, int posY, Board myBoard)
        {
            myBoard.Generate(); // генерируем новое игровое поле
            return true;
        }
    }
}
