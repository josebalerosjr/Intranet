using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Models.CorpComm
{
    public class Collateral
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(32)")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Description")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Brand")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Brand { get; set; }

        [Required]
        [DisplayName("Size")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Size { get; set; }

        [Required]
        [DisplayName("Image")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string ImgLink { get; set; }

        [Required]
        [DisplayName("isActive")]
        public bool isActive { get; set; }

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