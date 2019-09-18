using System;
using System.Collections.Generic;
using System.Linq;

namespace RamerDouglasPeuckerNetV2.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Point> points = Data.Array.Select(d => new Point(d[0], d[1])).ToList();
            Console.WriteLine(points.Count);

            List<Point> reducedPoints = RamerDouglasPeucker.Reduce(points, 0.5f);
            Console.WriteLine(reducedPoints.Count);
            Console.ReadKey();
        }
    }
}
