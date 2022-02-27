using System.ComponentModel.DataAnnotations;

namespace TweetBook.Contracts.V1.Requests
{
    public class UpinsertFlowerRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
