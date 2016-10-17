namespace Konsole.Tests.TestClasses
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
}