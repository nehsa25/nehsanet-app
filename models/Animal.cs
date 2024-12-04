using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.models
{
    [Table("Animal")]
    public class DBAnimal
    {
        [Key]
        public int ID { get; set; }
        public int AnimalID { get; set; }
        public string AnimalName { get; set; } = "";
    }
}
