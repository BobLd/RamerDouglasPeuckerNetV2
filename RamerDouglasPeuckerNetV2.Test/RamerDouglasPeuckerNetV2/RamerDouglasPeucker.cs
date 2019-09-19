using System.Collections.Generic;

namespace RamerDouglasPeuckerNetV2
{
    /***************************************************************************
      * Ramer–Douglas–Peucker algorithm
      * The purpose of the algorithm is, given a curve composed of line segments 
      * (which is also called a Polyline in some contexts), to find a similar 
      * curve with fewer points. The algorithm defines 'dissimilar' based on the 
      * maximum distance between the original curve and the simplified curve 
      * (i.e., the Hausdorff distance between the curves). The simplified curve 
      * consists of a subset of the points that defined the original curve.
      * [https://en.wikipedia.org/wiki/Ramer%E2%80%93Douglas%E2%80%93Peucker_algorithm]
      * 
      * The pseudo-code is available on the wikipedia page.
      * In this implementation, we use the perpendicular distance. In order to 
      * make the algorithm faster, we consider the squared distance (and epsilon) 
      * so that we avoid using the 'abs' and 'sqrt' in the distance computation.
      * We also split the computation of the distance so that we put in the 'for 
      * loop' only what is needed.
      * 
      * The non-parametric version follows 'A novel framework for making dominant 
      * point detection methods non-parametric' by Prasad, Leung, Quek, and Cho.
      **************************************************************************/

    /// <summary>
    /// The purpose of the algorithm is, given a curve composed of line segments
    /// to find a similar curve with fewer points.
    /// </summary>
    public static class RamerDouglasPeucker
    {
        /// <summary>
        /// Uses the Ramer Douglas Peucker algorithm to reduce the number of points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="epsilon">The tolerance.</param>
        /// <returns></returns>
        public static List<Point> Reduce(List<Point> points, float epsilon)
        {
            if (points == null || points.Count < 3) return points;
            if (float.IsInfinity(epsilon) || float.IsNaN(epsilon)) return points;
            epsilon *= epsilon; // we use squared distance
            if (epsilon <= float.Epsilon) return points;

            List<Point> firsts = new List<Point>();
            while (points[0].Equals(points[points.Count - 1]))
            {
                firsts.Add(points[0]);
                points.RemoveAt(0);
            }

            firsts.AddRange(reduce(points, epsilon));
            return firsts;
        }

        private static List<Point> reduce(List<Point> points, float epsilon)
        {
            float dmax = 0;
            int index = 0;

            Point point1 = points[0];
            Point point2 = points[points.Count - 1];

            float distXY = point1.X * point2.Y - point2.X * point1.Y;
            float distX = point2.X - point1.X;
            float distY = point2.Y - point1.Y;
            float denominator = distX * distX + distY * distY;

            for (int i = 1; i < (points.Count - 2); i++) // -2 or -1?
            {
                // Compute perpendicular distance squared
                var current = points[i];
                float numerator = distXY + distX * current.Y - distY * current.X;
                float d = (numerator / denominator) * numerator;

                if (d > dmax)
                {
                    index = i;
                    dmax = d;
                }
            }

            // If max distance is greater than epsilon, recursively simplify
            if (dmax > epsilon)
            {
                // Recursive call
                var recResults1 = reduce(points.GetRange(0, index + 1), epsilon);
                var recResults2 = reduce(points.GetRange(index, points.Count - index), epsilon);

                // Build the result list
                recResults1.RemoveAt(recResults1.Count - 1);
                recResults1.AddRange(recResults2);
                return recResults1;
            }
            else
            {
                return new List<Point> { point1, point2 };
            }
        }

        /// <summary>
        /// Uses the non-parametric Ramer Douglas Peucker algorithm to reduce the number of points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns></returns>
        public static List<Point> Reduce(List<Point> points)
        {
            List<Point> firsts = new List<Point>();
            while (points[0].Equals(points[points.Count - 1]))
            {
                firsts.Add(points[0]);
                points.RemoveAt(0);
            }

            firsts.AddRange(reduceNP(points));
            return firsts;
        }

        /// <summary>
        /// Non-parametric Ramer Douglas Peucker algorithm.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private static List<Point> reduceNP(List<Point> points)
        {
            float dmax = 0;
            int index = 0;

            Point point1 = points[0];
            Point point2 = points[points.Count - 1];

            float distXY = point1.X * point2.Y - point2.X * point1.Y;
            float distX = point2.X - point1.X;
            float distY = point2.Y - point1.Y;
            float denominator = distX * distX + distY * distY;
            float epsilon = ComputeEpsilon(distX, distY);

            for (int i = 1; i < (points.Count - 2); i++) // -2 or -1?
            {
                // Compute perpendicular distance squared
                var current = points[i];
                float numerator = distXY + distX * current.Y - distY * current.X;
                float d = (numerator / denominator) * numerator;

                if (d > dmax)
                {
                    index = i;
                    dmax = d;
                }
            }

            // If max distance is greater than epsilon, recursively simplify
            if (dmax > epsilon)
            {
                // Recursive call
                var recResults1 = reduceNP(points.GetRange(0, index + 1));
                var recResults2 = reduceNP(points.GetRange(index, points.Count - index));

                // Build the result list
                recResults1.RemoveAt(recResults1.Count - 1);
                recResults1.AddRange(recResults2);
                return recResults1;
            }
            else
            {
                return new List<Point> { point1, point2 };
            }
        }

        /// <summary>
        /// Follows 'A novel framework for making dominant point detection methods non-parametric'
        /// by Prasad, Leung, Quek, and Cho.
        /// </summary>
        /// <param name="distX">point2.X - point1.X</param>
        /// <param name="distY">point2.Y - point1.Y</param>
        /// <returns></returns>
        private static float ComputeEpsilon(float distX, float distY)
        {
            float m = distY / distX;                                                // slope
            double s = System.Math.Sqrt((double)(distX * distX + distY * distY));   // distance
            double invS = 1.0 / s;
            double phi = System.Math.Atan(m);
            double cosPhi = System.Math.Cos(phi);
            double sinPHi = System.Math.Sin(phi);
            double tmax = invS * (System.Math.Abs(cosPhi) + System.Math.Abs(sinPHi));
            double poly = 1 - tmax + tmax * tmax;
            double partialPhi = System.Math.Max(System.Math.Atan(invS * System.Math.Abs(sinPHi + cosPhi) * poly),
                                                System.Math.Atan(invS * System.Math.Abs(sinPHi - cosPhi) * poly));
            float dmax = (float)(s * partialPhi);
            return dmax * dmax; // we use squared distance
        }
    }
}
