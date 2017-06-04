using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Konsole.Tests.MenuTests
{
    public class SelectingAMenuItemShould
    {

        [TestCase(true)]
        [TestCase(false)]
        [Test]
        public void only_run_the_item_when_overlapping_tasks_allowed(bool bisableWhenRunning)
        {
            
        }
    }

}
