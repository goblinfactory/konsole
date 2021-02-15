using System.Threading.Tasks;

namespace Konsole
{
    public interface IConsoleApplication : IConsole
    {
        void RemoveConsole(string id);
        void AddConsole(string id, IConsole console);
        int? TabOrder { get; }
        void Refresh(ControlStatus status);
        Task RunAsync();
    }
}   
