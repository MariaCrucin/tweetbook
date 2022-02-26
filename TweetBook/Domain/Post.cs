using Microsoft.AspNetCore.Identity;

namespace TweetBook.Domain
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? UserId { get; set; } 
        public IdentityUser User { get; set; } 
    }
}
