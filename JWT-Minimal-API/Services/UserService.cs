using JWT_Minimal_API.Models;
using JWT_Minimal_API.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Minimal_API.Services
{
    public class UserService : IUserService
    {

        private readonly IConfiguration _config;

        public UserService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public User GetUser(UserLogin userLogin)
        {
            User user = UserRepository.Users.FirstOrDefault(o => o.Username.Equals(userLogin.Username, StringComparison.OrdinalIgnoreCase) && o.Password.Equals(userLogin.Password));

            return user;
        }

        public string GenerateToken(User loggedInUser)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
                new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
                new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
                new Claim(ClaimTypes.Surname, loggedInUser.Surname),
                new Claim(ClaimTypes.Role, loggedInUser.Role)
            };

            var token = new JwtSecurityToken
            (
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"])), SecurityAlgorithms.HmacSha256),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                notBefore: DateTime.UtcNow
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;

        }

        public User GetUserClaims(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;

            if (identity is not null)
            {

                var userClaims = identity.Claims;

                var user = new User()
                {
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    GivenName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };

                return user;

            }

            return null;

        }
    }
}
