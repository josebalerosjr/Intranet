using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Models.CorpComm
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        public string LoginUser { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime ShippingDate { get; set; }

        [Required]
        public DateTime OrderTotal { get; set; }

        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public Status Status { get; set; }

        public int LocationId { get; set; }

        [ForeignKey("LocationId")]
        public Location Location { get; set; }

        public bool? ItemReceipt { get; set; }

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