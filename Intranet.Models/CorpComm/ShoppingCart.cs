using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Models.CorpComm
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }

        [Key]
        public int Id { get; set; }

        public string LoginUser { get; set; }

        public int CollateralId { get; set; }
        [ForeignKey("CollateralId")]
        public Collateral Collateral { get; set; }

        public int? EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }

        [Range(1, 1000)]
        public int Count { get; set; }

        [NotMapped]
        public int Price { get; set; }

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