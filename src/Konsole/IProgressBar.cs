using System;

namespace Konsole
{
    public interface IProgress
    {
        void Refresh(int current);
        int Max { get; set; }
        int Current { get; }

        /// <summary>
        /// Current Y top position in the console.
        /// </summary>
        int Y { get; }
    }

    // This is currently a bit backwards, but to do this properly will require a complete new major version for konsole, so this is temporary.
    public interface IProgressBar : IProgress
    {


        /// <summary>
        /// Returns first line text, or "" for ProgressBarNoText
        /// </summary>
        string Line1 { get; }

        /// <summary>
        /// In Multiline progress bar this is the resulting rendered text line, e.g. "MyFiles.zip : ( 50%) ########". Returns "" in NoText progress bar.
        /// </summary>
        string Line2 { get; }


        /// <summary>
        /// Calling refesh without any text will use current Text value, i.e. the last text value used.
        /// </summary>
        /// <param name="current"></param>
        void Refresh(int current, string item);
        void Refresh(int current, string format, params object[] args);

        void Next(string item);

        /// <summary>
        /// This is the item that the progressbar represents, e.g, "MyFiles.zip" Returns last text value used or "" if the progressbar is NoText. Setting this text value will refresh the progressbar. 
        /// </summary>
        string Item { get; set; }
    }
}