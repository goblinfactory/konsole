namespace Konsole
{
    public class Column
    {
        public Column(string name, int width = 0, Colors colors = null, bool visible = true)
        {
            Visible = visible;
            Name = name;
            Width = width;
            Colors = colors;
        }

        public bool Visible { get; }
        public string Name { get; }
        public int Width { get;  }
        public Colors Colors { get; }

        public Column WithWidth(int width)
        {
            return new Column(Name, width, Colors, Visible);
        }
        public Column WithVisibility(bool visible)
        {
            return new Column(Name, Width, Colors, visible);
        }

    }
}
