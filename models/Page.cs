using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nehsanet_app.Models
{
    [Table("page")]
    public class DBPage()
    {
        [Key]
        [Required]
        public int? id { get; set; }

        [Required]
        public string stem { get; set; } = "";

        [Required]        
        public string title { get; set; } = "";

        [Required]
        public DateTime date { get; set; } 

        public List<DBRelatedPage> RelatedPages { get; set; } = new();
    }
}
