using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmailScheduler.Model
{
    public class User
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}