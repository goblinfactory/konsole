namespace Konsole
{
// begin-snippet: IWindowed
    public interface IWindowed
    {
        /// <summary>
        /// The absolute X position this window is located on the real or root console. This is where relative x:0 starts from for this window.
        /// </summary>
        int AbsoluteX { get; }

        /// <summary>
        /// The absolute Y position this window is located on the real or root console. This is where relative y:0 starts from for this window.
        /// </summary>
        int AbsoluteY { get; }

        int WindowWidth { get; }
        int WindowHeight { get; }
    }
    //end-snippet
}   