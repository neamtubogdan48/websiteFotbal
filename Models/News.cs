using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.data.models
{
    public class News
    {
        public int id { get; set; }
        public required string title { get; set; }
        public DateOnly publishDate { get; set; }
        public required string photo { get; set; }
    }
}