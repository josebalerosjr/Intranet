using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Models.CorpComm
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public OrderHeader OrderHeader { get; set; }

        public int? CollateralId { get; set; }

        [ForeignKey("CollateralId")]
        public Collateral Collateral { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }

        #region UserDetails

        [Required]
        [Display(Name = "Encoder Name")]
        [Column(TypeName = "nvarchar(32)")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "IP Address")]
        [Column(TypeName = "nvarchar(15)")]
        public string UserIP { get; set; }

        [Required]
        [Display(Name = "Date")]
        [Column(TypeName = "nvarchar(10)")]
        public string UserDate { get; set; }

        #endregion UserDetails
    }
}