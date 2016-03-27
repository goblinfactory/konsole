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

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AFieldWithAMuchLongerName { get; set; }
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

        //public void long_text_fields_should_be_rendered_as_multi_line_textboxes()
        //{
        //    more than (x) lines should be trimmed with ellipses.    
        //}
    }   
}
