using ApprovalTests;
using Konsole.Forms;
using Konsole.Tests.TestClasses;
using NUnit.Framework;

namespace Konsole.Tests.FormTests
{
    public class EditShould
    {
        //NOT YET IMPLEMENTED, FUTURE FUNCTIONALITY
        public void when_opening_first_field_should_be_highlighted()
        {
            System.Console.WriteLine("given a console form for a person");
            // ---------------------------------------------------
            var console = new Window(200, 20);
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
