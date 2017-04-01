using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Konsole.Drawing;

namespace Konsole.Menus
{
    public class Theme
    {
        public ConsoleColor Background { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor Border { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor Foreground { get; set; } = ConsoleColor.Gray;
        public ConsoleColor SelectedItemBackground { get; set; } = ConsoleColor.Gray;
        public ConsoleColor SelectedItemForeground { get; set; } = ConsoleColor.DarkBlue;
    }

    public class Model
    {
        public Window Window { get; }
        public string Title { get; }
        public int Current { get; }
        public int Height { get; }
        public int Width { get; }
        public MenuItem[] MenuItems { get; }
        public Theme Theme { get; }

        public Model(Window window, string title, int current, int height, int width, MenuItem[] menuItems, Theme theme)
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

    public class Menu
    {



        private readonly IConsole _console;
        private readonly IConsole _output;
        private readonly char _quit;
        private readonly int _width;

        //private Dictionary<int, MenuItem> _menuItems = new Dictionary<int, MenuItem>();
        private Dictionary<int, MenuItem> _menuItems = new Dictionary<int, MenuItem>();

        private Dictionary<char,int> _keyBindings = new Dictionary<char, int>();

        // hooks for clearing screen and redrawing menu, if required
        public Action BeforeMenu = () => { };
        public Action AfterMenu = () => { };

        /// <summary>
        /// Whether pressing enter once a menu item has been highlighted will run that menu item.
        /// </summary>
        public bool PressEnterToSelect { get; } = true;

        /// <summary>
        /// Enable to display the shortcut key for the menu
        /// </summary>
        public bool EnableShortCut { get; set; } = true;
        public Theme Theme { get; set;  } = new Theme();
        public string Title { get; set; } = "";

        private int _current = 0;

        public int Current
        {
            get { return _current; }
        }

        private int _top = 0;
        private int _height;

        public int NumMenus { get; }
        public int Height { get; }

        private static object _locker = new object();

        public IReadKey Keyboard { get; set; }

        public Menu(IConsole console, string title, char quit, int width, params MenuItem[] menuActions)
            : this(console, null,title, quit, width, menuActions)
        {
            
        }

        public Menu(IConsole console, IConsole output, string title, char quit, int width, params MenuItem[] menuActions)
        {
            Title = title;
            Keyboard  = Keyboard ?? new KeyReader();
            _console = console;
            _output = output;
            _quit = quit;
            _width = width;
            NumMenus = menuActions.Length;
            for(int i = 0; i<menuActions.Length; i++)
            {
                var item = menuActions[i];
                var key = item.Key;
                if(key.HasValue) _keyBindings.Add(key.Value, i);
                _menuItems.Add(i, item);

            }
            if (menuActions == null || menuActions.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(menuActions), "Must provide at least one menu action");
            _height = menuActions.Length + 4;
            _window = Window.OpenInline(_console, 2, _width, _height, Theme.Foreground, Theme.Background, K.Clipping);
            _console.CursorTop += _height;
        }

        private Window _window;

        public Action<Model> Render = (model) => { _refresh(model); };

        public void Refresh()
        {
            var items = _menuItems.Values.ToArray();
            var model = new Model( _window, Title, Current, Height, _width, items, Theme);
            _refresh(model);
        }

        private static void _refresh(Model model)
        {
            var con = model.Window;
            int cnt = model.MenuItems.Length;
            int left = 2;
            lock (_locker)
            {
                int len = model.Width - 4;
                con.PrintAtColor(model.Theme.Foreground, 2, 1, model.Title.FixLeft(len), model.Theme.Background);
                con.PrintAtColor(model.Theme.Foreground, 2, 2, new string('-',len), model.Theme.Background);
                for (int i = 0; i < cnt; i++)
                {
                    var item = model.MenuItems[i];
                    var text = item.Title.FixLeft(len);

                    if (i == model.Current)
                    {
                        con.PrintAtColor(model.Theme.SelectedItemForeground, left, i+3, text, model.Theme.SelectedItemBackground);
                    }
                    else
                    {
                        con.PrintAtColor(model.Theme.Foreground, left, i+3, text, model.Theme.Background);
                    }
                }
            }
        }

        private MenuItem this[int i]
        {
            get { return _menuItems.ElementAt(i).Value; }
        }


        public Action<Exception, Window> OnError = (e, w) =>
        {
            w.PrintAtColor(ConsoleColor.White, 0, 0, $"Error :{e.Message}", ConsoleColor.Red);
        };

        public void Run()
        {
            ConsoleState state = _console.State;
            try
            {
                _run();
            }
            finally
            {
                _console.State = state;
            }
        }

        private void _run()
        {
            _console.CursorVisible = false;
            var state = _console.State;
            ConsoleKeyInfo cmd;
            
            Refresh();

            while ((cmd = Keyboard.ReadKey()).KeyChar != _quit)
            {
                int move = isMoveMenuKey(cmd.Key);
                if (move!=0)
                {
                    MoveSelection(move);
                    Refresh();
                    continue;
                }
                if (!_keyBindings.ContainsKey(cmd.KeyChar)) continue;
                var itemKey = _keyBindings[cmd.KeyChar];
                var item = _menuItems[itemKey];

                try
                {
                    _console.State = state;
                    BeforeMenu();
                    item.Action(_output);
                }
                finally
                {
                    AfterMenu();
                    _console.State = state;
                }
            }

        }

        private void MoveSelection(int move)
        {
            _current= (_current + move) % _height;
            if (_current < 0) _current = _height-1;
        }


        private int isMoveMenuKey(ConsoleKey cmd)
        {
            switch (cmd)
            {
                case ConsoleKey.RightArrow: return 1;
                case ConsoleKey.LeftArrow: return -1;
                case ConsoleKey.DownArrow: return 1;
                case ConsoleKey.UpArrow: return -1;

                default:
                    return 0;
            }
        }


        private static int menuCount = 1;

    
        }
    }

