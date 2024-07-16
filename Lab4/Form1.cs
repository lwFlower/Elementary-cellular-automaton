namespace Lab4
{
    public partial class Form1 : Form
    {
        int resolution;
        private Graphics graphics;
        private bool[,] field;
        private int rows;
        private int cols;
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void NextGeneration()
        {
            graphics.Clear(Color.Black);

            var newfield = new bool[cols, rows];

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    int Neighbours = CountNeighbours(i, j);

                    if (!field[i, j] && Neighbours == 3) newfield[i, j] = true;
                    else if (field[i, j] && (Neighbours < 2 || Neighbours > 3)) newfield[i, j] = false;
                    else newfield[i, j] = field[i, j];

                    if (field[i, j])
                    {
                        graphics.FillRectangle(Brushes.Purple, i * resolution, j * resolution, resolution, resolution);
                    }
                }
            }
            field = newfield;
            pictureBox1.Refresh();
        }

        private void StartGame()
        {
            if (timer1.Enabled) return;



            resolution = (int)edResolution.Value;
            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;
            field = new bool[cols, rows];

            Random random = new Random();
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    field[i, j] = random.Next((int)edDensity.Value) == 0;
                }
            }
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.FillRectangle(Brushes.Purple, 0, 0, resolution, resolution);

            timer1.Start();
        }

        private int CountNeighbours(int i, int j)
        {
            int count = 0;
            for (int i2 = -1; i2 < 2; i2++)
            {
                for (int j2 = -1; j2 < 2; j2++)
                {
                    int x = (i + i2 + cols) % cols;
                    int y = (j + j2 + rows) % rows;
                    if ((field[x, y] == true) && x != i2 && y != j2) count += 1;


                }

            }
            return count;
        }
        private void button_Start_Click(object sender, EventArgs e)
        {
            StartGame();
        }



        private void button_stop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (timer1.Enabled) return;
            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                bool valid = ValidateMousePosition(x, y);
                if (valid) field[x, y] = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                bool valid = ValidateMousePosition(x, y);
                if (valid) field[x, y] = false;
            }
        }

        private bool ValidateMousePosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }
    }
}