using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TweetBook.Domain
{
    public class RefreshToken
    {
        [Key]
        public String Token { get; set; }

        public string JwtId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool Used { get; set; }

        public bool Invalided { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

    }
} 
