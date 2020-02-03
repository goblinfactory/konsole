namespace Konsole
{
    public class WriteResult
    {
        internal WriteResult(string written, string overflow, bool atLastChar)
        {
            Written = written;
            Overflow = overflow;
            AtLastChar = atLastChar;
        }
        public string Written { get; }
        public string Overflow { get; }
        public bool AtLastChar { get; }
    }
}