using System.Collections.Generic;
using System.Text;

namespace Konsole
{
    public interface IPrintAt
    {
        void PrintAt(int x, int y, string format, params object[] args);
        void PrintAt(int x, int y, string text);
        void PrintAt(int x, int y, char c);
    }
}
