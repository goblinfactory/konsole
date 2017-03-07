using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole.Forms;

namespace Konsole.Sample.Demos
{
    public class FormDemos
    {
        public static void Run()
        {
            var form1 = new Form(80, new ThickBoxStyle());
            var person = new Person()
            {
                FirstName = "Fred",
                LastName = "Astair",
                FieldWithLongerName = "22 apples",
                FavouriteMovie = "Night of the Day of the Dawn of the Son of the Bride of the Return of the Revenge of the Terror of the Attack of the Evil, Mutant, Hellbound, Flesh-Eating Subhumanoid Zombified Living Dead, Part 2: In Shocking 2-D"
            };
            form1.Show(person);

            // works with anonymous types
            new Form().Show(new { Height = "40px", Width = "200px" }, "Demo Box");

            // change the box style, and width, thickbox
            new Form(40, new ThickBoxStyle()).Show(new { AddUser = "true", CloseAccount = "false", OpenAccount = "true" }, "Permissions");
        }

    }

    internal class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FieldWithLongerName { get; set; }
        public string FavouriteMovie { get; set; }
    }

}
