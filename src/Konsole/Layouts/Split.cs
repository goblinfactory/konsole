using Konsole.Drawing;
using System;

namespace Konsole
{
    public class Split
    {
        public Split()
        {

        }

        public Split(int size)
        {
            Size = size;
        }

        public Split(int size, string title)
        {
            Size = size;
            Title = title;
            Thickness = LineThickNess.Single;
        }

        public Split(int size, ConsoleColor foreground)
        {
            Size = size;
            Foreground = foreground;
        }

        public Split(int size, string title, LineThickNess thickness)
        {
            Size = size;
            Title = title;
            Thickness = thickness;
        }

        public Split(int size, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            Size = size;
            Title = title;
            Thickness = thickness;
            Foreground = foreground;
        }

        public int Size { get; set; } = 3;
        public string Title { get; set; } = null;
        public LineThickNess? Thickness { get; set; } = null;
        public ConsoleColor? Foreground { get; set; } = null;
        public ConsoleColor? Background { get; set; } = null;
    }
}
