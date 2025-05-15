using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class News
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateOnly publishDate { get; set; }
        public string? photo { get; set; }
    }
}