using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Rasterization
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static int pixel = 1;
        private static int midPointX = 0;
        private static int midPointY = 0;
        private static HashSet<ValueTuple<int, int>> points = new HashSet<ValueTuple<int, int>>();

        private void GetLine(int x1, int y1, int x2, int y2)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int startY, endY;
            if (y1 >= y2)
            {
                startY = y2;
                endY = y1;
            }
            else
            {
                startY = y1;
                endY = y2;
            }

            int startX, endX;
            if (x1 > x2)
            {
                startX = x2;
                endX = x1;
            }
            else
            {
                startX = x1;
                endX = x2;
            }

            if (x1 == x2)
            {
                for (int i = startY; i <= endY; i++)
                {
                    points.Add(new ValueTuple<int, int>(x1, i));
                }
            }
            else 
            {
                double k = (y1 - y2) * 1.0 / (x1 - x2);
                double b = y1 - k * x1;
                if (Math.Abs(k) >= 1)
                {
                    for (int i = startY; i <= endY; i++)
                    {
                        points.Add(new ValueTuple<int, int>((int)Math.Round((i - b) / k), i));
                    }
                }
                else
                {
                    for (int i = startX; i <= endX; i++)
                    {
                        points.Add(new ValueTuple<int, int>(i, (int)Math.Round(k * i + b)));
                    }
                }
            }

            stopwatch.Stop();
            label6.Text = $"Время: {stopwatch.ElapsedTicks} ms";
        }

        private void Cda(int x1, int y1, int x2, int y2)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int L = Math.Max(Math.Abs(x2 - x1), Math.Abs(y2 - y1));
            double dX = (x2 - x1) * 1.0 / L;
            double dY = (y2 - y1) * 1.0 / L;
            points.Add(new ValueTuple<int, int>(x1, y1));
            double prevX = x1;
            double prevY = y1;
            int i = 1;
            while (i < L)
            {
                prevX = prevX + dX;
                prevY = prevY + dY;
                points.Add(new ValueTuple<int, int>((int)Math.Round(prevX), (int)Math.Round(prevY)));
                i++;
            }

            points.Add(new ValueTuple<int, int>(x2, y2));

            stopwatch.Stop();
            label6.Text = $"Время: {stopwatch.ElapsedTicks} ms";
        }

        private void BrezenhemLine(int x1, int y1, int x2, int y2)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if (steep)
            {
                Swap(ref x1, ref y1);
                Swap(ref x2, ref y2);
            }

            if (x1 > x2)
            {
                Swap(ref x1, ref x2);
                Swap(ref y1, ref y2);
            }

            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int error = dx / 2;
            int ystep = (y1 < y2) ? 1 : -1;
            int y = y1;
            for (int x = x1; x <= x2; x++)
            {
                points.Add(new ValueTuple<int, int>(steep ? y : x, steep ? x : y));
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }

            stopwatch.Stop();
            label6.Text = $"Время: {stopwatch.ElapsedTicks} tics";
        }

        private void Swap(ref int x1, ref int x2)
        {
            int t = x2;
            x2 = x1;
            x1 = t;
        }

        //points.Add(new ValueTuple<int, int>(x, y));
        private void BrezenhemCircle(int x1, int y1, int r)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int x = 0;
            int y = r;
            int e = 3 - 2 * r;
            points.Add(new ValueTuple<int, int>(x + x1, y + y1));
            points.Add(new ValueTuple<int, int>(x + x1, -y + y1));
            points.Add(new ValueTuple<int, int>(-x + x1, y + y1));
            points.Add(new ValueTuple<int, int>(-x + x1, -y + y1));

            points.Add(new ValueTuple<int, int>(y + x1, x + y1));
            points.Add(new ValueTuple<int, int>(-y + x1, x + y1));
            points.Add(new ValueTuple<int, int>(y + x1, -x + y1));
            points.Add(new ValueTuple<int, int>(-y + x1, -x + y1));
            while (x < y)
            {
                if (e >= 0)
                {
                    e = e + 4 * (x - y) + 10;
                    x = x + 1;
                    y = y - 1;
                }
                else
                {
                    e = e + 4 * x + 6;
                    x = x + 1;
                }

                points.Add(new ValueTuple<int, int>(x + x1, y + y1));
                points.Add(new ValueTuple<int, int>(x + x1, -y + y1));
                points.Add(new ValueTuple<int, int>(-x + x1, y + y1));
                points.Add(new ValueTuple<int, int>(-x + x1, -y + y1));

                points.Add(new ValueTuple<int, int>(y + x1, x + y1));
                points.Add(new ValueTuple<int, int>(-y + x1, x + y1));
                points.Add(new ValueTuple<int, int>(y + x1, -x + y1));
                points.Add(new ValueTuple<int, int>(-y + x1, -x + y1));

                stopwatch.Stop();
                label6.Text = $"Время: {stopwatch.ElapsedTicks} ms";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetLine(int.Parse(tbx.Text), int.Parse(tbY.Text), 
                int.Parse(tbX2.Text), int.Parse(tbY2.Text));

            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int w = 1000;
            int h = 600;

            Font drawFont = new Font("Arial", 10);  
            SolidBrush drawBrush = new SolidBrush(Color.Blue);
            int startX = -w / (pixel * 2) + midPointX;
            int endX = startX + w / pixel; 
            int padding = 30;
            int full = endX - startX;
            int pause = full / 20;
            int k = 0;
            for (int i = startX; i <= endX; i += pause / 2)
            {
                int newX = (i - startX) * pixel + padding;
                if (k % 2 == 0)
                    e.Graphics.DrawString(i.ToString(), drawFont, drawBrush, newX - 5, 0);

                e.Graphics.DrawLine(new Pen(Brushes.BlueViolet), new Point(newX,
                    padding), new Point(newX, h + padding));

                k++;
            }

            k = 0;
            int startY = -h / (pixel * 2) + midPointY;
            int endY = startY + h / pixel;
            full = endY - startY;
            pause = full / 10;

            for (int i = startY; i <= endY; i += pause / 2)
            {
                int newY = (i - startY) * pixel + padding;
                if (k % 2 == 0)
                    e.Graphics.DrawString(i.ToString(), drawFont, drawBrush, 0, newY - 5);

                
                e.Graphics.DrawLine(new Pen(Brushes.BlueViolet), new Point(padding,
                    newY), new Point(w + padding, newY));

                k++;
            }

            drawAllPoints(e.Graphics);
            if (startX <= 0 && endX >= 0)
            {
                int newX = -startX * pixel + padding;
                e.Graphics.DrawLine(new Pen(Brushes.Red), new Point(newX,
                        padding), new Point(newX, h + padding));
            }

            if (startY <= 0 && endY >= 0)
            {
                int newY = -startY * pixel + padding;
                e.Graphics.DrawLine(new Pen(Brushes.Red), new Point(padding,
                        newY), new Point(w + padding, newY));
            }
        }

        private void drawAllPoints(Graphics g)
        {
            int frameLeftX = midPointX - 500 / pixel;
            int frameRightX = midPointX + 500 / pixel;
            int frameLeftY = midPointY - 300 / pixel;
            int frameRightY = midPointY + 300 / pixel;

            int newP = 25 / pixel;

            foreach (ValueTuple<int, int> tuple in points)
            {
                if (tuple.Item1 >= frameLeftX && tuple.Item1 < frameRightX &&
                    tuple.Item2 >= frameLeftY && tuple.Item2 < frameRightY)
                {
                    int newX = (tuple.Item1 - frameLeftX) * pixel + 30;
                    int newY = (tuple.Item2 - frameLeftY) * pixel + 30;
                    g.FillRectangle(Brushes.Green, newX, newY, pixel, pixel);
                }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int frameLength = 500 / pixel;
            int frameHeight = 300 / pixel;
            int realX = (e.X - 30) / pixel - frameLength;
            int realY = (e.Y - 30) / pixel - frameHeight;

            /*midPointX += realX;
            midPointY += realY;*/

            if (e.Button == MouseButtons.Right)
            {
                if (pixel != 1)
                {
                    pixel /= 5;
                    midPointX += realX;
                    midPointY += realY;
                }
            }
            else
            {
                if (pixel < 25)
                {
                    pixel *= 5;
                    midPointX += realX;
                    midPointY += realY;
                }
            }

            if (pixel == 1)
            {
                midPointX = 0;
                midPointY = 0;
            }
            
            pictureBox1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            points.Clear();
            pictureBox1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            Cda(int.Parse(tbx.Text), int.Parse(tbY.Text),
            int.Parse(tbX2.Text), int.Parse(tbY2.Text));

            pictureBox1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BrezenhemLine(int.Parse(tbx.Text), int.Parse(tbY.Text),
                int.Parse(tbX2.Text), int.Parse(tbY2.Text));

            pictureBox1.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BrezenhemCircle(int.Parse(tbx.Text), int.Parse(tbY.Text),
                int.Parse(tbRadius.Text));

            pictureBox1.Refresh();
        }
    }
}
