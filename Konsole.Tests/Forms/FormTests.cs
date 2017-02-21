using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Maintenance;
using ApprovalTests.Reporters;
using Konsole.Forms;
using Konsole.Testing;
using Konsole.Tests.TestClasses;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Console = Konsole.Testing.Console;

namespace Konsole.Tests
{
    [UseReporter(typeof (DiffReporter))]
    public class FormTests
    {
        [Test]
        public void Show_should_show_the_form_inline_at_the_next_line_below_current_cursor_position()
        {
            var console = new Console(200, 20);
            var form = new Form(console);
            var person = new Person()
            {
                FirstName = "Freddy",
                LastName = "Astair",
                AFieldWithAMuchLongerName = "22 apples",
                FavouriteMovie =
                    "Night of the Day of the Dawn of the Son of the Bride of the Return of the Revenge of the Terror of the Attack of the Evil, Mutant, Hellbound, Flesh-Eating Subhumanoid Zombified Living Dead, Part 2: In Shocking 2-D"
            };
            console.WriteLine("line1");
            form.Show(person);
            console.WriteLine("line2");
            Approvals.Verify(console.Buffer);
        }

        //[Test]
        //public void ShowAt_should_show_the_form_inline_at_the_required_cursor_position_and_not_change_current_cursor_position()
        //{
        //    var console = new TestConsole(100, 20);
        //    var form = new Form(console);
        //    var box= new 
        //    {
        //        Height = 11.4M,
        //        Width = 20.32M
        //    };
        //    console.WriteLine("line1");
        //    form.Show(box, "test");
        //    console.WriteLine("line2");
        //    Approvals.Verify(console.Buffer);
        //}


        // todo; add in test so that form is visible here in the test.

        // show dialog, with caption
        // show dialog async!

        // public void should_support_any_width 
        // e.g. so that we think carefully about clipping
        // if it works at 3 chars, then it will work at 2000!
        // also, will allow us to dynamically 'collapse' forms
        // and have them 'autosize' to fit neatly together
        // when 'composing' forms.

        //public void nested_objects_should_render_as_sub_boxes()
        //{

        //}

        //public void collection_properties_should_render_as_grid()
        //{
            
        //}

        // - renderer to detect cycles maintain a list of already visited objects

        // - limit the recursion to only items in the same assembly 

        //public void fields_should_retain_their_ordinal_position()
        //{

        //}

        //public void long_text_fields_should_be_rendered_as_multi_line_textboxes()
        //{
        //    more than (x) lines should be trimmed with ellipses.    
        //}

        //public void print_at_should_render_the_dialog_at_required_position()
        //{

        //}


        //public enum Button { Ok, OkCancel, YesNo, YesNoCancel }

        //public void async_await_dialog_should_allow_user_to_select_from_list_and_return_option_selected()
        //{

        //}


        [Test]
        public void Numeric_types_both_nullable_and_non_nullable_should_be_supported()
        {
            var console = new Console(200, 20);
            var form = new Form(console);
            var numclass = new TestClasses.FormTests.MixedNumClass
            {
                IntMinValue = int.MaxValue,
                IntNull = null,
                IntField = 123,
                DecimalMinValue = decimal.MinValue,
                DecimalMaxValue = decimal.MaxValue,
                DecimalField = 123.456789M,
                FloatField = 10.1234F,
                FloatMaxValue = float.MaxValue,
                FloatMinValue = float.MinValue,
                FloatNull = null,
                FloatEpsilon = float.Epsilon
            };
            form.Show(numclass);
            Approvals.Verify(console.Buffer);
            System.Console.WriteLine(console.Buffer);
        }


    }

}