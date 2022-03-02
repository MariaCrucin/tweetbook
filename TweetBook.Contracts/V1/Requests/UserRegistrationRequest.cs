using System.ComponentModel.DataAnnotations;

namespace TweetBook.Contracts.V1.Requests
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }    
    }
}
