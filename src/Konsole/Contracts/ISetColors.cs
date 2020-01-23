using System;

namespace Konsole
{
    // begin-snippet: ISetColors
    public interface ISetColors 
    {
        ConsoleColor ForegroundColor { get; set; }
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Set the foreground and background color in a single threadsafe way. i.e. locks using a static locker before setting the individual ForegroundColor and BackgroundColor properties. 
        /// </summary>
        /// <remarks>Setting Colors = new Colors(Red, White) must be implemented such that it is the same as having called { ForegroundColor = Red; BackgroundColor = White }</remarks>
        Colors Colors { get; set; }
    }
    //end-snippet
}
