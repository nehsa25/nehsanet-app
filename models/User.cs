using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nehsanet_app.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? UserID { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 0)]
        public string FirstName { get; set; } = "";

        [Required]
        [StringLength(255, MinimumLength = 0)]
        public string LastName { get; set; } = "";

        [Required]
        [StringLength(255, MinimumLength = 0)]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public int RoleID { get; set; }

        [ForeignKey("RoleID")]
        public Role? Role { get; set; }
    }
}
