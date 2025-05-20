using System.ComponentModel.DataAnnotations.Schema;

namespace nehsanet_app.Models
{
    [Table("contactus")]
    public class ContactUs
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = "";

        [Column("email")]
        public string Email { get; set; } = "";

        [Column("message")]
        public string Message { get; set; } = "";
    }
}
