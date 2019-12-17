using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.Slow
{

    public class WindowConcurrencyTests
    {
        [Test]
        public void WindowsAndSingleBackgroundThread()
        {
            var console = new MockConsole(80,20);
            var w1 = Window.Open(0, 0, 20, 20, "w1", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkBlue, console);
            int max = 80000;
            Task.Run(() =>
            {
                for (int i = 0; i < max; i++) w1.Write(" {0} ", i.ToString());
            }).Wait();

            var expected = new[]
            {
                "┌─────── w1 ───────┐                                                            ",
                "│955  79956  79957 │                                                            ",
                "│79958  79959  7996│                                                            ",
                "│0  79961  79962  7│                                                            ",
                "│9963  79964  79965│                                                            ",
                "│  79966  79967  79│                                                            ",
                "│968  79969  79970 │                                                            ",
                "│79971  79972  7997│                                                            ",
                "│3  79974  79975  7│                                                            ",
                "│9976  79977  79978│                                                            ",
                "│  79979  79980  79│                                                            ",
                "│981  79982  79983 │                                                            ",
                "│79984  79985  7998│                                                            ",
                "│6  79987  79988  7│                                                            ",
                "│9989  79990  79991│                                                            ",
                "│  79992  79993  79│                                                            ",
                "│994  79995  79996 │                                                            ",
                "│79997  79998  7999│                                                            ",
                "│9                 │                                                            ",
                "└──────────────────┘                                                            "
            };

            var actual = console.BufferWritten;
            actual.Should().BeEquivalentTo(actual);
        }

        [Test]
        public void WindowsWithFourBackgroundThreads()
        {
            int max = 8000;
            var console = new MockConsole(80, 20);
            var w1 = Window.Open(0, 0, 20, 20, "w1", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkBlue, console);
            var w2 = Window.Open(20, 0, 20, 20, "w2", LineThickNess.Single, ConsoleColor.Red, ConsoleColor.DarkYellow, console);
            var w3 = Window.Open(40, 0, 20, 20, "w3", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkYellow, console);
            var w4 = Window.Open(60, 0, 20, 20, "w4", LineThickNess.Single, ConsoleColor.Black, ConsoleColor.White, console);
            Task t1 = Task.Run(() =>
            {
                for (int i = 0; i < max; i++) w1.Write(" {0} ", i.ToString());
            });

            Task t2 = Task.Run(() =>
            {
                for (int i = 0; i < max; i++) w2.Write(" {0} ", i.ToString());
            });

            Task t3 = Task.Run(() =>
            {
                for (int i = 0; i < max; i++) w3.Write(" {0} ", i.ToString());
            });

            Task t4 = Task.Run(() =>
            {
                for (int i = 0; i < max; i++) w4.Write(" {0} ", i.ToString());
            });


            Task.WaitAll(new[] {t1, t2, t3, t4});

            var expected = new[]
            {
                "┌─────── w1 ───────┐┌─────── w2 ───────┐┌─────── w3 ───────┐┌─────── w4 ───────┐",
                "│  7948  7949  7950││  7948  7949  7950││  7948  7949  7950││  7948  7949  7950│",
                "│  7951  7952  7953││  7951  7952  7953││  7951  7952  7953││  7951  7952  7953│",
                "│  7954  7955  7956││  7954  7955  7956││  7954  7955  7956││  7954  7955  7956│",
                "│  7957  7958  7959││  7957  7958  7959││  7957  7958  7959││  7957  7958  7959│",
                "│  7960  7961  7962││  7960  7961  7962││  7960  7961  7962││  7960  7961  7962│",
                "│  7963  7964  7965││  7963  7964  7965││  7963  7964  7965││  7963  7964  7965│",
                "│  7966  7967  7968││  7966  7967  7968││  7966  7967  7968││  7966  7967  7968│",
                "│  7969  7970  7971││  7969  7970  7971││  7969  7970  7971││  7969  7970  7971│",
                "│  7972  7973  7974││  7972  7973  7974││  7972  7973  7974││  7972  7973  7974│",
                "│  7975  7976  7977││  7975  7976  7977││  7975  7976  7977││  7975  7976  7977│",
                "│  7978  7979  7980││  7978  7979  7980││  7978  7979  7980││  7978  7979  7980│",
                "│  7981  7982  7983││  7981  7982  7983││  7981  7982  7983││  7981  7982  7983│",
                "│  7984  7985  7986││  7984  7985  7986││  7984  7985  7986││  7984  7985  7986│",
                "│  7987  7988  7989││  7987  7988  7989││  7987  7988  7989││  7987  7988  7989│",
                "│  7990  7991  7992││  7990  7991  7992││  7990  7991  7992││  7990  7991  7992│",
                "│  7993  7994  7995││  7993  7994  7995││  7993  7994  7995││  7993  7994  7995│",
                "│  7996  7997  7998││  7996  7997  7998││  7996  7997  7998││  7996  7997  7998│",
                "│  7999            ││  7999            ││  7999            ││  7999            │",
                "└──────────────────┘└──────────────────┘└──────────────────┘└──────────────────┘"
            };

            var actual = console.BufferWritten;
            actual.Should().BeEquivalentTo(expected);
        }

    }
}
