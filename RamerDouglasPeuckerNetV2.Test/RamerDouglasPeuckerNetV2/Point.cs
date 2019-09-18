namespace RamerDouglasPeuckerNetV2
{
    public struct Point
    {
        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public override string ToString()
        {
            return X.ToString() + ", " + Y.ToString();
        }
    }
}
