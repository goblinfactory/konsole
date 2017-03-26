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
    public class Menu
    {
        private readonly IConsole _console;
        private readonly IConsole _output;
        private readonly char _quit;
        private readonly IReadKey _keyreader;
        private readonly int _width;
        private Dictionary<char, MenuItem> _menuItems = new Dictionary<char, MenuItem>();


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
        public ConsoleColor Background { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor Foreground { get; set; } = ConsoleColor.White;
        public ConsoleColor SelectedItemBackground { get; set; } = ConsoleColor.White;
        public ConsoleColor SelectedItemForeground { get; set; } = ConsoleColor.Red;
        public ConsoleColor ShortCutKeyForeground { get; set; } = ConsoleColor.Yellow;
        public ConsoleColor ShortCutKeyBackground { get; set; } = ConsoleColor.DarkBlue;

        public LineThickNess LineThickNess { get; set; } = LineThickNess.Single;
        private int _messageHeight = 4;

        public int MessageHeight
        {
            get { return _messageHeight; }
            set
            {
                _messageHeight = value;
                Refresh();
            }
        }


        private static object _locker = new object();

        public IReadKey Keyreader { get; set; }

        public Menu(IConsole console, IConsole output, char quit, int width, params MenuItem[] menuActions)
        {
            _keyreader = new KeyReader();
            _console = console;
            _output = output;
            _quit = quit;
            _width = width;
            foreach (var menu in menuActions)
            {
                _menuItems.Add(menu.Key, menu);
            }
            if (menuActions == null || menuActions.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(menuActions), "Must provide at least one menu action");
            _height = menuActions.Length;
        }

        private int _current = 0;

        public int Current
        {
            get { return _current; }
        }

        private int _top = 0;
        private int _height = 0;

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                Refresh();
            }
        }

        private Window _window;
        private Window _messageWindow;

        public void DrawBorder()
        {
            var ex = _width + 2;
            var ey = _height + 2;

            new Draw(_console)
                .Box(0, 0, ex, ey)
                .Box(0, ey, ex, ey + MessageHeight + 2);
        }

        public void Refresh()
        {
            DrawBorder();
            DrawMenu();
        }

        // todo; move to seperate class : IMenuRender
        private void DrawMenu()
        {
            int left = EnableShortCut ? 4 : 0;
            lock (_locker)
            {
                for (int i = 0; i < Height; i++)
                {
                    var item = this[i];
                    if (EnableShortCut) _window.PrintAtColor(ShortCutKeyForeground, 0, i, $"'{item.Key}' ", ShortCutKeyBackground);
                    
                    if (i == Current)
                    {
                        _window.PrintAtColor(SelectedItemForeground, left, i, item.Title, SelectedItemBackground);
                    }
                    else
                    {
                        _window.PrintAtColor(Foreground, left, i, item.Title, Background);
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

        public string Title { get; set; } = "Menu";


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
            _window = Window.Open(0, 0, _width+2, _height+2, "menu", LineThickNess, Foreground, Background, _console); // need to set these windows to clip!
            //_window = new Window(_console, 1, );

            _window.CursorVisible = false;
            _messageWindow = Window.Open(0, _height + 3, _width + 2, 4, "menu", LineThickNess, Foreground, Background, _console); // need to set this window to clip!
            var state = _console.State;
            ConsoleKeyInfo cmd;
            
            Refresh();

            while ((cmd = _keyreader.ReadKey()).KeyChar != _quit)
            {
                int move = isMoveMenuKey(cmd.Key);
                if (move!=0)
                {
                    MoveSelection(move);
                    DrawMenu();
                    continue;
                }
                if (!_menuItems.ContainsKey(cmd.KeyChar)) continue;
                var item = _menuItems[cmd.KeyChar];

                try
                {
                    _console.State = state;
                    BeforeMenu();
                    item.Action(_output);
                }
                catch (Exception ex)
                {
                    OnError(ex, _messageWindow);
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

