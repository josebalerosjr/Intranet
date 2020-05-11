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

        [Required]
        public int? CollateralId { get; set; }
        [ForeignKey("CollateralId")]
        public Collateral Collateral { get; set; }

        public int Count { get; set; }
        public int Price { get; set; }
    }
}