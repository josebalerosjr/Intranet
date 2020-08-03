using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Intranet.Models.CorpComm
{
    public class History
    {
        [Key]
        public int Id { get; set; }

        public int CollateralId { get; set; }

        [DisplayName("Request #")]
        public int? RequestId { get; set; }

        [DisplayName("Request Date")]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [DisplayName("Admin")]
        public string LoginUser { get; set; }

        [DisplayName("Collateral Name")]
        public string CollateralName { get; set; }

        public string EventType { get; set; }
        public int Quantity { get; set; }

        [DisplayName("Station Name / Event Name")]
        public string StationEvent { get; set; }

        public DateTime EventDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public string DropOffPoint { get; set; }
        public int rating { get; set; }
        public string? RejectMarks { get; set; }

        [DisplayName("Adjustment Remarks")]
        public string? ReconRemarks { get; set; }
    }
}