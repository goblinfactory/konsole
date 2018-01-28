using ApprovalTests;

namespace Konsole.Tests
{
    public static class ApprovalExtensions
    {
        private static bool _configured = false;

        static ApprovalExtensions()
        {
            if (!_configured)
            {
                ApprovalUtilities.SimpleLogger.Logger.Show(false, false, false, false, false, false);
                _configured = true;
            }
        }

        public static void Verify(this string[] source)
        {
            Approvals.VerifyAll("", source, s=> s);
        }
    }
}