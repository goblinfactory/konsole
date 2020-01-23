using System;

namespace Konsole
{
    // begin-snippet: IPrintAtColor
    public interface IPrintAtColor : IPrintAt, ISetColors
    {
        void PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background);
    }
    //end-snippet
}
