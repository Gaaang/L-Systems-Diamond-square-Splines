using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BezierCubicSplines
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private SolidBrush brushRed = new SolidBrush(Color.Red);
        private SolidBrush brushBlack = new SolidBrush(Color.Black);
        private Pen penHull = new Pen(Color.LightGoldenrodYellow, 1);
        private Pen penCurve = new Pen(Color.Magenta, 1);
        private List<PointF> points = new List<PointF>();
        PointF currentPoint = new PointF(float.NaN, float.NaN);
        PointF nap = new PointF(float.NaN, float.NaN);
        int ind = -1;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.White);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PointF h = e.Location;
                if (IsInRadius(ref h) && ind != (-1))
                    points.Remove(points[ind]);
                currentPoint = e.Location;
                g.Clear(Color.White);
            }
            pictureBox1.Invalidate();
        }

        //попадает ли выбранная точка в радиус одной из уже добавленных
        bool IsInRadius(ref PointF e)
        {
            PointF p = new PointF(0, 0);
            for(int i = 0;i < points.Count;i++)
            {
                p = points[i];
                if (p.X - 15 <= e.X && e.X <= p.X + 15 && p.Y - 15 <= e.Y && e.Y <= p.X + 15)
                {
                    ind = i;
                    return true;
                }
            }
            return false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                currentPoint = e.Location;
            pictureBox1.Invalidate();
        }


        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentPoint = e.Location;
                points.Add(currentPoint);
                currentPoint = nap;
            }
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (PointF p in points)
            {
                e.Graphics.FillEllipse(brushRed, p.X - 3, p.Y - 3, 7, 7);
            }
            e.Graphics.FillEllipse(brushBlack, currentPoint.X - 3, currentPoint.Y - 3, 7, 7);
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            points.Clear();
            pictureBox1.Invalidate();
        }
    }
}
