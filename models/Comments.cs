using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace nehsanet_app.Models
{
    [Table("Comment")]
    public class DBComment
    {
        [Key]
        public int? CommentID { get; set; }

        [Required]
        public string Username { get; set; } = "";

        [Required]
        public string Comment { get; set; } = "";

        [ForeignKey("id")]
        public int PageID { get; set; }

        [Required]
        [NotMapped]
        public string Stem { get; set; } = "";

        public string IP { get; set; } = "";
        public DateTime DateUTC { get; set; } = DateTime.Now;

        [JsonIgnore]
        [ValidateNever]
        public Page PageNavigation { get; set; } = null!;
    }
}
