using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nehsanet_app.Models
{
    [Table("Log")]
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }

        [Required]
        public int Log_LogLevelID { get; set; }

        [Required]
        [StringLength(10000, MinimumLength = 0)]
        public string Message { get; set; } = "";

        [Required]
        [StringLength(128, MinimumLength = 0)]
        public string IP { get; set; } = "";

        [Required]
        public long SessionID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public LogLevel? LogLevel { get; set; }
    }
}