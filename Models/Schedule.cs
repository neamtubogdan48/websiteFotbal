using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.data.models
{
    public class Schedule
    {
        public int id { get; set; }
        public required string homeTeam { get; set; }
        public required string awayTeam { get; set; }
        public required string stadium { get; set; }
        public DateOnly matchDate { get; set; }
        public TimeOnly matchTime { get; set; }
        public required string result { get; set; }
        [ForeignKey("News")]
        public int newsId { get; set; }
        public News News { get; set; }
    }
}