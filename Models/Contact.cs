using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Contact
    {
        public int id { get; set; }
        public string message { get; set; }
        public DateTime timeSent { get; set; } = DateTime.UtcNow;

        [ForeignKey("Users")]
        public string userId { get; set; }
        public Users? user { get; set; }
    }
}
