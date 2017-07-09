using NUnit.Framework;

namespace Konsole.Tests.KeyboardTests
{
    //- `KeyWaitFor({c})`  use in simple loops e.g. `while(c.KeyWaitFor('q'))` 
    // default is case insensitive. Also will read the key and dispose of it, and not echo to screen.


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

        [Test]
        public void WhenWaitingForSpecialKeys_raise_key_press_events_while_waiting()
        {
            //var k = new MockKeyboard('a', 'B');
            //var sut = new Keyboard(k);
            //sut.KeyWait('a');
            //sut.KeyWait('B');
        }

    }
}
