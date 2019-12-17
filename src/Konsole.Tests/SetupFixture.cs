using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace Konsole.Tests
{
    [SetUpFixture]
    public class SetupFixture
    {
        [OneTimeSetUp]
        public void RunOnceBeforeAnyTestsInNamespace()
        {
            ApprovalExtensions.RemoveApprovalDebugInfoFromBuildLots();
        }
    }
}
