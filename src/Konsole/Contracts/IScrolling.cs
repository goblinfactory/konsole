using System;

namespace Konsole
{
    public interface IScrolling
    {
        void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft,
            int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor);

        void ScrollDown();
    }
}   