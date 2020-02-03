namespace Konsole
{
    // at a later stage can look at convert this to use C#7 Span<Cell>
    public interface IPeek
    {
        // begin-snippet: IPeek

        /// <summary>
        /// returns a copy of the cells
        /// </summary>
        Row Peek(int sx, int sy, int width);

        /// <summary>
        /// returns a copy of the cells
        /// </summary>
        Cell Peek(int sx, int sy);

        /// <summary>
        /// returns a copy of the cells the specified screen region
        /// </summary>
        Row[] Peek(ConsoleRegion region);
        // end-snippet
    }
}
