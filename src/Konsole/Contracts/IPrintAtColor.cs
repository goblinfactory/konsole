using System;

namespace Konsole
{
    public interface IPrintAtColor : IPrintAt, ISetColors
    {
        void PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background);
    }
}
