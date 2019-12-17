namespace Konsole
{
    public class MenuModel
    {
        public IConsole Window { get; }
        public string Title { get; }
        public int Current { get; }
        public int Height { get; }
        public int Width { get; }
        public MenuItem[] MenuItems { get; }
        public MenuTheme Theme { get; }

        public MenuModel(IConsole window, string title, int current, int height, int width, MenuItem[] menuItems, MenuTheme theme)
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