using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nehsanet_app.Models
{
    [Table("Role")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RoleID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 0)]
        public string RoleLevel { get; set; } = "";
    }
}
