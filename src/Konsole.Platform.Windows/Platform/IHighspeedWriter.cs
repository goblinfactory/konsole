using System;

namespace Konsole.Platform
{

    public interface IHighspeedWriter : IDisposable
    {
        void ClearScreen();
        char ClearScreenChar { get; set; }

        Colors Colors { get; set; }
        void Flush();

        bool AutoFlush { get; }
    }
}
