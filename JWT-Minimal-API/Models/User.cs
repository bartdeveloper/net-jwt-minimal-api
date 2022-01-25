using System.Text.Json.Serialization;

namespace JWT_Minimal_API.Models
{
    public class User
    {
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
    }
}
