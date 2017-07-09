using System.Collections.Generic;
using NUnit.Framework;

namespace Konsole.Tests.KeyboardTests
{
    public class WaitForKeyPressTests
    {
        [Test]
        public void WaitForKeyPress_should_be_case_insensitive()
        {
            var k = new MockKeyboard('a', 'B');
            var sut = new Keyboard(k);
            sut.WaitForKeyPress('a');
            sut.WaitForKeyPress('B');
        }

        [Test]
        public void WhenWaitingForSpecialKeys_read_key_and_remove_it_from_buffer()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void WhenWaitingForSpecialKeys_not_echo_to_screen()
        {
            Assert.Inconclusive();
        }
    }
}
