using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Komivoyager
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }
        private Point[] pointer = new Point[0];

        internal Point[] Pointer { get => pointer; set => pointer = value; }

        private void GenerateRandom_Button_Click(object sender, RoutedEventArgs e)
        {
            PointsMap.Children.Clear();
            if (Convert.ToInt32(AX_TextBoX.Text) > 439) AX_TextBoX.Text = "439";
            if (Convert.ToInt32(AY_TextBoX.Text) > 439) AY_TextBoX.Text = "439";
            if (Convert.ToInt32(BX_TextBoX.Text) > 450) BX_TextBoX.Text = "450";
            if (Convert.ToInt32(BY_TextBoX.Text) > 450) BY_TextBoX.Text = "450";


            Point A = new Point(Convert.ToInt32(AX_TextBoX.Text), Convert.ToInt32(AY_TextBoX.Text));
            Point B = new Point(Convert.ToInt32(BX_TextBoX.Text), Convert.ToInt32(BY_TextBoX.Text));
            
            if (Point.ABValid(A, B))    
            {
                Point[] pointlist = Point.RandomPoints(A, B, Convert.ToInt32(PointsCount_TextBox.Text));
                PointsMap.Children.Add(Point.AddEllipseAB(A));
                PointsMap.Children.Add(Point.AddEllipseAB(B));
                PointsMap.Children.Add(A.AddLabel("A"));
                PointsMap.Children.Add(B.AddLabel("B"));
                Line[] lines = Point.DrawRectange(A, B);
                foreach(var l in lines) PointsMap.Children.Add(l);
                int w = 1;
                foreach (var p in pointlist)
                {
                    PointsMap.Children.Add(p.AddLabel(w.ToString()));
                    PointsMap.Children.Add(p.AddEllipse());
                    w++;
                }
                Pointer = pointlist;
               
            }
            
        }

        private void LoadFromFile_Button_Click(object sender, RoutedEventArgs e)
        {
            PointsMap.Children.Clear();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "Document";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            Nullable<bool> result = dlg.ShowDialog();
            string filename = "";

            if (result == true)
            {

                filename = dlg.FileName;
            }
            Point[] pointlist = Point.LoadPoints((Convert.ToInt32(PointsCount_TextBox.Text) + 2), filename);
            Point A = pointlist[0];
            Point B = pointlist[1];
            Point[] whAB = new Point[pointlist.GetLength(0) - 2];
            for (int i = 2; i < pointlist.GetLength(0); i++) whAB[i - 2] = pointlist[i];
            AX_TextBoX.Text = A.PointtoTextX();
            AY_TextBoX.Text = A.PointtoTextY();
            BX_TextBoX.Text = B.PointtoTextX();
            BY_TextBoX.Text = B.PointtoTextY();
            Line[] lines = Point.DrawRectange(A, B);
            foreach (var l in lines) PointsMap.Children.Add(l);
            if (Point.ABValid(A, B))
            {
                
                PointsMap.Children.Add(Point.AddEllipseAB(A));
                PointsMap.Children.Add(Point.AddEllipseAB(B));
                PointsMap.Children.Add(A.AddLabel("A"));
                PointsMap.Children.Add(B.AddLabel("B"));
                
                for (int i = 0; i<whAB.GetLength(0); i++)
                {
                    PointsMap.Children.Add(whAB[i].AddLabel((i+1).ToString()));
                    PointsMap.Children.Add(whAB[i].AddEllipse()); ;
                    
                }
                Pointer = whAB;
            }


        }

        private void SaveToFile_Button_Click(object sender, RoutedEventArgs e)
        {
            Point A = new Point(Convert.ToInt32(AX_TextBoX.Text), Convert.ToInt32(AY_TextBoX.Text));
            Point B = new Point(Convert.ToInt32(BX_TextBoX.Text), Convert.ToInt32(BY_TextBoX.Text));
            Point[] pointlist = new Point[Pointer.GetLength(0)+2];
            pointlist[0] = A;
            pointlist[1] = B;
            for (int i = 2; i < pointlist.GetLength(0); i++) pointlist[i] = pointer[i - 2];
            
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                Point.PrintPoints(pointlist, saveFileDialog1.FileName);
            }
        }

        private void Solution_Button_Click(object sender, RoutedEventArgs e)
        {
            if (pointer.Length > 0)
            {
                Point[] pointlist = new Point[Pointer.GetLength(0) + 1];
                Point A = new Point(Convert.ToInt32(AX_TextBoX.Text), Convert.ToInt32(AY_TextBoX.Text));
                pointlist[0] = A;
                for (int i = 1; i < pointlist.GetLength(0); i++) pointlist[i] = Pointer[i - 1];
                Matrix matrix = Point.PointsToMatrix(pointlist);
                ArrayList route = Solution.FindRoute(matrix);
                
                
                
                for (int i = 0; i < route.Count - 1; i++)
                {
                    
                    Point S = pointlist[Convert.ToInt32(route[i])];
                    Point H = pointlist[Convert.ToInt32(route[i+1])];
                    PointsMap.Children.Add(Point.DrawLine(S, H));
                }

                string res = "Trasa = ";
                foreach (var it in route)
                {
                    res += it + " - ";
                }
                double length = 0;
                for (int i = 1; i < route.Count -1 ; i++)
                {
                    length += matrix.Value[Convert.ToInt32(route[i - 1]), Convert.ToInt32(route[i])];
                }
                res += "Długość: " + length;
                
                result_label.Visibility = Visibility.Visible;
                result_label.Content = res;
            }
        }
    }
}
