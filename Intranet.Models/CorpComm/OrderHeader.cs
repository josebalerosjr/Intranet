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
        public int OrderTotal { get; set; }
        public string TrackingNumber { get; set; }
        public string RequestorEmail { get; set; }
        public string RequestType { get; set; }
        public string PickUpPoints { get; set; }
        public bool? ItemReceipt { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string TransactionId { get; set; }
        public int OrderRating { get; set; }
    }
}