namespace Konsole.Tests.TestClasses
{
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
}