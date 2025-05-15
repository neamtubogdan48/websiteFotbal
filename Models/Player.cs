using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Player
    {
        public int id { get; set; }
        public string name { get; set; }
        public int number { get; set; }
        public string position { get; set; }
        public string? photo { get; set; }
    }
}