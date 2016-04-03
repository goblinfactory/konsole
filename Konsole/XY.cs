namespace Konsole
{
    public struct XY
    {
        public int X;
        public int Y;
        public XY(int x, int y) { X = x; Y = y; }
        public XY WithX(int x)  { return new XY(x, Y); }
        public XY IncX(int x)  { return new XY(x + X, Y); }
        public XY WithY(int y)  { return new XY(X, y); }
        public XY IncY(int y)  { return new XY(X, Y+y); }
        public override string ToString()
        {
            return string.Format("{0},{1}",X,Y);
        }
    }
}