
using ApprovalTests;

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
        public static void ShouldBe(this string[] src, string[] expected)
        {
            var srcArray = src ?? new string[0];
            var expectedArray = expected ?? new string[0];
            var actual = string.Join("\n", srcArray);

            Approvals.AssertText(expectedArray, actual);
        }
    }
}