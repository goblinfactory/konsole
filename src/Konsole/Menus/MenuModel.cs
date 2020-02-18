namespace Konsole
{
    public class MenuModel
    {
        public MenuLine Separator { get; }

        public IConsole Window { get; }
        public string Title { get; }
        public int Current { get; }
        public int Height { get; }
        public int Width { get; }
        public MenuItem[] MenuItems { get; }
        public Style Style { get; }

        internal bool Naked
        {
            get { return Separator == MenuLine.naked; }
        }

        public MenuModel(IConsole window, string title, int current, int height, int width, MenuLine separator, MenuItem[] menuItems, Style style)
        {           
            Window = window;
            Title = title;
            Current = current;
            Height = height;
            Width = width;
            Separator = separator;
            MenuItems = menuItems;
            Style = style;
        }
    }
}