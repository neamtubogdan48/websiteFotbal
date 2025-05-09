using Microsoft.AspNetCore.Identity;
using mvc.data.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Users : IdentityUser
    {
        public string accountType { get; set; }
        [ForeignKey("Subscription")]
        public int subscriptionId { get; set; }
        public Subscription? subscription { get; set; }
        public string? photoPath { get; set; }
    }
}
