using System.ComponentModel.DataAnnotations;

namespace Intranet.Models
{
    public class PaymentEntry
    {
        [Key]
        public int EntryId { get; set; }

        public double Amount { get; set; }
        public string Bank { get; set; }
    }
}