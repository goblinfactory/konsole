using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using Konsole.Forms;
using NUnit.Framework;

namespace Konsole.Tests
{
    public class Address
    {
        public Address()
        {
            Line1 = "";
            Line2 = "";
            PostCode = "";
        }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string PostCode { get; set; }
    }
    public class Person
    {
        public Person()
        {
            Address = new Address();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AFieldWithAMuchLongerName { get; set; }
        public Address Address { get; set; }
        public string FavouriteMovie { get; set; }
    }

    [UseReporter(typeof(DiffReporter))]
    public class FormTests
    {
        [Test]
        public void showing_form_should_show_the_form()
        {
            var console = new TestConsole(200,20);
            var form = new Form(console);
            var person = new Person()
            {
                FirstName = "Freddy",
                LastName = "Astair",
                AFieldWithAMuchLongerName = "22 apples",
                FavouriteMovie = "Night of the Day of the Dawn of the Son of the Bride of the Return of the Revenge of the Terror of the Attack of the Evil, Mutant, Hellbound, Flesh-Eating Subhumanoid Zombified Living Dead, Part 2: In Shocking 2-D"
            };
            form.Show(person);
            Approvals.Verify(console.Buffer);
        }

        // todo; add in test so that form is visible here in the test.
    
        // show dialog, with caption
        // show dialog async!

        //public void nested_objects_should_render_as_sub_boxes()
        //{
            
        //}


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

        //public void progress_bar_print_at_should_position_progress_bar_at_required_position()
        //{
            
        //}

        //public void progress_bar_and_form_objects_should_support_zindex_and_respect_clipped_overlapped_areas_when_rendering()
        //{
            
        //}

        //public enum Button { Ok, OkCancel, YesNo, YesNoCancel }

        //public void async_await_dialog_should_allow_user_to_select_from_list_and_return_option_selected()
        //{
            
        //}

        internal class IntegerOnlyClass
        {
            public int Number { get; set; }
        }

        [TestCase(-100)]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        [Test]
        public void integer_fields_should_render_as_integers(int number)
        {
            var console = new TestConsole(200, 20);
            var form = new Form(console);
            var numclass = new IntegerOnlyClass()
            {
                Number = number
            };
            form.Show(numclass);
            Approvals.Verify(console.Buffer);
        }

        internal class NullableIntegerClass
        {
            public int? Number { get; set; }
        }

        [Test]
        [TestCase(null)]
        [TestCase(1)]
        [TestCase(1245)]
        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        public void nullable_integer_field_should_render_as_blank_when_null_or_use_configured_null_placeholder(int? nullableNumber)
        {
            var console = new TestConsole(200, 20);
            var form = new Form(console);
            var numclass = new NullableIntegerClass()
            {
                Number = nullableNumber
            };
            form.Show(numclass);
            Approvals.Verify(console.Buffer);
        }
    }   
}
