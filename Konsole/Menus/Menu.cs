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
    // throw at any time to exit the menu.

    public class Theme
    {
        public ConsoleColor Background { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor Border { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor Foreground { get; set; } = ConsoleColor.Gray;
        public ConsoleColor ShortcutKeyHilite { get; set; } = ConsoleColor.White;
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
        private readonly ConsoleKey _quit;
        private readonly int _width;

        //private Dictionary<int, MenuItem> _menuItems = new Dictionary<int, MenuItem>();
        private Dictionary<int, MenuItem> _menuItems = new Dictionary<int, MenuItem>();

        private Dictionary<ConsoleKey,int> _keyBindings = new Dictionary<ConsoleKey, int>();

        // hooks for clearing screen and redrawing menu, if required
        public Action BeforeMenu = () => { };
        public Action AfterMenu = () => { };

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

        public Menu(string title, ConsoleKey quit, int width, params MenuItem[] menuActions) : this(new Writer(), null, title, quit, width, menuActions)
        {

        }

        public Menu(IConsole console, string title, ConsoleKey quit, int width, params MenuItem[] menuActions)
            : this(console, null,title, quit, width, menuActions)
        {
            
        }

        public Menu(IConsole console, IConsole output, string title, ConsoleKey quit, int width, params MenuItem[] menuActions)
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
                        if (item.Key != null)
                        {
                            var key = item.Key.Value;
                            
                            int sub = text.IndexOfAny(new[] { char.ToLower((char)key), char.ToUpper((char)key) });
                            if (sub != -1)
                            {
                                string shortcut = text.Substring(sub, 1);
                                con.PrintAtColor(model.Theme.ShortcutKeyHilite, left + sub, i + 3, shortcut, model.Theme.SelectedItemBackground);
                            }
                        }
                        
                    }
                    else
                    {
                        con.PrintAtColor(model.Theme.Foreground, left, i+3, text, model.Theme.Background);
                        if (item.Key != null)
                        {
                            var key = item.Key.Value;
                            
                            int sub = text.IndexOfAny(new[] { char.ToLower((char)key), char.ToUpper((char)key) });
                            if (sub != -1)
                            {
                                string shortcut = text.Substring(sub, 1);
                                con.PrintAtColor(model.Theme.ShortcutKeyHilite, left + sub, i + 3, shortcut, model.Theme.Background);
                            }
                        }

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
            catch (ExitMenu)
            {
                
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
            ConsoleKey cmd;
            
            Refresh();

            while ((cmd = Keyboard.ReadKey().Key) != _quit)
            {
                int move = isMoveMenuKey(cmd);
                if (move!=0)
                {
                    MoveSelection(move);
                    Refresh();
                    continue;
                }

                if (cmd == ConsoleKey.Enter)
                {
                    var currentItem  = _menuItems[Current];
                    if (currentItem?.Key == _quit) throw new ExitMenu();
                    RunItem(state, currentItem);
                    continue;
                }
                if (!_keyBindings.ContainsKey(cmd)) continue;

                var itemKey = _keyBindings[cmd];
                var item = _menuItems[itemKey];
                // setting a menu item to null is equivalent to exit.
                if (item == null) return;
                SetSelected(cmd);
                RunItem(state, item);
            }

        }

        private void SetSelected(ConsoleKey key)
        {
            for (int i = 0; i < _menuItems.Count; i++)
            {
                if (key.SameAs(_menuItems[i].Key))
                {
                    _current = i;
                    Refresh();
                    return;
                }
            }
        }

        private void RunItem(ConsoleState state, MenuItem item)
        {
            try
            {
                _console.State = state;
                BeforeMenu();
                item.Action?.Invoke(_output);
            }
            finally
            {
                AfterMenu();
                _console.State = state;
            }
        }


        private void MoveSelection(int move)
        {
            _current= (_current + move) % NumMenus;
            if (_current < 0) _current = NumMenus - 1;
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

