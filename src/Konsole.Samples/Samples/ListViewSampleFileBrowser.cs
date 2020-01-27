using System;
using System.IO;
using static System.ConsoleColor;

namespace Konsole.Samples
{
    public static class ListViewSampleFileBrowser
    {


        public static void Main(string[] args)
        {
            var window = new Window();
            var console = window.SplitLeft("left");
            var right = window.SplitRight("right");
            var listView = new DirectoryListView(console, "../../..");

            // let's highlight - all files > 4 Mb and make directories green
            listView.BusinessRuleColors = (o, column) =>
            {
                if (column == 2 && o.Size > 4000000) return new Colors(White, DarkBlue);
                if (column == 1 && o.Item is DirectoryInfo) return new Colors(Green, Black);
                return null;
            };

            listView.Refresh();
        
            
            Console.ReadKey(true);

            //SampleColors.PrintSampleColors(console);
            // list needs to scroll
            // needs to have a current item
            // needs to highlight items
            // needs to react to arrow keys up and down (to start with)

            // window.Keyboard.OnUp(()=> )
            // window.Keyboard.OnDown(()=> );

            // will need to define exit? to stop
            // window.clear to remove list?

        }
    }
}
