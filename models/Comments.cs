using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nehsanet_app.models
{
    [Table("Comments")]
    public class DBComment(string username, string comment, string? page, int? commentid, string? ip = null)
    {
        [Key]
        public int? CommentID { get; set; } = commentid;
        public string Username { get; set; } = username;
        public string Comment { get; set; } = comment;
        public string Page { get; set; } = page ?? "";
        public string IP { get; set; } = ip ?? "";
        public DateTime Date { get; set; } = DateTime.Now;
    }
}