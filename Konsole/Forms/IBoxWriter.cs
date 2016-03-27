namespace Konsole.Forms
{
    public interface IBoxWriter
    {
        string Header(string title);
        string Footer { get; }
        string Line { get; }
        string Write(string text);
    }
}