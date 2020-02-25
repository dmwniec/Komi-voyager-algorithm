using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Komivoyager
{
    class Point 
    {
        private static Random rng = new Random();
        private int _X;
        private int _Y;


        public Point(int x, int y)
        {
            _X = x;
            _Y = y;
        }
        public static bool ABValid(Point A, Point B)
        {
            bool Valid = false;
            if (A._X < B._X || A._Y < B._Y) Valid = true;

            return Valid;
        }

        private static double Distance(Point A, Point B)
        {
            double distance;
            distance = Math.Sqrt(Math.Pow((B._X - A._X), 2) + Math.Pow((B._Y - B._Y), 2));
            return distance;
        }

        private static Point GenerateRandomPoint(Point A, Point B)
        {

            int x = rng.Next(A._X + 1, B._X - 1);
            int y = rng.Next(A._Y + 1, B._Y - 1);
            Point point = new Point(x, y);
            return point;
        }
        public static Point[] RandomPoints(Point A, Point B, int Amount)
        {
            Point[] pointlist = new Point[Amount];
            for (int i = 0; i < Amount; i++) pointlist[i] = GenerateRandomPoint(A, B);
            return pointlist;
        }
        public static Matrix PointsToMatrix(Point[] pointlist)
        {
            int Count = pointlist.GetLength(0);
            Matrix matrix = new Matrix(Count);
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < Count; j++)
                {
                    if (i == j) matrix.Value[i, j] = double.MinValue;
                    else matrix.Value[i, j] = Distance(pointlist[i], pointlist[j]);
                }
            }
            return matrix;
        }
        public static void PrintPoints(Point[] pointlist, string path)
        {

            Matrix matrix = new Matrix(pointlist.GetLength(0), 2);
            int i = 0;

            foreach (var s in pointlist)
            {
                matrix.Value[i, 0] = s._X;
                matrix.Value[i, 1] = s._Y;
                i++;
            }
            Matrix.PrintToFile(matrix, path);



        }
        public static Point[] LoadPoints(int n, string path)
        {
            Matrix matrix = Matrix.LoadFromFile(n, path);
            int h = matrix.Value.GetLength(0);
            Point[] pointlist = new Point[h];
            for (int i = 0; i < h; i++)
            {
                Point point = new Point(Convert.ToInt32(matrix.Value[i, 0]), Convert.ToInt32(matrix.Value[i, 1]));
                pointlist[i] = point;
            }

            return pointlist;
        }
        public  string PointtoTextX()
        {
            _ = this;
            return _X.ToString();
        }
        public string PointtoTextY()
        {
            _ = this;
            return _Y.ToString();
        }


        public  Ellipse AddEllipse()
        {
            _ = this;
            Ellipse currentDot = new Ellipse();
            currentDot.Stroke = new SolidColorBrush(Colors.Red);
            currentDot.StrokeThickness = 3;
            Canvas.SetZIndex(currentDot, 3);
            currentDot.Height = 10;
            currentDot.Width = 10;
            currentDot.Fill = new SolidColorBrush(Colors.Red);
            currentDot.Margin = new Thickness(_X, _Y, 0, 0); 
            return currentDot;
        }
        public static Ellipse AddEllipseAB(Point S)
        {

            Ellipse currentDot = new Ellipse();
            currentDot.Stroke = new SolidColorBrush(Colors.Black);
            currentDot.StrokeThickness = 3;
            Canvas.SetZIndex(currentDot, 3);
            currentDot.Height = 10;
            currentDot.Width = 10;
            currentDot.Fill = new SolidColorBrush(Colors.Black);
            currentDot.Margin = new Thickness(S._X, S._Y, 0, 0);
            return currentDot;
        }
        public Label AddLabel ( string number)
        {
            _ = this;
            Label label = new Label();
            label.Content = number;
            label.FontSize = 22;
            label.Measure(new Size(10, 10));
            Canvas.SetLeft(label, _X);
            Canvas.SetTop(label, _Y);
            return label;

        }

        public static Line DrawLine(Point A, Point B)
        {
            Line line = new Line();
            line.Stroke = Brushes.LightSteelBlue;

            line.X1 = A._X;
            line.X2 = B._X;
            line.Y1 = A._Y;
            line.Y2 = B._Y;

            line.StrokeThickness = 3;

            return line;
        }
        public static Line[] DrawRectange(Point A, Point B)
        {
            Line[] linelist = new Line[4];

            Line Crateline(int X1, int X2, int Y1, int Y2)
            {
                Line line = new Line();
                line.Stroke = Brushes.Chocolate;

                line.X1 = X1;
                line.X2 = X2;
                line.Y1 = Y1;
                line.Y2 = Y2;

                line.StrokeThickness = 3;
                return line;
            }
            linelist[0] = Crateline(A._X, A._Y, A._X,B._Y);
            linelist[1] = Crateline(B._X, A._Y, A._X, A._Y);
            linelist[2] = Crateline(B._X, B._Y, A._X, B._Y);
            linelist[3] = Crateline(B._X, A._Y, B._X, B._Y);


            return linelist;
        }
    }
}
 