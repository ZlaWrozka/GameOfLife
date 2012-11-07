using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls;


namespace GameOfLife
{
    class Board
    {
        // fields
        private Canvas canvas;                                      // board canvas
        private Rectangle[,] rectangles;                            // map of rectangles (cell bodies)
        private List<Line> lines = new List<Line>();                // board lines container
        TextBlock tb = new TextBlock();                             // statistics field

        // properties
        public Color BackgroundCol { get; set; }                    // board background color
        public Color LineCol { get; set; }                          // board lines color
        public int X { get; set; }                                  // size of a board
        public int Y { get; set; }
        protected double DeltaX { get; set; }                       // interval between two x lines
        protected double DeltaY { get; set; }                       // interval between two y lines

        // constructors
        public Board(Color backgroundCol, Color lineCol, int linesX, int linesY)
        {
            BackgroundCol = backgroundCol;
            LineCol = lineCol;
            X = linesX;
            Y = linesY;
            this.rectangles = new Rectangle[X, Y];
            SetCanvas();
            SetLines();
        }
        public Board() { }

        // set lines position on the board and add them to the list
        private void SetLines()
        {
                double i = 0.0;
                while (i <= canvas.Width)
                {
                    Line line = new Line();
                    line.Stroke = new SolidColorBrush(LineCol);
                    line.X1 = i;
                    line.Y1 = 0;
                    line.X2 = i;
                    line.Y2 = canvas.Height;
                    line.StrokeThickness = 1;
                    this.lines.Add(line);
                    i += DeltaX;
                }
                i = 0.0;
                while (i <= canvas.Height)
                {
                    Line line = new Line();
                    line.Stroke = new SolidColorBrush(LineCol);
                    line.X1 = 0;
                    line.Y1 = i;
                    line.X2 = canvas.Width;
                    line.Y2 = i;
                    line.StrokeThickness = 1;
                    this.lines.Add(line);
                    i += DeltaY;
                }
  
        }

        // draw lines on board
        internal void DrawLines(bool linesOn)
        {
            if (linesOn)
            {
                foreach (Line l in this.lines)
                {
                    canvas.Children.Add(l);
                }
            }
            else
            {
                foreach (Line l in this.lines)
                {
                    canvas.Children.Remove(l);
                }
            }

        }

        // window and canvas properties
        private void SetCanvas()
        {
            canvas = new Canvas();
            canvas.Background = new SolidColorBrush(BackgroundCol);

            Window window = Application.Current.MainWindow;
            window.Content = canvas;
            window.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            window.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            canvas.Width = window.Width;
            canvas.Height = window.Height;

            this.DeltaX = canvas.Width / X;
            this.DeltaY = canvas.Height / Y;

            double x = 0.0;
            double y = 0.0;
            double recSize = (DeltaX < DeltaY) ? DeltaX / 1.2 : DeltaY / 1.2;

            for (int i = 0; i < X; ++i)
            {
                for (int j = 0; j < Y; ++j)
                {
                    Rectangle rec = new Rectangle();
                    rec.Width = recSize;
                    rec.Height = recSize;
                    rectangles[i, j] = rec;
                    Canvas.SetLeft(rec, x + 0.5 * DeltaX - 0.5 * recSize);
                    Canvas.SetTop(rec, y + 0.5 * DeltaY - 0.5 * recSize);
                    canvas.Children.Add(rec);
                    y += this.DeltaY;
                }
                x += this.DeltaX;
                y = 0.0;
            }

            canvas.Children.Add(tb);
        }

        // draw cells
        internal void DrawPopulation(CellMap current, CellMap former)
        {
            for (int i = 0; i < current.X; ++i)
            {
                for (int j = 0; j < current.Y; ++j)
                {
                    if (current[i, j].IsAlive)
                    {
                        if (former[i, j].IsAlive == false)
                            DrawCell(Cell.LivingCol, i, j);
                    }
                    else
                    {
                        if (former[i, j].IsAlive == true)
                            DrawCell(Cell.DeadCol, i, j);
                    }
                }
            }
        }

        // draw one cell
        private void DrawCell(Color color, int i, int j)
        {
            Brush brush = new SolidColorBrush(color);
            this.rectangles[i, j].Fill = brush;
        }

        // set staitistics field
        internal void SetInfo(string info, bool gameOn, int interval)
        {
            tb.Height = 100;
            tb.Width = 500;
            tb.Text = string.Format("Step: {0,-5}", info);
            tb.FontSize = 24;
            if (!gameOn)
                tb.Text += " PAUSED";
            else
                tb.Text += string.Format(" Drawing interval: {0} ms", interval);
        }

    }
}
