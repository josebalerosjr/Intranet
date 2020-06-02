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
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        public int SizeId { get; set; }
        [ForeignKey("SizeId")]
        public Size Size { get; set; }

        [Required]
        public int UnitId { get; set; }
        [ForeignKey("UnitId")]
        public Unit Unit { get; set; }

        [Required]
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }

        [Required]
        public int LocationId { get; set; }
        [ForeignKey("LocationId")]
        public Location Location { get; set; }

        [Required]
        [Range(1,10000)]
        public int Count { get; set; }

        [Required]
        [Range(1, 1000000)]
        public int Price { get; set; }
        public string ImgUrl { get; set; }

        [Required]
        [Range(1, 1000000)]
        public int CritCount { get; set; }

        [Required]
        [DisplayName("isActive")]
        public bool isActive { get; set; }

        #region UserDetails

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

        #endregion
    }
}