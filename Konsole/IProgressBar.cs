namespace Konsole
{
    public interface IProgressBar
    {
        // current Y top position in the console
        int Y { get; }
        string Line1 { get; }
        string Line2 { get; }
        int Max { get; set; }
        void Refresh(int current, string item);
        void Refresh(int current, string format, params object[] args);
        void Next(string item);
    }
}