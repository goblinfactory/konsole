using Goblinfactory.Konsole.Platform.Windows;

namespace Goblinfactory.Konsole.Platform
{
    public enum Platform
    {
        Windows,
        Unix,
        IOS
    }
    public class PlatformStuff : IPlatformStuff
    {
        private IPlatformStuff _platformStuff;
        public PlatformStuff()
        {
            // detect platform and "wire up? " a provider?
            // hard code to windows for now
            _platformStuff = new WindowsPlatformStuff();
        }
        public void LockResizing(bool allowClose = true, bool allowMinimize = true)
        {
            _platformStuff.LockResizing(allowClose, allowMinimize);
        }
    }
}
