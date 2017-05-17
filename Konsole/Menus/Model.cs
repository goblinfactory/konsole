namespace Konsole.Menus
{
    public class Model
    {
        public IConsole Window { get; }
        public string Title { get; }
        public int Current { get; }
        public int Height { get; }
        public int Width { get; }
        public MenuItem[] MenuItems { get; }
        public Theme Theme { get; }

        public Model(IConsole window, string title, int current, int height, int width, MenuItem[] menuItems, Theme theme)
        {
            Window = window;
            Title = title;
            Current = current;
            Height = height;
            Width = width;
            MenuItems = menuItems;
            Theme = theme;
        }
    }
}