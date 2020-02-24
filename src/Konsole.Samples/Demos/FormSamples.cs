using Konsole.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole.Samples
{
    public static class FormSamples
    {
        public static void Demo()
        {
            var w = new Window();
            var form = new Form(w);
            form.Write(new { Name = "cat" }, "animal");

        }
    }
}
