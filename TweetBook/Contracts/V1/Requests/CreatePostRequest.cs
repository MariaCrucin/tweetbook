using System.ComponentModel.DataAnnotations;

namespace TweetBook.Contracts.V1.Requests
{
    public class CreatePostRequest
    { 
        [Required]
        public string Name { get; set; }
    }
}
