using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nehsanet_app.Models
{
    [Table("UserEvent")]
    public class UserEvent
    {
        [Key]
        [Column(Order = 0)]
        public int UserID { get; set; }

        [Column(Order = 1)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(45, MinimumLength = 0)]
        public string IP { get; set; } = "";

        [ForeignKey("UserID")]
        public User? User { get; set; }
    }
}
