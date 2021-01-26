using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Models.CorpComm
{
    public class EmailSend
    {
        public string Template { get; set; }
        public string Subject { get; set; }
        public string Subject2 { get; set; }
        public string OrderId { get; set; }
        public string ShippingDate { get; set; }
        public string PickUpPoints { get; set; }
        public string LoginUser { get; set; }
        public string RequestorEmail { get; set; }
        public string RejectReason { get; set; }
        public string clickhere { get; set; }
        public string items { get; set; }
    }
}
