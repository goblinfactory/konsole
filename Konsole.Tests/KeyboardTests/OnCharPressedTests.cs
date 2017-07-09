using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Konsole.Tests.KeyboardTests
{
    public class OnCharPressedTests
    {
        [Test]
        public void only_chars_registered_to_receive_events_should_recieve_events()
        {
            var k = new MockKeyboard('c', 'B', 'a', 'd', 'o','g','t', 'q');
            var seq = new List<char>();
            var sut = new Keyboard(k);
            sut.OnCharPressed(new[] {'c', 't', 'a'}, c => seq.Add(c));

            sut.WaitForKeyPress('q');
            Assert.AreEqual("cat", new string(seq.ToArray()));
        }


        [Test]
        public void only_char_registered_to_receive_events_should_recieve_events()
        {
            var k = new MockKeyboard('c', 'B','c', 'a', 'd', 'o', 'g', 't', 'q');
            var seq = new List<char>();
            var sut = new Keyboard(k);
            sut.OnCharPressed('c', c => seq.Add(c));

            sut.WaitForKeyPress('q');
            Assert.AreEqual("cc", new string(seq.ToArray()));
        }

    }
}
