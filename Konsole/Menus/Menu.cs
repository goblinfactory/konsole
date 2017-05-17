using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Konsole.Drawing;
using Konsole.Internal;

namespace Konsole.Menus
{
    // throw at any time to exit the menu.

    // Requirements 
    // - menu should run inline
    // - when finished, should continue below from where the menu was running.
    // - allows you do something small, reserve a small portion of the screen, e.g. user input
    // - and then continue without having to 'clear' the screen.
    // - can optionally, 'clear' the menu screen portio and continue as if the menu had never happened.
    // - great for popping up a question in a first time developer (attended) vs un-attended build!

    public class Menu
    {
        // locker is static here, because menu makes wrapped calls to Console.XX which is static, even though this class is not!
        private static object _locker = new object();

        private readonly IConsole _menuConsole;
        private readonly IConsole _output;
        private readonly ConsoleKey _quit;
        private readonly int _width;

        //private Dictionary<int, MenuItem> _menuItems = new Dictionary<int, MenuItem>();
        private Dictionary<int, MenuItem> _menuItems = new Dictionary<int, MenuItem>();


        private Dictionary<ConsoleKey, int> _keyBindings = new Dictionary<ConsoleKey, int>();

        /// <summary>
        /// called before a menu item is called.
        /// </summary>
        public Action<MenuItem> BeforeMenuItem = (i) => { };
        
        /// <summary>
        /// called after a menu item has completed.
        /// </summary>
        public Action<MenuItem> AfterMenuItem = (i) => { };

        /// <summary>
        /// Called after the menu has run, and the user has selected to exit the menu. 
        /// </summary>
        public Action<Menu> AfterMenu = (m) => { };

        /// <summary>
        /// Called before the menu starts running, i.e. at the start of .Run(), and before anything is rendered any of the consoles.
        /// </summary>
        public Action<Menu> BeforeMenu = (m) => { };

        /// <summary>
        /// Enable to display the shortcut key for the menu
        /// </summary>
        public bool EnableShortCut { get; set; } = true;
        public Theme Theme { get; set; } = new Theme();
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

        public IReadKey Keyboard { get; set; }


        public Menu(string title, ConsoleKey quit, int width,params MenuItem[] menuActions)  : this(new Writer(), new Writer(), title, quit, width, menuActions)
        {

        }


        public Menu(IConsole output, string title, ConsoleKey quit, int width,params MenuItem[] menuActions) 
            :this(new Writer(), output, title,quit, width,menuActions)
        {
            
        }

        public Menu(IConsole menuConsole, IConsole output, string title, ConsoleKey quit, int width, params MenuItem[] menuActions)
        {
            lock (_locker)
            {
                Title = title;
                Keyboard = Keyboard ?? new KeyReader();
                _menuConsole = menuConsole;
                _output = output;
                _quit = quit;
                _width = width;
                NumMenus = menuActions.Length;
                for (int i = 0; i < menuActions.Length; i++)
                {
                    var item = menuActions[i];
                    var key = item.Key;
                    if (key.HasValue) _keyBindings.Add(key.Value, i);
                    _menuItems.Add(i, item);

                }
                if (menuActions == null || menuActions.Length == 0)
                    throw new ArgumentOutOfRangeException(nameof(menuActions), "Must provide at least one menu action");
                _height = menuActions.Length + 4;
                _window = Window.OpenInline(_menuConsole, 2, _width, _height, Theme.Foreground, Theme.Background, K.Clipping);
            }
        }

        private IConsole _window;

        public Action<Model> Render = (model) => { _refresh(model); };

        public void Refresh()
        {
            lock (_locker)
            {
                var items = _menuItems.Values.ToArray();
                var model = new Model(_window, Title, Current, Height, _width, items, Theme);
                _refresh(model);
            }
        }

        private static void _refresh(Model model)
        {
            var con = model.Window;
            int cnt = model.MenuItems.Length;
            int left = 2;
                int len = model.Width - 4;
                con.PrintAtColor(model.Theme.Foreground, 2, 1, model.Title.FixLeft(len), model.Theme.Background);
                con.PrintAtColor(model.Theme.Foreground, 2, 2, new string('-', len), model.Theme.Background);
                for (int i = 0; i < cnt; i++)
                {
                    var item = model.MenuItems[i];
                    var text = item.Title.FixLeft(len);


                    if (i == model.Current)
                    {
                        con.PrintAtColor(model.Theme.SelectedItemForeground, left, i + 3, text, model.Theme.SelectedItemBackground);
                        if (item.Key != null)
                        {
                            var key = item.Key.Value;

                            int sub = text.IndexOfAny(new[] { char.ToLower((char)key), char.ToUpper((char)key) });
                            if (sub != -1)
                            {
                                string shortcut = text.Substring(sub, 1);
                                con.PrintAtColor(model.Theme.ShortcutKeyHiliteSelected, left + sub, i + 3, shortcut, model.Theme.SelectedItemBackground);
                            }
                        }

                    }
                    else
                    {
                        con.PrintAtColor(model.Theme.Foreground, left, i + 3, text, model.Theme.Background);
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
            ConsoleState state;
            lock (_locker)
            {
                state = _menuConsole.State;
            }
            try
            {
                _run();
            }
            catch (ExitMenu)
            {

            }
            finally
            {
                lock (_locker)
                {
                    _menuConsole.State = state;
                }
            }
        }

        private void _run()
        {
            ConsoleState state;
            ConsoleKey cmd;

            // #ADH I have a nested locker, parent Run locks as well, is this one needed then? must check...where is the unit test that proves I need this?
            // i.e. if I remove the locker below, what is guaranteed to break and where is the test to prove that? otherwise I'm just guessing...
            lock (_locker)
            {
                _menuConsole.CursorVisible = false;
                state = _menuConsole.State;
                BeforeMenu(this);
            }
            Refresh();

            while ((cmd = Keyboard.ReadKey().Key) != _quit)
            {
                int move = isMoveMenuKey(cmd);
                if (move != 0)
                {
                    MoveSelection(move);
                    Refresh();
                    continue;
                }

                if (cmd == ConsoleKey.Enter)
                {
                    var currentItem = _menuItems[Current];
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
            AfterMenu(this);

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

        protected virtual void RunItem(ConsoleState state, MenuItem item)
        {
            try
            {
                _menuConsole.State = state;
                BeforeMenuItem(item);
                item.Action?.Invoke();
                item.ConsoleAction?.Invoke(_output);
            }
            finally
            {
                AfterMenuItem(item);
                _menuConsole.State = state;
            }
        }


        private void MoveSelection(int move)
        {
            _current = (_current + move) % NumMenus;
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

        public static MenuOutput WithOutput(int height, int menuColumnWidth, string menuTitle, string outputTitle, params MenuItem[] menuItems) 
        {
            var console = new Window();
            console.WriteLine("");
            int menuColumn = menuColumnWidth;
            int menuWidth = menuColumn - 4;
            var width = (Console.WindowWidth - menuColumn) - 1;
            var output = Window.Open(menuColumn, 0, width, height, outputTitle, LineThickNess.Single, ConsoleColor.Gray, ConsoleColor.DarkBlue, console);

            var menu = new Menu(console, output, menuTitle, ConsoleKey.Escape, menuWidth, menuItems);
            return new MenuOutput(menu,output);
        }

    }

    public class MenuOutput 
    {
        public Menu Menu { get; }
        public IConsole Output { get;  }

        public MenuOutput(Menu menu, IConsole output)
        {
            Menu = menu;
            Output = output;
        }
    }
}

