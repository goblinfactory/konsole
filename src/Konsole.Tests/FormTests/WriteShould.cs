using FluentAssertions;
using Konsole.Forms;
using Konsole.Tests.TestClasses;
using NUnit.Framework;

namespace Konsole.Tests.FormTests
{
    public class WriteShould
    {
        internal class Cat
        {
            public Cat(int age, string breed, string name) 
            { 
                Breed = breed; 
                Age = age; 
                Name = name; 
            }
            public static string StarSign = "LEO";
            public int Age;
            public string Breed { get; set; }
            private string Breeder { get; set; } = "private";
            public string Name;
            public int NumberOfKittens = 3;
        }

        [Test]
        public void print_public_properties_then_fields()
        {
            var console = new MockConsole(80, 20);
            var form = new Form(console);
            var cat = new Cat(10, "Tabby", "Fred");
            console.WriteLine("line1");
            form.Write(cat);
            console.WriteLine("line2");
            var expected = new[]
            {
                "line1",
                " ┌──────────────────────────────────── Cat ────────────────────────────────────┐",
                " │ Breed             : Tabby                                                   │",
                " │ Age               : 10                                                      │",
                " │ Name              : Fred                                                    │",
                " │ Number Of Kittens : 3                                                       │",
                " └─────────────────────────────────────────────────────────────────────────────┘",
                "line2"
            };

            console.BufferWrittenTrimmed.Should().BeEquivalentTo(expected);
        }
        [Test]
        public void show_the_form_inline_at_the_next_line_below_current_cursor_position_and_update_cursor()
        {
            var console = new MockConsole(80, 20);
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
            form.Write(person);
            console.WriteLine("line2");
            var expected = new[]
            {
                "line1",
                " ┌────────────────────────────────── Person  ──────────────────────────────────┐",
                " │ First Name                      : Freddy                                    │",
                " │ Last Name                       : Astair                                    │",                                                                                                                       
                " │ A Field With A Much Longer Name : 22 apples                                 │",                                                                                                                       
                " │ Favourite Movie                 : Night of the Day of the Dawn of the Son...│",                                                                                                                       
                " └─────────────────────────────────────────────────────────────────────────────┘",                                                                                                                       
                "line2"
            };

            console.BufferWrittenTrimmed.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void render_null_as_NULL()
        {
            var console = new MockConsole(80, 20);
            var form = new Form(console);
            Person p = null;
            console.WriteLine("line1");
            form.Write(p);
            console.WriteLine("line2");
            var expected = new[]
            {
                "line1",
                " ┌────────────────────────────────── Person  ──────────────────────────────────┐",
                " │ Null                                                                        │",
                " └─────────────────────────────────────────────────────────────────────────────┘",
                "line2"
            };

            console.BufferWrittenTrimmed.Should().BeEquivalentTo(expected);

        }

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
        public void support_Numeric_types_both_nullable_and_non_nullable()
        {
            var console = new MockConsole(200, 20);
            var form = new Form(console, 54, new ThinBoxStyle());
            var numclass = new TestClasses.FormTests.MixedNumClass
            {
                DoubleField = double.MaxValue,
                DoubleNull = null,
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
                FloatEpsilon = float.Epsilon,
            };
            form.Write(numclass);
            var expected = new[]
            {
                " ┌────────────────── MixedNumClass  ──────────────────┐",
                " │ Double Field      : 1.7976931348623157E+308        │",
                " │ Double Null       : Null                           │",
                " │ Int Min Value     : 2147483647                     │",
                " │ Int Null          : Null                           │",
                " │ Int Field         : 123                            │",
                " │ Decimal Min Value : -79228162514264337593543950335 │",
                " │ Decimal Max Value : 79228162514264337593543950335  │",
                " │ Decimal Field     : 123.456789                     │",
                " │ Float Field       : 10.1234                        │",
                " │ Float Max Value   : 3.4028235E+38                  │",
                " │ Float Min Value   : -3.4028235E+38                 │",
                " │ Float Null        : Null                           │",
                " │ Float Epsilon     : 1E-45                          │",
                " └────────────────────────────────────────────────────┘"
            };
            console.BufferWrittenTrimmed.Should().BeEquivalentTo(expected);
        }


    }

}