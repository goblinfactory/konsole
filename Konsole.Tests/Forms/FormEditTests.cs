using System;
using ApprovalTests;
using Konsole.Forms;
using Konsole.Testing;
using Konsole.Tests.TestClasses;

namespace Konsole.Tests.Forms
{
    public class FormEditTests
    {
        public void when_opening_first_field_should_be_highlighted()
        {
            Console.WriteLine("given a console form for a person");
            // ---------------------------------------------------
            var console = new TestConsole(200, 20);
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

            Console.WriteLine("When I open the form for editing");
            // ---------------------------------------------------
            form.Edit(person);

            Console.WriteLine("then the form should be rendered");
            // ---------------------------------------------------
            Approvals.Verify(console.Buffer);
            Console.WriteLine(console.Buffer);

            Console.WriteLine("and the first entry field should be highlighted");
            // render console and prefix all chars with (space) for normal text, and ░ (light shade utf char) U+2591
            
        }

    }
}
