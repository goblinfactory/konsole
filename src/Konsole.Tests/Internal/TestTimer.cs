using NUnit.Framework;
using System;
using System.Diagnostics;

namespace Konsole.Tests
{
    public static class TestTimer
    {
        private static double _minimumMicroSecondsResolutionSupported;
        
        // above this will assert.inconclusive
        public static double DebugMultiplierInconclusive = 2.0D;

        // above this will fail a unit test
        public static double DebugMultiplierFail = 4.0D;

        private static bool _debug = false;

        static TestTimer()
        {
            SetDebug(ref _debug);
            var ticksPerMicrosecond = ((double)Stopwatch.Frequency) / 1000_000D;
            _minimumMicroSecondsResolutionSupported = 1 / ticksPerMicrosecond;
        }
        public static T RunTest_µs<T>(double microSeconds, Func<T> test, bool warmup = true)
        {
            if (microSeconds < _minimumMicroSecondsResolutionSupported)
            {
                Assert.Inconclusive("Cannot run this test. Hardware not accurate enough. Minimum resolution possible is :{_minimumMicroSecondsResolutionSupported}µs. You requested {microSeconds}µs");
            }
            T t;
            var sw = new Stopwatch();
            if (warmup) test();
            sw.Start();
            t = test();
            sw.Stop();
            var ms = sw.Elapsed.TotalMilliseconds;
            var µs = ms * 1000;
            if(_debug)
                debugCheck_µs(µs, microSeconds);
            else if (µs > microSeconds)
                Failµs(µs, microSeconds);
            return t;
        }

        public static T RunTest_ms<T>(double milliSeconds, Func<T> test, bool warmup = true)
        {
            T t;
            var sw = new Stopwatch();
            if (warmup) test();
            sw.Start();
            t = test();
            sw.Stop();
            var ms = sw.Elapsed.TotalMilliseconds;
            if (_debug)
                debugCheck_ms(ms, milliSeconds);
            else if (ms > milliSeconds) 
                Failms(ms, milliSeconds);
            return t;
        }

        private static void debugCheck_µs(double µsActual, double µsMaxAllowed)
        {
            if (µsActual > µsMaxAllowed * DebugMultiplierFail)
            {
                Failµs(µsActual, µsMaxAllowed * DebugMultiplierFail);
            }
            if (µsActual > µsMaxAllowed * DebugMultiplierInconclusive)
            {
                Inconclusiveµs(µsActual, µsMaxAllowed * DebugMultiplierInconclusive);
            }
        }

        private static void debugCheck_ms(double msActual, double msMaxAllowed)
        {
            if (msActual > msMaxAllowed * DebugMultiplierFail)
            {
                Failms(msActual, msMaxAllowed * DebugMultiplierFail);
            }
            if (msActual > msMaxAllowed * DebugMultiplierInconclusive)
            {
                Inconclusivems(msActual, msMaxAllowed * DebugMultiplierInconclusive);
            }
        }


        private static void Failµs(double µsActual, double µsAllowedMax) 
        {
            Assert.Fail($"Failed Performance test. Took {µsActual:0.00}µs. Expected less than {µsAllowedMax:0.00}µs.");
        }

        private static void Failms(double msActual, double msAllowedMax)
        {
            Assert.Fail($"Failed Performance test. Took {msActual:0.00}ms. Expected less than {msAllowedMax:0.00}ms.");
        }

        private static void Inconclusiveµs(double µsActual, double µsAllowedMax)
        {
            Assert.Inconclusive($"Took {µsActual:0.00}µs. Expected less than {µsAllowedMax:0.00}µs.");
        }

        private static void Inconclusivems(double msActual, double msAllowedMax)
        {
            Assert.Inconclusive($"Took {msActual:0.00}ms. Expected less than {msAllowedMax:0.00}ms.");
        }

        
        [Conditional("DEBUG")]
        private static void SetDebug(ref bool debug)
        {
            debug = true;
        }
    }
}
