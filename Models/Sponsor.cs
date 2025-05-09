using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.data.models
{
    public class Sponsor
    {
        public int id { get; set; }
        public required string name { get; set; }
        public required string description { get; set; }
        public required string logo { get; set; }
        [ForeignKey("News")]
        public int newsId { get; set; }
        public News News { get; set; }
    }
}