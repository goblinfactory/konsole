using System;
using NUnit.Framework;

namespace Konsole.Tests.Helpers
{
    /// <summary>
    /// Helper class to allow you to check a test precondition, and importantly NOT fail the test. This allows us to see the the wood from the trees.
    /// If a test is not a true unit test and is a bit tied to another class, then checking preconditions helps us avoid some classes of false negatives that can result.
    /// </summary>
    public static class Precondition
    {
        public static void Check(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                var message = "Test inconclusive. Precondition failed, most likely due to some other dependancy. Fix other failing tests then re-run this test. Error was :";

                Console.WriteLine("TEST INCONCLUSIVE");
                Console.WriteLine("-----------------");
                Console.WriteLine(message + " " + ex);

                if(System.Diagnostics.Debugger.IsAttached)
                {
                    Assert.Fail(message + " " + ex.Message);}
                else
                {
                    Assert.Inconclusive(message + " " + ex.Message);
                }
                
                
            }
        }
    }
}