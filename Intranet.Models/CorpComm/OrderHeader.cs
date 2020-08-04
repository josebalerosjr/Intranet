using System;
using System.ComponentModel.DataAnnotations;

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

        public string RequestorEmail { get; set; }
        public string RequestType { get; set; }
        public string PickUpPoints { get; set; }
        public string OrderStatus { get; set; }
        public int OrderRating { get; set; }
        public string EventName { get; set; }
        public string StationEvent { get; set; }
        public DateTime EventDate { get; set; }
        public string? RejectReason { get; set; }
    }
}