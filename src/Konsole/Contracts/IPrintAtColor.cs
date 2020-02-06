using System;

namespace Konsole
{
    // begin-snippet: IPrintAtColor
    public interface IPrintAtColor : IPrintAt, ISetColors
    {
        void PrintAt(Colors colors, int x, int y, string format, params object[] args);
        void PrintAt(Colors colors, int x, int y, string text);
        void PrintAt(Colors colors, int x, int y, char c);

        void PrintAt(ConsoleColor color, int x, int y, string format, params object[] args);
        void PrintAt(ConsoleColor color, int x, int y, string text);
        void PrintAt(ConsoleColor color, int x, int y, char c);
    }
    //end-snippet
}
