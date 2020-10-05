using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Models
{
    public class ImageCarousel
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [DisplayName("Name")]
        [Column(TypeName = "nvarchar(50)", Order = 1)]
        public string ImageName { get; set; }

        [DisplayName("Link")]
        [Column(TypeName = "nvarchar(255)", Order = 2)]
        public string ImageLink { get; set; }

        [Required]
        [DisplayName("isActive")]
        [Column(Order = 3)]
        public bool isActive { get; set; }

        [DisplayName("Encoder Name")]
        [Column(TypeName = "nvarchar(100)", Order = 4)]
        public string UserName { get; set; }

        [DisplayName("IP Address")]
        [Column(TypeName = "nvarchar(15)", Order = 5)]
        public string UserIP { get; set; }

        [DisplayName("Date")]
        [Column(TypeName = "nvarchar(10)", Order = 6)]
        public string UserDate { get; set; }
    }
}