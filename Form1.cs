namespace SegmentClipping
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
        }

        bool linesDrew = false;
        bool windowDrew = false;
        bool refresh = false;
        int[] edges = new Int32[4];
        List<Point> randLinesVertex = new List<Point>();


        Graphics g;
        Bitmap bmp;

        private void Form1_Load(object sender, EventArgs e)
        {
            label3.Text = "N/A";
            label4.Text = "N/A";
            Print_System();
        }

        private void Print_System()
        {
            if (refresh)
            {
                linesDrew = false;
                windowDrew = false;
                edges = new Int32[4];
                randLinesVertex = new List<Point>();
                refresh = false;
                pictureBox1.Invalidate();
            }

            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            int w = pictureBox1.ClientSize.Width / 2;
            int h = pictureBox1.ClientSize.Height / 2;
            g.TranslateTransform(w, h);

            g.DrawLine(Pens.Black, new Point(-w, 0), new Point(w, 0));
            g.DrawLine(Pens.Black, new Point(0, h), new Point(0, -h));
            g.DrawLine(Pens.Black, new Point(w - 1, h), new Point(w - 1, -h));
            g.DrawLine(Pens.Black, new Point(-w, h), new Point(-w, -h));
            g.DrawLine(Pens.Black, new Point(-w, -h), new Point(w, -h));
            g.DrawLine(Pens.Black, new Point(-w, h - 1), new Point(w, h - 1));
            pictureBox1.Image = bmp;

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int w = pictureBox1.ClientSize.Width / 2;
            int h = pictureBox1.ClientSize.Height / 2;
            int x = Convert.ToInt32(e.X);
            int y = Convert.ToInt32(e.Y);
            label3.Text = (x - w).ToString();
            label4.Text = (-(y - h)).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clearText();
            refresh = true;
            Print_System();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            g = Graphics.FromImage(bmp);
            int w = pictureBox1.ClientSize.Width / 2;
            int h = pictureBox1.ClientSize.Height / 2;
            g.TranslateTransform(w, h);

            int x = Convert.ToInt32(label3.Text);
            int y = Convert.ToInt32(label4.Text);
            if ((textBox1.Text != "" && textBox2.Text != "") && (textBox3.Text == "" && textBox4.Text == ""))
            {
                textBox3.Text = x.ToString();
                textBox4.Text = y.ToString();
                g.FillRectangle(Brushes.DarkRed, x, -y, 5, 5);
                drawWindow();
            }
            if (textBox1.Text == "" && textBox2.Text == "")
            {
                textBox1.Text = x.ToString();
                textBox2.Text = y.ToString();
                g.FillRectangle(Brushes.DarkRed, x, -y, 5, 5);
            }
            pictureBox1.Image = bmp;
        }

        private void lineClipping(int lineNum)
        {
            g = Graphics.FromImage(bmp);
            int w = pictureBox1.ClientSize.Width / 2;
            int h = pictureBox1.ClientSize.Height / 2;
            g.TranslateTransform(w, h);

            int k = 0;
            int x3 = 0, x4 = 0, y3 = 0, y4 = 0;
            int x1 = randLinesVertex[lineNum].X;
            int x2 = randLinesVertex[lineNum + 1].X;
            int y1 = randLinesVertex[lineNum].Y;
            int y2 = randLinesVertex[lineNum + 1].Y;
            int xmin = edges[0];
            int xmax = edges[1];
            int ymax = edges[2];
            int ymin = edges[3];
            double xn = x1;
            double xk = x2;
            double yn = y1;
            double yk = y2;
            if (yn > -ymin) { xn = x1 + (x2 - x1) * (-ymin - y1) / (y2 - y1); yn = -ymin; }
            if (yk > -ymin) { xk = x2 + (x1 - x2) * (-ymin - y2) / (y1 - y2); yk = -ymin; }
            if (yn < -ymax) { xn = x1 + (x2 - x1) * (-ymax - y1) / (y2 - y1); yn = -ymax; }
            if (yk < -ymax) { xk = x2 + (x1 - x2) * (-ymax - y2) / (y1 - y2); yk = -ymax; }
            if (xn < xmin) { yn = y1 + (y2 - y1) * (xmin - x1) / (x2 - x1); xn = xmin; }
            if (xk < xmin) { yk = y2 + (y1 - y2) * (xmin - x2) / (x1 - x2); xk = xmin; }
            if (xn > xmax) { yn = y1 + (y2 - y1) * (xmax - x1) / (x2 - x1); xn = xmax; }
            if (xk > xmax) { yk = y2 + (y1 - y2) * (xmax - x2) / (x1 - x2); xk = xmax; }

            if ((xn >= xmin) && (xn <= xmax)) { x3 = Convert.ToInt32(xn); } else { k += 2; };
            if ((xk >= xmin) && (xk <= xmax)) { x4 = Convert.ToInt32(xk); } else { k += 2; };
            if ((yn <= -ymin) && (yn >= -ymax)) { y3 = Convert.ToInt32(yn); } else { k += 2; };
            if ((yk <= -ymin) && (yk >= -ymax)) { y4 = Convert.ToInt32(yk); } else { k += 2; };

            if (k < 2)
            {
                g.DrawLine(Pens.DarkRed, new Point(x3, y3), new Point(x4, y4));
            }

            pictureBox1.Image = bmp;
        }

        private void drawWindow()
        {
            g = Graphics.FromImage(bmp);
            int w = pictureBox1.ClientSize.Width / 2;
            int h = pictureBox1.ClientSize.Height / 2;
            g.TranslateTransform(w, h);

            int x1 = Convert.ToInt32(textBox1.Text);
            int y1 = Convert.ToInt32(textBox2.Text);
            int x2 = Convert.ToInt32(textBox3.Text);
            int y2 = Convert.ToInt32(textBox4.Text);

            int winW = Math.Abs(x1 - x2);
            int winH = Math.Abs(y1 - y2);

            Rectangle window = new Rectangle(x2, y1, winW, winH);

            if (x1 >= x2)
            {
                if (y1 >= y2)
                {
                    window = new Rectangle(x2, -y1, winW, winH);
                    edges[0] = x2;
                    edges[1] = x1;
                    edges[2] = y1;
                    edges[3] = y2;

                }
                else
                {
                    window = new Rectangle(x2, -y2, winW, winH);
                    edges[0] = x2;
                    edges[1] = x1;
                    edges[2] = y2;
                    edges[3] = y1;
                }
            }
            else
            {
                if (y1 >= y2)
                {
                    window = new Rectangle(x1, -y1, winW, winH);
                    edges[0] = x1;
                    edges[1] = x2;
                    edges[2] = y1;
                    edges[3] = y2;
                }
                else
                {
                    window = new Rectangle(x1, -y2, winW, winH);
                    edges[0] = x1;
                    edges[1] = x2;
                    edges[2] = y2;
                    edges[3] = y1;
                }
            }
            g.DrawRectangle(new Pen(Brushes.DarkRed), window);
            pictureBox1.Image = bmp;
            windowDrew = true;
        }

        private void clearText()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!linesDrew)
            {
                g = Graphics.FromImage(bmp);
                int w = pictureBox1.ClientSize.Width / 2;
                int h = pictureBox1.ClientSize.Height / 2;
                g.TranslateTransform(w, h);

                Random rnd = new Random();

                int count = rnd.Next(1, 20);
                int x1 = 0;
                int y1 = 0;
                Point[] linePoints = new Point[count * 2];
                for (int i = 0; i < linePoints.Length; i++)
                {
                    int x2 = rnd.Next(-w, w);
                    int y2 = rnd.Next(-h, h);
                    if (i != 0)
                    {
                        x1 = linePoints[i - 1].X;
                        y1 = linePoints[i - 1].Y;
                    }
                    while (y2 == y1 && x2 == x1 && i != 0)
                    {
                        x2 = rnd.Next(-w, w);
                        y2 = rnd.Next(-h, h);
                    }
                    linePoints[i] = new Point(x2, y2);
                }
                for (int i = 0; i < linePoints.Length; i += 2)
                {
                    g.DrawLine(Pens.MediumAquamarine, linePoints[i], linePoints[i + 1]);
                }
                pictureBox1.Image = bmp;
                linesDrew = true;
                randLinesVertex = linePoints.ToList();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (linesDrew == true && windowDrew == true)
            {
                for (int i = 0; i < randLinesVertex.Count; i += 2)
                {
                    lineClipping(i);
                }
            }
        }
    }
}