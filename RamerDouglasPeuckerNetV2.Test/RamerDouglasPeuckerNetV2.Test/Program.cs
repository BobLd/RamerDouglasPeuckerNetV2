using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RamerDouglasPeuckerNetV2.Test
{
    class Program
    {
        static Random random = new Random(42);

        static void Main(string[] args)
        {
            List<Point> points = Enumerable.Range(0, 50_000).Select(x => new Point(x, (float)random.NextDouble())).ToList();
            //List<Point> points = Data.Array.Select(d => new Point(d[0], d[1])).ToList();

            Console.WriteLine(points.Count().ToString("#,0") + " points");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Point> reducedPoints = RamerDouglasPeucker.Reduce(points, 0.5f);
            stopwatch.Stop();
            Console.WriteLine("To " + reducedPoints.Count().ToString("#,0") + " points in " + stopwatch.ElapsedMilliseconds + "ms");
            Console.ReadKey();
        }
    }
}
