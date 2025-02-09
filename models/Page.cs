using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nehsanet_app.Models
{
    [Table("pages")]
    public class Page
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("stem")]
        public string Stem { get; set; } = "";
        [Column("title")]
        public string Title { get; set; } = "";
        [Column("date")]
        public DateTime Date { get; set; }
        public List<RelatedPage> RelatedPagesNavigations { get; set; } = null!;
        public List<RelatedPage> RelatedPages { get; set; } = null!;
    }
}
