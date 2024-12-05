using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nehsanet_app.Models
{
    [Table("Names")]
    public class DBName
    {
        [Key]
        public int NameID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int SpiritAnimalID { get; set; }
        public DBAnimal? Animal { get; set; }
    }
}