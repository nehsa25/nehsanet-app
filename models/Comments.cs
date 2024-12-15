using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nehsanet_app.Models
{
    [Table("Comments")]
    public class DBComment
    {
        [Key]
        [Required]
        public int? CommentID { get; set; }

        [Required]
        public string Username { get; set; } = "";

        [Required]
        public string Comment { get; set; } = "";

        [Required]
        [ForeignKey("id")]
        public string PageID { get; set; } = "";

        [Required]
        public string IP { get; set; } = "";

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        public Page Page { get; set; } = null!;
    }
}
