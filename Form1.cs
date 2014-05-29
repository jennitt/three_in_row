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
        // Игра
        public Game newGame;
        // Флаг состояния
        public bool flag = false;
        // 
        int FposX = 0, FposY = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.StripScore.Text = "Score: _" + Convert.ToString(newGame.newBoard.Score);
            newGame.newBoard.Draw(e);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if(!flag)
            {
                if(flag = newGame.newBoard.FirstClick(e))
                {
                    FposX = (int)(e.X / 36);
                    FposY = (int)(e.Y / 36);
                    this.Score2.Text = "Ждем клик" + Convert.ToString(FposX) + Convert.ToString(FposY);
                    this.Refresh();
                }
                else
                {
                    this.Score2.Text = "Активация!";
                    this.Refresh();
                    flag = false;
                }
            }
            else 
            {
                if (newGame.newBoard.SecondClick(FposX, FposY, e))
                {
                    this.Score2.Text = "Второй клик";
                    this.Refresh();
                    flag = false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            newGame = new Game();
            newGame.Initialization();

            do
            {
                newGame.newBoard.Score = 0;
                newGame.newBoard.Scoring();
            } while (newGame.newBoard.Score != 0);

            this.ClientSize = new System.Drawing.Size(37 * newGame.newBoard.SizeM, 37 * newGame.newBoard.SizeN + 22);
        }
    }
}
