using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace task2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(520, 520);
        }

        private void drawMap(DiamondSquare d, double[,] map)
        {
            double[] map_cpy = new double[513 * 513];
            for (int i = 0; i < 512; i++)
                for (int j = 0; j < 512; j++)
                {
                    map_cpy[i * 512 + j] = map[i, j];
                }

            Array.Sort(map_cpy);
            double minval = map_cpy[0];
            double maxval = map_cpy[512 * 512 - 1];
            double range = maxval - minval;
            double step = range / 12;
            //double step = range / 5;

            Graphics g = pictureBox1.CreateGraphics();

            for (int i = 0; i < 513; i++)
                for (int j = 0; j < 513; j++)
                {
                    double p = map[i, j];
                    Color clr = Color.White;

                   if (p < minval + step)
                        clr = Color.MidnightBlue;
                    else if (minval + step < p && p < minval + step * 2)
                        clr = Color.Navy;
                    else if (minval + step * 2 < p && p < minval + step * 3)
                        clr = Color.DarkBlue;
                    else if (minval + step * 3 < p && p < minval + step * 4)
                        clr = Color.MediumBlue;
                    else if (minval + step * 4 < p && p < minval + step * 5)
                        clr = Color.Blue;
                    else if (minval + step * 5 < p && p < minval + step * 6)
                        clr = Color.DarkGreen;
                    else if (minval + step * 6 < p && p < minval + step * 7)
                        clr = Color.Green;
                    else if (minval + step * 7 < p && p < minval + step * 8)
                        clr = Color.ForestGreen;
                    else if (minval + step * 8 < p && p < minval + step * 9)
                        clr = Color.OliveDrab;
                    else if (minval + step * 9 < p && p < minval + step * 10)
                        clr = Color.DarkKhaki;
                    else if (minval + step * 10 < p && p < minval + step * 11)
                        clr = Color.PaleGoldenrod;
                    else if (minval + step * 11 < p)
                        clr = Color.LightGoldenrodYellow;

                    /*if (p < minval + step)
                        clr = Color.Blue;
                    else if (minval + step < p && p < minval + step * 2)
                        clr = Color.DarkGreen;
                    else if (minval + step * 2 < p && p < minval + step * 3)
                        clr = Color.ForestGreen;
                    else if (minval + step * 3 < p && p < minval + step * 4)
                        clr = Color.OliveDrab;
                    else if (minval + step * 4 < p && p < minval + step * 5)
                        clr = Color.LightGoldenrodYellow;*/

                    ((Bitmap)pictureBox1.Image).SetPixel(i, j, clr);

                }
            pictureBox1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DiamondSquare d = new DiamondSquare(512, trackBar1.Value, 5);
            double[,] map = d.getData();
            drawMap(d, map);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DiamondSquare d = new DiamondSquare(512, trackBar1.Value, 5);

            double[][,] map = d.getDataStepByStep();
            for (int j = 0; j < d.countSteps(); j++)
            {
                drawMap(d, map[j]);
                pictureBox1.Refresh();
                System.Threading.Thread.Sleep(500);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = String.Format("roughness value: {0}", trackBar1.Value);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    public class DiamondSquare
    {

        private int _terrainPoints;
        private double _roughness;
        private double _seed; 


        public DiamondSquare(int terrainPoints, double roughness, double seed) 
        {
            this._terrainPoints = terrainPoints;
            this._roughness = roughness;
            this._seed = seed;
        }

        public double[,] getData()
        {
            return diamondSquareAlgorithm();
        }

        public double[][,] getDataStepByStep()
        {
            return algorithmStepByStep();
        }

        private void algorithmStep(double[,] data, int DATA_SIZE, int sideLength, int halfSide, double h, Random r) 
        {
            //square values
            for (int x = 0; x < DATA_SIZE - 1; x += sideLength)
            {
                for (int y = 0; y < DATA_SIZE - 1; y += sideLength)
                {
                    double avg = data[x, y] + data[x + sideLength, y] +
                        data[x, y + sideLength] + data[x + sideLength, y + sideLength];
                    avg /= 4.0;

                    data[x + halfSide, y + halfSide] = avg + (r.NextDouble() * 2 * h) - h;
                }
            }
            //diamond values

            for (int x = 0; x < DATA_SIZE - 1; x += halfSide)
            {
                for (int y = (x + halfSide) % sideLength; y < DATA_SIZE - 1; y += sideLength)
                {
                    double avg =
                      data[(x - halfSide + DATA_SIZE) % DATA_SIZE, y] + data[(x + halfSide) % DATA_SIZE, y] +
                        data[x, (y + halfSide) % DATA_SIZE] + data[x, (y - halfSide + DATA_SIZE) % DATA_SIZE];
                    avg /= 4.0;

                    avg = avg + (r.NextDouble() * 2 * h) - h;
                    
                    data[x, y] = avg;

                    if (x == 0) data[DATA_SIZE - 1, y] = avg;
                    if (y == 0) data[x, DATA_SIZE - 1] = avg;
                }
            }
        }

        private double[,] diamondSquareAlgorithm()
        {

            int DATA_SIZE = _terrainPoints + 1;  

            double[,] data = new double[DATA_SIZE, DATA_SIZE];
            data[0, 0] = data[0, DATA_SIZE - 1] = data[DATA_SIZE - 1, 0] =
            data[DATA_SIZE - 1, DATA_SIZE - 1] = _seed;

            double h = _roughness; 
            Random r = new Random();

            for (int sideLength = DATA_SIZE - 1; sideLength >= 2; sideLength /= 2, h /= 2.0)
            {
                int halfSide = sideLength / 2;

                algorithmStep(data, DATA_SIZE, sideLength, halfSide, h, r);

            }
            return data;
        }

        public int countSteps()
        {
            int cnt = 0;
            int value = _terrainPoints;
            while ((value % 2) == 0)
            {
                cnt++;
                value = value / 2;
            }
            return cnt;
        }

        private double[][,] algorithmStepByStep()
        {
            int DATA_SIZE = _terrainPoints + 1;  
            double[][,] steps = new double[this.countSteps() + 1][,];

            double[,] data = new double[DATA_SIZE, DATA_SIZE];
            data[0, 0] = data[0, DATA_SIZE - 1] = data[DATA_SIZE - 1, 0] =
            data[DATA_SIZE - 1, DATA_SIZE - 1] = _seed;

            double h = _roughness; 
            Random r = new Random();

            int i = 0;
            for (int sideLength = DATA_SIZE - 1; sideLength >= 2; sideLength /= 2, h /= 2.0)
            {
                int halfSide = sideLength / 2;
                algorithmStep(data, DATA_SIZE, sideLength, halfSide, h, r);

                steps[i] = new double[DATA_SIZE, DATA_SIZE];
                Array.Copy(data, steps[i], DATA_SIZE * DATA_SIZE);
                i++;
            }

            return steps;
        }
    }
}
