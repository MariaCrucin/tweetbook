using System.ComponentModel.DataAnnotations;

namespace TweetBook.Contracts.V1.Requests
{
    public class UpsertFlowerRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
