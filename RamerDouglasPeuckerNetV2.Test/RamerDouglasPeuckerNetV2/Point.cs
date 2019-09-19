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

        public override bool Equals(object obj)
        {
            if (obj is Point point)
            {
                if (this.X != point.X || this.Y != point.Y) return false;
                return true;
            }
            return false;
        }
    }
}
