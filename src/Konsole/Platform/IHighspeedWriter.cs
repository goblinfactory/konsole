using System;

namespace Konsole.Platform
{
    // Oh dear...this interface cannot be right  oops!! :0 
    public interface IHighspeedWriter : IDisposable
    {
        void ClearScreen();
        char ClearScreenChar { get; set; }

        Colors Colors { get; set; }
        void Flush();

        bool AutoFlush { get; }
    }
}
