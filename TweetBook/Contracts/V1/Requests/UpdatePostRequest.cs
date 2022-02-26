using System.ComponentModel.DataAnnotations;

namespace TweetBook.Contracts.V1.Requests
{
    public class UpdatePostRequest
    {
        [Required]
        public string Name { get; set; }

    }
}
