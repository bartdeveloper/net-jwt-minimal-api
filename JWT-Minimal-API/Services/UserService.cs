using JWT_Minimal_API.Models;
using JWT_Minimal_API.Repositories;
using System.Security.Claims;

namespace JWT_Minimal_API.Services
{
    public class UserService : IUserService
    {
        public User Get(UserLogin userLogin)
        {
            User user = UserRepository.Users.FirstOrDefault(o => o.Username.Equals(userLogin.Username, StringComparison.OrdinalIgnoreCase) && o.Password.Equals(userLogin.Password));

            return user;
        }

    }
}
