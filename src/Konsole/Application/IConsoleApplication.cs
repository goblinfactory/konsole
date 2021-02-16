using System;
using System.Threading.Tasks;

namespace Konsole
{
    public interface IConsoleApplication
    {
        IConsoleApplication Parent { get; }
        IConsoleManager Manager { get; }
        bool Enabled { get; set; }
        int? TabOrder { get; set; }
        string Title { get; }
        Guid Id { get; }
    }
}   
