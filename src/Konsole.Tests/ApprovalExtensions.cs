
namespace Konsole.Tests
{
    public static class ApprovalExtensions
    {
        private static bool _configured = false;

        public static void RemoveApprovalDebugInfoFromBuildLots()
        {
            if (!_configured)
            {
                ApprovalUtilities.SimpleLogger.Logger.Show(false, false, false, false, false, false);
                _configured = true;
            }
        }
    }
}