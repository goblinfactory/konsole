using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Konsole.Tests
{
    //- `KeyWaitFor({c})`  use in simple loops e.g. `while(c.KeyWaitFor('q'))` 
    // default is case insensitive. Also will read the key and dispose of it, and not echo to screen.


    public class KeyWaitForShould
    {
        [Test]
        public void be_case_insensitive()
        {
            IConsole c = new MockConsole(5,1);
            //c.KeyWaitFor('q')
        }

        [Test]
        public void read_key_and_remove_it_from_buffer()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void not_echo_to_screen()
        {
            Assert.Inconclusive();
        }

    }
}
