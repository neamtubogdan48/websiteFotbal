using System.ComponentModel.DataAnnotations;

namespace mvc.Models
{
    public class Subscription
    {
        public int id { get; set; }
        public required string type { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime expireDate { get; set; }
    }
}