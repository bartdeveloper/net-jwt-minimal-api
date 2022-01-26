using JWT_Minimal_API.Models;

namespace JWT_Minimal_API.Services
{
    public interface IUserService
    {
        public User GetUser(UserLogin userLogin);
        string GenerateToken(User loggedInUser);
        User GetUserClaims(HttpContext httpContext);
    }
}
