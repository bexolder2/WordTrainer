using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WordTrainer.Models.Enums;

namespace WordTrainer.Models.DbModels
{
    [Table("Words")]
    public class Word
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NativeWord { get; set; }
        public string TranslatedWord { get; set; }
        public WordStatus Status { get; set; } 
    }
}
