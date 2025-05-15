using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Schedule
    {
        public int id { get; set; }
        public string homeTeam { get; set; }
        public string awayTeam { get; set; }
        public string stadium { get; set; }
        public DateTime matchDate { get; set; }
        public string result { get; set; }
    }
}