namespace Konsole
{
    // begin-snippet: IWrite
    public interface IWrite
    {
        void WriteLine(string format, params object[] args);
        void WriteLine(string text);
        void Write(string format, params object[] args);
        void Write(string text);
        void Clear();
    }
    //end-snippet
}
