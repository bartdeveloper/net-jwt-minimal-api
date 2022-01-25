using JWT_Minimal_API.Models;

namespace JWT_Minimal_API.Services
{
    public interface IUserService
    {
        public User Get(UserLogin userLogin);
    }
}
