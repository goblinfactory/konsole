using Konsole.Samples;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ConsoleColor;

namespace Konsole.Tests.RunOne
{

    class Program
    {
        // consider using ControlSample! there are the start of some automation tests.
        // TODO: experiment with control composition, e.g. CharBoxes for password, with navigation being "captured" by control until finished with exit key, e.g. enter or tab.
        // All "windows" will become keyHandlers and will delgate jeyHandling "down", the reverse of how print commands delegated UP the compositional hierarchy.
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            var test = Window.OpenBox("Test Chinese", 20, 12);
            test.WriteLine("这是一个中文句子测试，是否可以自动换行不越界");

            Console.ReadLine();
            return;

            Console.CursorVisible = false;
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(20, 5);
            Console.SetWindowSize(20, 5);

            var window = new Window();
            var (l, r) = window.SplitLeftRight("left", "right");
            l.Write("line1\nline2\nline3\nline4");
            Console.ReadKey(true);
            return;


            var (top, bot) = window.SplitTopBottom("top", "bottom");
            var (left, right) = bot.SplitLeftRight("left", "right");
            var (veg, fruit) = right.SplitTopBottom("vegetables", "fruit");

            veg.WriteLine("potatoe\ncarrots\nbeans");
            fruit.WriteLine("oranges\nbananas\ntomatoes");
            // disable the bottom right for demo purposes.
            // all controls and text inside the right window
            // will be rendered in Disabled styling.
            right.Enabled = false;




            //await window.RunAsync();

            // at this point we should be able to print the window layout...
            // ask each window recursively to dump .. their configuration?
            // window should report it's child windows as well by call
            PrintWindowTitles(top, window, 0, 0, 0);
            top.WriteLine(Yellow, "Press enter to quit");
            Console.ReadLine();
        }

        // turn into treeview control.
        private static void PrintWindowTitles(IConsole output, IConsole window, int row, int total, int indent) {
            var tc = TreeChars(indent, row, total);
            output.WriteLine($"{tc}{(window.Title ?? "[untitled]")}");
            var cons = window.Manager.ConsolesByTabOrder().ToArray();
            int cnt = cons.Length;
            int i = 1;
            foreach (var win in cons)
            {
                var treechars = TreeChars(indent+1, i++, cnt);
                output.WriteLine($"{treechars}{win.Title}");
                var children = win?.Manager?.ConsolesByTabOrder() ?? new IConsole[0];
                int subrow = 1;
                int subIndent = indent + 2;
                foreach (var win2 in children)
                {
                    PrintWindowTitles(output, win2, subrow++, children.Length, subIndent);
                }
            }

        }

        private static string TreeChars(int indent, int row, int cnt)
        {
            if (indent == 0) return "";
            //if (row == 1 && cnt > 1) return $" ├─ ";
            if (row == cnt) return $"{(Repeat(" │ ", (indent-1)))} └─ ";
            return $"{(Repeat(" │ ", (indent - 1)))} ├─ ";
        }

        private static string Repeat(string src, int cnt)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < cnt; i++) sb.Append(src);
            return sb.ToString();
        }
    }
}
