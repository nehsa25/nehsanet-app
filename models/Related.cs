using System.ComponentModel.DataAnnotations.Schema;

namespace nehsanet_app.Models
{
    [Table("related_pages")]
    public class RelatedPage
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("page_id")]
        public int PageId { get; set; }
        public Page Page { get; set; } = null!;
        [Column("related_page_id")]
        public int RelatedPageId { get; set; }
        public Page RelatedPageNavigation { get; set; } = null!;
    }
}