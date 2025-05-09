using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.data.models
{
    public class Player
    {
        public int id { get; set; }
        public required string name { get; set; }
        public int number { get; set; }
        public required string position { get; set; }
        public required string photo { get; set; }
        [ForeignKey("News")]
        public int newsId { get; set; }
        public News News { get; set; }
    }
}