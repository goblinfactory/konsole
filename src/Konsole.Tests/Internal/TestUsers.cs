using System.Linq;

namespace Konsole.Tests.Internal
{
    internal class User
    {
        public User(string name, string iD, int credits)
        {
            Name = name;
            ID = iD;
            Credits = credits;
        }

        public string Name { get; set; }
        public string ID { get; set; }
        public int Credits { get; set; }
    }

    internal static class TestUsers
    {
        public static User[] CreateUsers(int cnt)
        {
            return new User[]
            {
                    new User("Graham", "GRH01", 100),
                    new User("Kendall", "KEN01", 250),
                    new User("Michael", "MIK01", 55),
                    new User("Susan", "SUS01", 77)
            }.Take(cnt).ToArray();
        }

    }
}
