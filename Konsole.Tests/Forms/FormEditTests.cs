using System;
using ApprovalTests;
using Konsole.Forms;
using Konsole.Internal;
using Konsole.Tests.TestClasses;

namespace Konsole.Tests.Forms
{
    public class FormEditTests
    {
        // not yet implemented
        public void when_opening_first_field_should_be_highlighted()
        {
            System.Console.WriteLine("given a console form for a person");
            // ---------------------------------------------------
            var console = new BufferedWriter(200, 20);
            var form = new Form(console);
            var person = new Person
            {
                FirstName = "First name",
                LastName = "last name",
                AFieldWithAMuchLongerName = "long field",
                Address = new Address()
                {
                    Line1 = "line 1",
                    Line2 =  "line 2",
                     PostCode = "postcode"
                },
                FavouriteMovie = "fav movie"
            };

            System.Console.WriteLine("When I open the form for editing");
            // ---------------------------------------------------
            form.Edit(person);

            System.Console.WriteLine("then the form should be rendered");
            // ---------------------------------------------------
            Approvals.Verify(console.BufferWrittenString);
            System.Console.WriteLine(console.BufferWrittenString);

            System.Console.WriteLine("and the first entry field should be highlighted");
            // ------------------------------------------------------------------

        }

    }
}
