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


        [Test]
        public void mutliple_registered_clients_should_all_recieve_events()
        {
            var k = new MockKeyboard('c', 'B', 'c', 'a', 'd', 'o', 'g', 't', 'q');

            var seq1 = new List<char>();
            var seq2 = new List<char>();

            var keyboard = new Keyboard(k);

            keyboard.OnCharPressed(new[] {'c','a','t' }, c => seq1.Add(c));
            keyboard.OnCharPressed(new[] { 'd', 'o', 'g' }, c => seq2.Add(c));

            keyboard.WaitForKeyPress('q');
            Assert.AreEqual("ccat", new string(seq1.ToArray()));
            Assert.AreEqual("dog", new string(seq2.ToArray()));
        }

    }
}
