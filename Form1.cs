using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Life
{
    public partial class Form1 : Form
    {
        private int currenGeneration = 0;
        private Graphics graphics;
        private int resolution;
        private bool [,] field;
        private int rows;
        private int cols;

        public Form1()
        {
            InitializeComponent();
        }
        private void StartGame()
        {
            if (timer.Enabled) 
                return;

            currenGeneration = 0;
            Text = $"Generation {currenGeneration}";

            nudDensity.Enabled = false;
            nudResolution.Enabled = false;
            resolution = (int) nudResolution.Value;
            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;
            field = new bool[cols, rows];

            Random random = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)nudDensity.Value) == 0;
                }
            }


            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer.Start();
        }
       private void NextGeneration()
        {
            graphics.Clear(Color.Black);

            var newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y);
                    var HasLife = field[x, y];

                    if (HasLife&& neighboursCount ==3)
                        newField[x, y] = true;
                    else if (HasLife && (neighboursCount < 2 || neighboursCount > 3))
                        newField[x, y] = false;
                    else
                        newField[x, y] = field[x, y];

                    if (HasLife)
                    {
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution);
                    }
                }
            }
            field = newField;
            pictureBox1.Refresh();
            Text = $"Generation {++currenGeneration}";


        }
        private int CountNeighbours(int x, int y)
        {
            int count = 0;

            for (int i = -1; i<2; i++)
            {
                for (int j=-1; j<2; j++)
                {
                    var col = (x + i + cols)% cols;
                    var row = (y + j+rows)%rows;


                    var isSelfChecking = col == x && row == y;
                    var HasLife = field[col, row];

                    if (HasLife && !isSelfChecking)
                        count++;
                }
            }

            return count;
        }

        private void StopGame ()
        {
            if (!timer.Enabled) return;
            timer.Stop();
            nudDensity.Enabled = true;
            nudResolution.Enabled = true;

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartGame();
            graphics.FillRectangle(Brushes.Crimson, 0, 0, resolution, resolution);
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer.Enabled)
                return;

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                var validatorpassed = ValidateMousePosition(x, y);
                if (validatorpassed)
                   field[x, y] = true;
                                
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                var validatorpassed = ValidateMousePosition(x, y);
                if (validatorpassed)
                    field[x, y] = true;
            }
        }
    private bool ValidateMousePosition (int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"Generation {currenGeneration}";
        }
    }

}
