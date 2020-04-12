using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Models
{
    public class Suggestion
    {
        [Key]
        public int SuggId { get; set; }

        [Required]
        [DisplayName("Name")]
        [Column(TypeName = "nvarchar(50)")]
        public string SuggName { get; set; }

        [Required]
        [DisplayName("Email")]
        [Column(TypeName = "nvarchar(50)")]
        public string SuggEmail { get; set; }

        [Required]
        [DisplayName("Subject")]
        [Column(TypeName = "nvarchar(100)")]
        public string SuggSubject { get; set; }

        [Required]
        [DisplayName("Message")]
        [Column(TypeName = "nvarchar(max)")]
        public string SuggMessage { get; set; }

        [Required]
        [DisplayName("Encoder Name")]
        [Column(TypeName = "nvarchar(32)")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("IP Address")]
        [Column(TypeName = "nvarchar(15)")]
        public string UserIP { get; set; }

        [Required]
        [DisplayName("Date")]
        [Column(TypeName = "nvarchar(10)")]
        public string UserDate { get; set; }
    }
}