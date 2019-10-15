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
        private Pen penCubeCurve = new Pen(Color.BlueViolet, 2);
        private Pen penCurve = new Pen(Color.Green, 2);
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
            foreach (PointF p in controlPolygon)//контрольные точки
            {
                e.Graphics.FillEllipse(brushBlack, p.X - 5, p.Y - 5, 10, 10);
            }
            if(controlPolygon.Count > 1)
                e.Graphics.DrawLines(penControl, controlPolygon.ToArray());//соединяем контрольные точки в порядке следования

            //
            for (int i = 1; i < controlPolygon.Count - 1; i += 2)
            {
                PointF p0, p1, p2, p3;

                int prev = i - 1; // p0
                int next2 = i + 2; // p3 

                if (prev == 0) 
                    p0 = controlPolygon[prev];
                else
                    p0 = new PointF((controlPolygon[i].X + controlPolygon[prev].X) / 2, (controlPolygon[i].Y + controlPolygon[prev].Y) / 2);

                if (i == controlPolygon.Count - 2)
                {
                    p1 = controlPolygon[i];
                    p3 = controlPolygon[i + 1];
                    p2 = new PointF((p3.X + p1.X) / 2, (p3.Y + p1.Y) / 2);
                }
                else
                {
                    p1 = controlPolygon[i];
                    p2 = controlPolygon[i + 1];
                    if (next2 == controlPolygon.Count - 1)
                        p3 = controlPolygon[next2];
                    else
                        p3 = new PointF((controlPolygon[next2].X + p2.X) / 2, (controlPolygon[next2].Y + p2.Y) / 2);
                }

                e.Graphics.DrawLines(penCubeCurve, calcaluteCubeСurve(p0, p1, p2, p3));
            }
            
            //рисует Кривую Безье по всем контрольным точкам
            e.Graphics.DrawLines(penCurve, CalcCurve(controlPolygon.ToArray()));

            if (currentPoint.X != float.NaN)
                e.Graphics.FillEllipse(brushRed, currentPoint.X - 5, currentPoint.Y - 5, 10, 10);
            pictureBox1.Invalidate();
        }

        //вычисляем кубическую кривую безье,по 4 точкам
        private PointF[] calcaluteCubeСurve(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            float step = 0.001f;
            int curvePointsNumber = (int)(1 / step) + 1;
            PointF[] res = new PointF[curvePointsNumber];

            //промежуточные точки Q0, Q1 и Q2 описывают линейные кривые
            //R0 и R1 описывают квадратичные кривые
            PointF q0 =  new PointF(0, 0);
            PointF q1 = q0, q2 = q0, r0 = q0, r1 = q0;
            float t = 0;
            for (int i = 0; i < curvePointsNumber; i++)
            {
                float param = 1 - t;
                float paramSq = param * param;
                q0.X = p0.X * param + p1.X * t; q0.Y = p0.Y * param + p1.Y * t;
                q1.X = p1.X * param + p2.X * t; q1.Y = p1.Y * param + p2.Y * t;
                q2.X = p2.X * param + p3.X * t; q2.Y = p2.Y * param + p3.Y * t;
                r0.X = q0.X * param + q1.X * t; r0.Y = q0.Y * param + q1.Y * t;
                r1.X = q1.X * param + q2.X * t; r1.Y = q1.Y * param + q2.Y * t;
                float x = r0.X * param + r1.X * t;
                float y = r0.Y * param + r1.Y * t;
                t += step;
                res[i] = new PointF(x, y);
            }
            return res;
        }
        
        //Построение точек кривой Безье произвольной степени
        private PointF[] CalcCurve(PointF[] points)
        {
            float step = 0.001f;
            int curvePointsNumber = (int)(1 / step) + 1;
            PointF[] res = new PointF[curvePointsNumber];

            float t = 0;
            for (int i = 0; i < curvePointsNumber; i++)
            {
                float x = 0f, y = 0f;
                for(int j = 0;j < points.Length; j++)
                {
                    float bp = bernsteinPolynom(j, points.Length - 1, t);
                    x += points[j].X * bp;
                    y += points[j].Y * bp;
                }
                t += step;
                res[i] = new PointF(x, y);
            }

            return res;
        }
       
        private float bernsteinPolynom(int i,int n, float t)
        {
            float res = 0f;
            res = (float)factorial(n) / (factorial(i) * factorial(n - i))  * (float)Math.Pow((double)t,(double)i) * (float)Math.Pow((double)(1-t), (double)(n-i));
            return res;
        }
        
        private int factorial(int n)
        {
            int res = 1;
            for (int i = 1; i <= n; i++)
                res *= i;
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
