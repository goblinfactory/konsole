using System;
using System.Collections.Generic;
using System.Linq;
using Konsole.Internal;
using static System.ConsoleColor;

namespace Konsole
{
    public enum MenuLine { naked, box, none, top, topAndBottom }

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

        public ConsoleKeyInfo QuitKey { get; }

        private readonly int _width;

        //private Dictionary<int, MenuItem> _menuItems = new Dictionary<int, MenuItem>();
        private Dictionary<int, MenuItem> _menuItems = new Dictionary<int, MenuItem>();

        private Dictionary<ConsoleKeyInfo, int> _keyBindings = new Dictionary<ConsoleKeyInfo, int>();

        /// <summary>
        /// called before a menu item is called.
        /// </summary>
        public Action<MenuItem> OnBeforeMenuItem = (i) => { };

        /// <summary>
        /// called after a menu item has completed.
        /// </summary>
        public Action<MenuItem> OnAfterMenuItem = (i) => { };

        /// <summary>
        /// Called after the menu has run, and the user has selected to exit the menu. 
        /// </summary>
        public Action OnAfterMenu = () => { };

        /// <summary>
        /// after the user has opted to exit the menu, but before AfterMenu.
        /// </summary>
        protected Action OnBeforeExitMenu = () => { };

        /// <summary>
        /// Called before the menu starts running, i.e. at the start of .Run(), and before anything is rendered any of the consoles.
        /// </summary>
        public Action<Menu> OnBeforeMenu = (m) => { };

        public MenuLine Separator { get; set; } = MenuLine.none;

        /// <summary>
        /// Enable to display the shortcut key for the menu
        /// </summary>
        public bool EnableShortCut { get; set; } = true;

        public Style Style { get; set; } = Style
            .WhiteOnDarkBlue
            .WithThickness(LineThickNess.Single)
            .WithTitle(new Colors(Yellow, Red));
        public string Title { get; set; } = "";


        private int _current = 0;

        public int Current
        {
            get { return _current; }
        }

        private int _top = 0;
        private int _height;

        public int NumMenus { get; }
        public int Height => _height;
        public bool CaseSensitive { get; }

        public IKeyboard Keyboard { get; set; }

        private bool Naked
        {
            get
            {
                return Separator == MenuLine.naked;
            }
        }

        public Menu(string title, ConsoleKey quit, int width, params MenuItem[] menuActions)
            : this(new Writer(), title, quit, width, MenuLine.none, menuActions)
        {

        }

        public Menu(string title, ConsoleKey quit, int width, MenuLine separator, params MenuItem[] menuActions)
            : this(new Writer(), title, quit, width, separator, menuActions)
        {

        }

        public Menu(IConsole menuConsole, string title, ConsoleKey quit, int width, params MenuItem[] menuActions)
            : this(menuConsole, title, quit, width, MenuLine.none, menuActions)
        {

        }

        /// <summary>
        /// if we have any menu items with menu keys that differ only by case, then this is a case sensitive menu, otherwise the menu items will be case insensitive.
        /// </summary>
        public Menu(IConsole menuConsole, string title, ConsoleKey quit, int width, MenuLine separator, params MenuItem[] menuActions)
        {
            lock (_locker)
            {
                Separator = separator;
                CaseSensitive = CaseForMenuItems(menuActions) == Case.Sensitive;
                Title = title;
                Keyboard = Keyboard ?? new Keyboard();
                _menuConsole = menuConsole;
                QuitKey = quit.ToKeypress();
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
                _height = Naked ? menuActions.Length + 2 : menuActions.Length + 3;
                //new Draw(menuConsole).Box()
                _window = new Window(_menuConsole, new WindowSettings { SX = 2, Width = _width, Height = _height, Theme = Style.ToTheme(), Clipping = true });
            }
        }

        private char SwitchCase(char c)
        {
            var up = char.ToUpper(c);
            return (up == c) ? char.ToLower(c) : up;
        }

        private Case CaseForMenuItems(MenuItem[] menuActions)
        {
            // if there are menu items that only only differ by case, then the menu is case sensitive, otherwise it's case insensitive.
            var menuKeys = menuActions.Where(m => m.Key != null).Select(m => m.Key.Value).ToArray();
            // A + B = not sensitive
            // A + B + a = sensitive
            // foreach key, if there are any other keys with the same letter just with a different case then it's case sensitive

            foreach (var key in menuKeys)
            {
                var rest = menuKeys.Except(new[] {key});
                var flipped = SwitchCase(key.KeyChar);
                // if there are any other keys that match this one with the case switched
                if (rest.Any( k => k == new ConsoleKeyInfo(flipped,k.Key,k.Shift(),k.Alt(),k.Control()))) return Case.Sensitive;
            }
            return Case.Insensitive;
        }

        private IConsole _window;

        public Action<MenuModel, bool> Render = (model, printBorder) => { _refresh(model, printBorder); };
        private bool _printMenu = true;
        public void Refresh()
        {
            lock (_locker)
            {
                var items = _menuItems.Values.ToArray();
                var model = new MenuModel(_window, Title, Current, Height, _width, Separator, items, Style);
                _refresh(model, _printMenu);
                _printMenu = false;
            }
        }

        private static void _refresh(MenuModel model, bool printBorder)
        {
            var selectedColor = model.Style.SelectedItem;
            var shortCutColor = model.Style.Body.Foreground.ToSelectedItemForeground();
            var bodyColor = model.Style.Body;
            var con = model.Window;
            // redraw the bounding box (menu border) nb, check what the default is...x then y? or y then x?
            int cnt = model.MenuItems.Length;
            int left = model.NoHotKeys ? 1 : 4;
            int len = model.Width - (model.NoHotKeys ? 2 : 6);
            if(printBorder)
            {
                PrintTitleAndBorder(model, con);
            }
            for (int i = 0; i < cnt; i++)
            {
                var item = model.MenuItems[i];
                var text = $"{item.Title.FixLeft(len)}";
                int row = model.Naked ? i + 1 : i + 2;

                var keyInfo = item.Key;

                if(keyInfo!=null)
                {
                    con.PrintAt(bodyColor, 1, row, keyInfo.Value.KeyChar);
                    con.PrintAt(bodyColor, 2, row, '.');
                }

                if (i == model.Current)
                {
                    con.PrintAt(selectedColor, left, row, text);
                    if (item.Key != null)
                    {
                        var key = keyInfo.Value;
                        //int sub = text.IndexOfAny(new[] {char.ToLower(key.KeyChar), char.ToUpper(key.KeyChar)});
                        //if (sub != -1)
                        //{
                        //    string shortcut = text.Substring(sub, 1);
                        //    con.PrintAt(selectedColor, left + sub, row, shortcut);
                        //}
                    }
                }
                else
                {
                    con.PrintAt(bodyColor, left, row, text);
                    if (item.Key != null)
                    {
                        var key = keyInfo.Value.KeyChar;
                        int sub = text.IndexOfAny(new[] {char.ToLower(key), char.ToUpper(key)});
                        if (sub != -1)
                        {
                            string shortcut = text.Substring(sub, 1);
                            con.PrintAt(shortCutColor, left + sub, row, shortcut);
                        }
                    }

                }
            }
        }

        private static void PrintTitleAndBorder(MenuModel model, IConsole con)
        {
            if (model.Separator == MenuLine.naked) return;
            con.PrintAt(model.Style.Title, 0, 0, model.Title.FixCenter(model.Width));
            var draw = new Draw(con, model.Style);

            switch (model.Separator)
            {
                case MenuLine.box:
                    draw.Box(0, 1, model.Width - 1, model.Height - 1);
                    break;
                case MenuLine.none:
                    break;
                case MenuLine.top:
                    draw.Line(0, 1, model.Width - 1, 1);
                    break;
                case MenuLine.topAndBottom:
                    draw.Line(0, 1, model.Width - 1, 1);
                    draw.Line(0, model.Height - 1, model.Width - 1, model.Height - 1);
                    break;
            }


        }

        private MenuItem this[int i]
        {
            get { return _menuItems.ElementAt(i).Value; }
        }


        public Action<Exception, Window> OnError = (e, w) =>
        {
            w.PrintAt(new Colors(ConsoleColor.White, ConsoleColor.Red), 0, 0, $"Error :{e.Message}");
        };


        public virtual void Run()
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
                OnBeforeExitMenu();
                OnAfterMenu();

                lock (_locker)
                {
                    _menuConsole.State = state;
                }
            }
        }

        private void _run()
        {
            ConsoleState state;
            ConsoleKeyInfo cmd;

            lock (_locker)
            {
                _menuConsole.CursorVisible = false;
                state = _menuConsole.State;
                OnBeforeMenu(this);
            }
            Refresh();

            while (!IsMatching(cmd = Keyboard.ReadKey(true), QuitKey))
            {
                int move = isMoveMenuKey(cmd);
                if (move != 0)
                {
                    MoveSelection(move);
                    Refresh();
                    continue;
                }

                if (cmd.Key == ConsoleKey.Enter)
                {
                    var currentItem = _menuItems[Current];
                    if (!currentItem.Enabled) continue;
                    if(currentItem.Action == null || IsQuit(currentItem.Key)) throw new ExitMenu();
                    RunItem(state, currentItem);
                    continue;
                }
                if (!_keyBindings.ContainsKey(cmd)) continue;

                var itemKey = _keyBindings[cmd];
                var item = _menuItems[itemKey];
                // setting a menu item to null is equivalent to exit.
                if (item == null) return;
                SetSelected(cmd);

                // bypass running the menu item by setting it to disabled.
                if (item.Enabled)
                {
                    if(item.Action == null) throw new ExitMenu();
                    RunItem(state, item);
                }
            }


        }

        private bool IsQuit(ConsoleKeyInfo? key)
        {
            if (key == null) return false;
            if (!CaseSensitive) return key.Value == QuitKey;
            return key.Value == QuitKey && key.Value.KeyChar == QuitKey.KeyChar;
        }

        private bool IsMatching(ConsoleKeyInfo key1, ConsoleKeyInfo key2)
        {
            if (CaseSensitive) return key1 == key2;
            // for case insensitive match, just compare KeyInfo
            return key1.Key == key2.Key;
        }

        private void SetSelected(ConsoleKeyInfo key)
        {
            for (int i = 0; i < _menuItems.Count; i++)
            {
                if (_menuItems[i].Key == null) return;
                var k = _menuItems[i].Key.Value;

                if (CaseSensitive)
                {
                    if (key.Key == k.Key && k.KeyChar == key.KeyChar)
                    {
                        _current = i;
                        Refresh();
                        return;
                    }
                }
                else // not case sensitive
                {
                    if (key == k) 
                    {
                        _current = i;
                        Refresh();
                        return;
                    }
                }
            }
        }

        protected virtual void RunItem(ConsoleState state, MenuItem item)
        {
            lock (item.locker)
            {
                try
                {
                    if (item.DisableWhenRunning && item.Running == true) return;
                    item.Running = true;
                    _menuConsole.State = state;
                    OnBeforeMenuItem(item);
                    try
                    {
                        item.Action?.Invoke();
                    }
                    finally
                    {
                        OnAfterMenuItem(item);
                        // if an exception is thrown we need to reset the menu to not active
                        // otherwise the menu item will be blocked permanent in active state
                        item.Running = false;
                    }
                    
                }
                finally
                {
                    _menuConsole.State = state;
                }
            }
        }


        private void MoveSelection(int move)
        {
            _current = (_current + move) % NumMenus;
            if (_current < 0) _current = NumMenus - 1;
        }


        private int isMoveMenuKey(ConsoleKeyInfo cmd)
        {
            switch (cmd.Key)
            {
                case ConsoleKey.RightArrow: return 1;
                case ConsoleKey.LeftArrow: return -1;
                case ConsoleKey.DownArrow: return 1;
                case ConsoleKey.UpArrow: return -1;

                default:
                    return 0;
            }
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

