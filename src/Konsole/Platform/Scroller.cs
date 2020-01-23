using Konsole.Platform.Windows;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Konsole.Platform.Windows.Tests")]

namespace Konsole.Platform
{
    internal class Scroller
    {
        enum Direction
        {
            Up,
            UpRight,
            Right,
            RightDown,
            Down,
            DownLeft,
            Left,
            LeftUp
        }


        private readonly CharAndColor[] _buffer;
        private readonly int _bufferWidth;
        private readonly int _bufferHeight;
        private readonly Colors _colors;
        private CharAndColor _fillChar;

        // write up specs (tests) to follow these notes
        // https://docs.microsoft.com/en-us/dotnet/api/system.console.movebufferarea?view=netframework-4.8
        // by implementing this using a source and destination
        // I'll be able to create a scrollable text area that can display a subsection (visible region) of a larger area.
        // for now, all I need is one scroll direction, that is panDown aka scrollUp. (scroll the contents UP but move the viewport DOWN. )
        // if we're literally scrolling an expanding window, when we print to the bottom we extend the height by 1 row, and move the viewport down 1 row.
        // if we limit the maximum bytes allowed to be used then we clip from the top and show @clipped@ or "clipmessage" when we scroll back the other way.
        // if Highspeed logging , you can quickly hot 100 000 rows , which would be 20MB to 30MB, of buffer. So clipping is essential
        // to prevent memory exhaustion for a server monitoring applicaion, or a high speed logging utility.
        // do I want to use a linkedList of Rows for the buffer, so that I can easily expand (and clip) the buffer without performance problems?

        public Scroller(CharAndColor[] buffer, int bufferWidth, int bufferHeight, char emptySpaceChar, Colors colors)
        {
            _buffer = buffer;
            _bufferWidth = bufferWidth;
            _bufferHeight = bufferHeight;
            _colors = colors;
            _fillChar = _colors.Set(emptySpaceChar);
        }

        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
        {
            int xOffset = targetLeft - sourceLeft;
            int yOffset = targetTop - sourceTop;

            var direction = GetDirection(xOffset, yOffset);

            switch (direction)
            {
                case Direction.Down:
                    ScrollDown(-yOffset, sourceLeft, sourceTop, sourceWidth, sourceHeight);
                    break;
                default:
                    throw new NotSupportedException("No other direction other than scrolling Down is currently supported. Please wait for next major release.");
            }
        }

        public void ScrollDown(int numberOfLinesToScroll, int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight)
        {
            // TODO Add in boundary checking!
            int n = numberOfLinesToScroll;
            int startY = sourceTop;
            int endY = sourceTop + sourceHeight - numberOfLinesToScroll;
            int startX = sourceLeft;
            int endX = sourceLeft + sourceWidth;

            // move all the lines that should be moved
            for (int y = startY; y <= endY; y++)
            {
                for(int x = startX; x< endX; x++)
                {
                    int source = (y + n) * _bufferWidth + x;
                    int dest = y  * _bufferWidth + x;
                    _buffer[dest] = _buffer[source];
                }
            }

            // fill in the blacks for the number of lines exposed at the bottom
            for (int y = endY + 1 ; y <= endY + numberOfLinesToScroll; y++)
                {
                    for (int x = startX; x < endX; x++)
                    {
                        int offset = y * _bufferWidth  + x;
                        _buffer[offset] = _fillChar;
                    }
                }


        }

        private Direction GetDirection(int xOffset, int yOffset)
        {
            // for now we only support Scoll DOWN used when printing off the edge of the screen
            // later will add more functionality
            if (xOffset == 0 && yOffset < 0) return Direction.Down;
            throw new NotSupportedException("No other direction other than scrolling Down is currently supported. Please wait for next major release.");
        }

    }
}
