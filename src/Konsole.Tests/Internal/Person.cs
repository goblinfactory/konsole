namespace Konsole.Tests.TestClasses
{
    internal class Person
    {
        public Person()
        {
            Address = new Address();
        }

        public static string StarSign = "LEO";

        public static decimal Height
        {
            get { return 10.5M; }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AFieldWithAMuchLongerName { get; set; }
        public Address Address { get; set; }
        public string FavouriteMovie { get; set; }
    }
}