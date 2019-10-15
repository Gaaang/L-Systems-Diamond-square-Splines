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
        private Pen penControl = new Pen(Color.Black, 1);
        private Pen penCurve = new Pen(Color.BlueViolet, 2);
        private List<PointF> controlPolygon = new List<PointF>();
        private PointF NotAPoint = new PointF(float.NaN, float.NaN);
        private PointF currentPoint = new PointF(float.NaN, float.NaN);

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.White);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (PointslistBox.SelectedIndex == -1)
            {
                controlPolygon.Add(e.Location);
                PointslistBox.Items.Add("Точка " + controlPolygon.Count.ToString());
            }
            else
            {
                controlPolygon[PointslistBox.SelectedIndex] = e.Location;
                currentPoint = e.Location;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (PointF p in controlPolygon)
            {
                e.Graphics.FillEllipse(brushBlack, p.X - 5, p.Y - 5, 10, 10);
            }
            if(controlPolygon.Count > 1)
                e.Graphics.DrawLines(penControl, controlPolygon.ToArray());
            if (controlPolygon.Count >= 4)
            {
                PointF p0, p1, p2, p3;
                p0 = controlPolygon[0];
                p1 = controlPolygon[1];
                p2 = controlPolygon[2];
                p3 = controlPolygon[3];
                e.Graphics.DrawLines(penCurve, calcaluteСurve(p0,p1,p2,p3));
            }
            if(currentPoint.X != float.NaN)
                e.Graphics.FillEllipse(brushRed, currentPoint.X - 5, currentPoint.Y - 5, 10, 10);
            pictureBox1.Invalidate();
        }

        private PointF[] calcaluteСurve(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            float disc = (float)0.001;
            int cnt = (int)(1 / disc) + 1;
            PointF[] res = new PointF[cnt];

            float t = 0;
            for (int i = 0; i < cnt; i++)
            {
                float mt = 1 - t;
                float mt2 = mt * mt;
                float x;
                float y;
                float t2 = t * t;
                x = p3.X * t2 * t;
                y = p3.Y * t2 * t;
                x += 3 * t2 * p2.X * mt;
                y += 3 * t2 * p2.Y * mt;
                x += 3 * t * p1.X * mt2;
                y += 3 * t * p1.Y * mt2;
                x += p0.X * mt2 * mt;
                y += p0.Y * mt2 * mt;

                t += disc;
                res[i] = new PointF(x, y);
            }
            return res;
        }

        private void PointslistBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PointslistBox.SelectedIndex != -1)
                currentPoint = controlPolygon[PointslistBox.SelectedIndex];
            else
                currentPoint = NotAPoint;
        }

        private void AddPointBtn_Click(object sender, EventArgs e)
        {
            PointslistBox.SelectedIndex = -1;
        }

        private void DeletePntBtn_Click(object sender, EventArgs e)
        {
            if (PointslistBox.SelectedIndex != -1)
            {
                controlPolygon.RemoveAt(PointslistBox.SelectedIndex);
                for (int i = PointslistBox.SelectedIndex; i < PointslistBox.Items.Count; i++)
                    PointslistBox.Items.Clear();
                for (int i = 1; i <= controlPolygon.Count; i++)
                    PointslistBox.Items.Add("Точка " + Convert.ToString(i));
                currentPoint = NotAPoint;
            }
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            controlPolygon.Clear();
            PointslistBox.Items.Clear();
            currentPoint = NotAPoint;
            pictureBox1.Invalidate();
        }
    }
}
