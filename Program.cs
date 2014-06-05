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
        Random rnd = new Random(); // Инициализируем генератор случайных чисел
        int rand = 0; // Счетчик итераций генератора случайных чисел
        Type YellowType = Type.GetType("Игра.Yellow");
        Type RedType = Type.GetType("Игра.Red");
        Type BlueType = Type.GetType("Игра.Blue");
        Type GreenType = Type.GetType("Игра.Green");
        Type RainType = Type.GetType("Игра.Rainbow");

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
                    Matrix[i, j] = RandElement(); // ячейке матрицы присваиваем случайный объект класса Cell
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
            int k = 1; // счетчик доп. ячеек

            int StScore = Score; // запоминает текущее кол-во очков

            for (int i = 0; i < SizeN; i++) // подсчет очков по горизонтали
            {
                for (int j = 1; j < SizeM - 1; j++) // от первого до предпоследнего, т.к. у них нет соседа слева (справа)
                {
                    // проверяет соседей справа и слева от текущ. ячейки на одинаковость (или на радужный квадрат)
                    if (((Matrix[i, j - 1].GetType() == Matrix[i, j].GetType()) || (Matrix[i, j - 1].GetType() == RainType))
                        && ((Matrix[i, j + 1].GetType() == Matrix[i, j].GetType()) || (Matrix[i, j + 1].GetType() == RainType)))
                    {
                        k = 1;
                        // проверяем ещё соседей справа
                        while ((j + 1 + k < SizeM) && ((Matrix[i, j + 1 + k].GetType() == Matrix[i, j].GetType()) || (Matrix[i, j + 1 + k].GetType() == RainType)))
                            k++;
                        
                        Matrix[i, j - 1] = RandElement(); // заменяем найденные одинаковые ячейки (соседей)
                        Matrix[i, j] = RandElement();
                        Matrix[i, j + 1] = RandElement();

                        if (k != 1) // если были найдены доп. соседи справа
                        {
                            for (int l = 1; l < k; l++)
                                Matrix[i, j + 1 + l] = RandElement(); // меняем доп. соседи на новые
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
                        if (((Matrix[i - 1, j].GetType() == Matrix[i, j].GetType()) || (Matrix[i - 1, j].GetType() == RainType))
                            && ((Matrix[i + 1, j].GetType() == Matrix[i, j].GetType()) || (Matrix[i + 1, j].GetType() == RainType)))
                        {
                            // проверяем ещё соседей снизу
                            k = 1;
                            while ((i + 1 + k < SizeM) && ((Matrix[i + 1 + k, j].GetType() == Matrix[i, j].GetType()) || (Matrix[i + 1 + k, j].GetType() == RainType)))
                                k++;

                            Matrix[i - 1, j] = RandElement();  // заменяем найденные одинаковые ячейки (соседей)
                            Matrix[i, j] = RandElement();
                            Matrix[i + 1, j] = RandElement();

                            if (k != 1) // если были найдены доп. соседи снизу
                            {
                                for (int l = 1; l < k; l++)
                                    Matrix[i + 1 + l, j] = RandElement(); // меняем доп. соседи на новые
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
        public bool SecondClick(int FposX, int FposY, MouseEventArgs e) 
        {
            int posX, posY;
            if ((e.X < 37 * this.SizeM) && (e.Y < 37 * this.SizeN)) // защита от клика вне границ игрового поля
            {
                posX = (int)(e.X / 36); // получает номер столбца и строки ячейки, по которой кликнули
                posY = (int)(e.Y / 36);

                // проверяет корректность второго клика
                if (((Matrix[posY, posX].GetType() == YellowType) || (Matrix[posY, posX].GetType() == RedType) || (Matrix[posY, posX].GetType() == BlueType) || (Matrix[posY, posX].GetType() == GreenType))
                    &&
                    (((Math.Abs(posX-FposX)==1)&&(Math.Abs(posY-FposY)==0)) || ((Math.Abs(posY-FposY)==1)&&(Math.Abs(posX-FposX)==0))))
                {
                    Chain(FposX, FposY, posX, posY); // проверяем возможность образования цепочки в реультате перемены мест
                    return true;
                }
                Matrix[FposY, FposX].CreateElement(); // снимаем выделение с ячейки из первого клика
            }
            return false;
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
            int ChScore = Score; // запоминаем кол-во очков на текущий момент
            Swap(FposX, FposY, SposX, SposY); // меняем местами 2 ячейки с координатами из вх. аргументов
            Scoring(); // считаем кол-во очков на игровом поле
            if (Score == ChScore) // если кол-во очков не изменилось
            {
                Swap(SposX, SposY, FposX, FposY); // меняем 2 ячейки местами обратно
                Matrix[FposY, FposX].CreateElement(); // снимаем выделение с первой ячейки
            }
            else // если кол-во очков изменилось, сохраняем перемену мест
            {
                Matrix[SposY, SposX].CreateElement(); // снимаем выделение со второй ячейки
                do // производим подсчет очков до тех пор, пока не подсчитаем все
                {
                    ChScore = Score;
                    Scoring();
                } while (ChScore != Score);
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
            Cell SwCell = Matrix[FposY, FposX]; // сохраняем в буфер первую ячейку
            Matrix[FposY, FposX] = Matrix[posY, posX]; // меняем первую ячейку на вторую
            Matrix[posY, posX] = SwCell; // меняем вторую ячейку на первую (из буфера)
        }

        /// <summary>
        /// Генерирует случайный элемент игрового поля
        /// </summary>
        /// <param name="prand">Увеличивает счетчик итераций генератора случайных чисел на значение prand</param>
        /// <returns>Сгенерированный объект класса Cell (ячейку)</returns>
        public Cell RandElement()
        {
            int op;
            rand++; // увеличиваем счетчик итераций на 1

            if (rand <= 10) // пока счетчик итераций < 10
                op = rnd.Next(4); // генерируем только базовые элементы (0, 1, 2, 3)
            else // счетчик достигает 10
            {
                op = rnd.Next(7); // генерируем любой случайный элемент
                rand = 0; // обнуляем счетчик итераций
            }

            switch (op)
            {
                case 0: return new Yellow();
                case 1: return new Red();
                case 2: return new Blue();
                case 3: return new Green();
                case 4: return new Bomb();
                case 5: return new Rainbow();
                case 6: return new Zip();
            }

            return new Cell(); // возвращаем сформированный новый объект
        }
    }

    /// <summary>
    /// Класс Cell - ячейка
    /// </summary>
    public class Cell
    {
        public Image ImgSource; // Хранит изображение объекта

        public Cell() { CreateElement(); }

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

    // 0 - «Базовый желтый», 
    public class Yellow : Cell
    {
        public override void CreateElement()
        {
            ImgSource = Properties.Resources.yellow;
        }

        public override void SelectElement()
        {
            ImgSource = Properties.Resources.yellow_1;
        }
    }

    // 1 - «Базовый красный»,
    public class Red : Cell
    {
        public override void CreateElement()
        {
            ImgSource = Properties.Resources.red;
        }

        public override void SelectElement()
        {
            ImgSource = Properties.Resources.red_1;
        }
    }

    // 2 - «Базовый синий»,
    public class Blue : Cell
    {
        public override void CreateElement()
        {
            ImgSource = Properties.Resources.blue;
        }

        public override void SelectElement()
        {
            ImgSource = Properties.Resources.blue_1;
        }
    }

    // 3 - «Базовый зеленый»,
    public class Green : Cell
    {
        public override void CreateElement()
        {
            ImgSource = Properties.Resources.green;
        }

        public override void SelectElement()
        {
            ImgSource = Properties.Resources.green_1;
        }
    }

    // 5 - «Радужный квадрат», 
    public class Rainbow : Cell
    {
        public override void CreateElement()
        {
            ImgSource = Properties.Resources.rainbow;
        }
    }

    // 4 - «Бомба»,
    public class Bomb : Cell
    {
        public override void CreateElement()
        {
            ImgSource = Properties.Resources.bomb;
        }

        public override bool Activation(int posX, int posY, Board myBoard)
        {
            int lb = posX - 1, rb = posX + 1, tb = posY - 1, bb = posY + 1; // запоминаем координаты границ вокруг ячейки

            if (posX == 0) lb = posX; // корректируем границ, чтобы избежать выхода за границы поля
            if (posX == myBoard.SizeM - 1) rb = posX;
            if (posY == 0) tb = posY;
            if (posY == myBoard.SizeN - 1) bb = posY;

            for (int i = tb; i <= bb; i++) // проходим по всем ячейкам вокруг ячейки
                for (int j = lb; j <= rb; j++)
                {
                    myBoard.Score++; // увелииваем счетчик очков на 1
                    myBoard.Matrix[i, j] = myBoard.RandElement(); // заменяем на новый
                }
            return true;
        }
    }

    // 6 - «Молния»
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
