using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Sponsor
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string? photo { get; set; }
    }
}