using JWT_Minimal_API.Models;

namespace JWT_Minimal_API.Repositories
{
    public class UserRepository
    {
        public static List<User> Users = new()
        {
            new() { Username = "admin1", EmailAddress = "test.admin@mail.com", Password = "Pa$$w0rd", GivenName = "Bart", Surname = "Adminer", Role = "Administrator" },
            new() { Username = "normal1", EmailAddress = "test.normal@mail.com", Password = "Pa$$w0rd", GivenName = "Normie", Surname = "Smith", Role = "Normal" },
        };
    }
}
