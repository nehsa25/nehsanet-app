using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nehsanet_app.Models
{
     [Table("related_pages")]
    public class DBRelatedPage()
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public int page_id { get; set; }

        [Required]
        public string related_page_id { get; set; } = "";

        public DBPage Page { get; set; } = null!;
        public DBPage RelatedPage { get; set; } = null!;
    }
}