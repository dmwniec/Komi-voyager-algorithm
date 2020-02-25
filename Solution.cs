using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komivoyager
{
    class Solution
    {
        public static ArrayList FindRoute(Matrix matrix)
        {
            double[] points = new double[matrix.Value.GetLength(0)];
            ArrayList route = new ArrayList();
            route.Add(0);
            route.Add(0);
            for (int i = 0; i < points.GetLength(0); i++) points[i] = matrix.Value[i, 0];

           
            double max;
            int indexofmax;
            
                max = points.Max();
                indexofmax = Array.IndexOf(points, max);
                points[indexofmax] = double.MinValue;
            route.Insert(1, indexofmax);
          
            for (int i = 2; i < points.GetLength(0); i++)
            {
                max = points.Max();
                indexofmax = Array.IndexOf(points, max);
                double[] check = new double[route.Count - 1];
                
                for (int j = 0; j < route.Count - 1; j++)
                {


                    check[j] = matrix.Value[Convert.ToInt32(route[j]), indexofmax]
                       + matrix.Value[indexofmax, Convert.ToInt32(route[j + 1])]
                       - matrix.Value[Convert.ToInt32(route[j]), Convert.ToInt32(route[j + 1])];
                    

                }
                
                var checkmin = check.Min();
                double[] newpoints = new double[matrix.Value.GetLength(0)];
                for (int k = 0; k < points.GetLength(0); k++)
                {
                    newpoints[k] = matrix.Value[k, indexofmax];

                }

                for (int k = 0; k < points.GetLength(0); k++)
                {

                    if ((newpoints[k] < 0) && (newpoints[k] < points[k])) points[k] = newpoints[k];
                }

                points[indexofmax] = double.MinValue;
                

                var place = Array.IndexOf(check, checkmin);
                route.Insert(place+1, indexofmax);
               

            }
            
            return route;
        }
    }
}
