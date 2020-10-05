using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Models
{
    public class BdoPE
    {
        [Key]
        public int Id { get; set; }

        //[DisplayName("Document Date in Document")]
        [DisplayName("Doc. Date")]
        [Column(TypeName = "nvarchar(10)")]
        public string DocDateInDoc { get; set; }

        //[DisplayName("Document type")]
        [DisplayName("Type")]
        [Column(TypeName = "nvarchar(2)")]
        public string DocType { get; set; }

        [DisplayName("CoCd")]
        [MaxLength(4)]
        [MinLength(4)]
        [Column(TypeName = "nvarchar(4)")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Company code must be a numeric")]
        public string CompanyCode { get; set; }

        //[DisplayName("Posting Date in the Document")]
        [DisplayName("Posting Date")]
        [MaxLength(10)]
        [MinLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string PosDateInDoc { get; set; }

        [DisplayName("Period")]
        [Column(TypeName = "nvarchar(2)")]
        public string FiscalPeriod { get; set; }

        [DisplayName("CUR")]
        [Column(TypeName = "nvarchar(5)")]
        public string CurrentKey { get; set; }

        //[DisplayName("Reference Document Number")]
        [DisplayName("Reference")]
        [Column(TypeName = "nvarchar(16)")]
        public string RefDocNum { get; set; }

        //[DisplayName("Document Header Text")]
        [DisplayName("Doc. Header Text")]
        [Column(TypeName = "nvarchar(25)")]
        public string DocHeadT { get; set; }

        //[DisplayName("Posting Key for the Next Line Item")]
        [DisplayName("PstKy")]
        [Column(TypeName = "nvarchar(2)")]
        public string PosKeyInNextLine { get; set; }

        //[DisplayName("Account or Matchcode for the Next Line Item")]
        [DisplayName("Account")]
        [Column(TypeName = "nvarchar(10)")]
        public string AccMatNextLine { get; set; }

        //[DisplayName("Amount in document currency")]
        [DisplayName("Amount")]
        [Column(TypeName = "nvarchar(20)")]
        public string AmountDocCur { get; set; }

        [DisplayName("Value date")]
        [Column(TypeName = "nvarchar(10)")]
        public string ValDate { get; set; }

        //[DisplayName("Assignment number")]
        [DisplayName("Assignment")]
        [Column(TypeName = "nvarchar(18)")]
        public string AssignNum { get; set; }

        //[DisplayName("Item Text")]
        [DisplayName("Text")]
        [Column(TypeName = "nvarchar(50)")]
        public string ItemText { get; set; }

        //[DisplayName("Posting Key for the Next Line Item")]
        [DisplayName("PstKy")]
        [Column(TypeName = "nvarchar(2)")]
        public string PosKeyInNextLine2 { get; set; }

        //[DisplayName("Account or Matchcode for the Next Line Item")]
        [DisplayName("Customer")]
        [Column(TypeName = "nvarchar(10)")]
        public string AccMatNextLine2 { get; set; }

        //[DisplayName("Amount in document currency")]
        [DisplayName("Amount")]
        [Column(TypeName = "nvarchar(20)")]
        public string AmountDocCur2 { get; set; }

        //[DisplayName("Baseline Date for Due Date Calculation")]
        [DisplayName("Bline Date")]
        [Column(TypeName = "nvarchar(10)")]
        public string BaseDateDueCal { get; set; }

        //[DisplayName("Item Text")]
        [DisplayName("Text")]
        [Column(TypeName = "nvarchar(50)")]
        public string ItemText2 { get; set; }

        [DisplayName("CNC Associate")]
        [Column(TypeName = "nvarchar(50)")]
        public string MarketerZ2 { get; set; }

        [DisplayName("isDownloaded")]
        public bool isDownloaded { get; set; }

        [DisplayName("Encoder Name")]
        [Column(TypeName = "nvarchar(100)")]
        public string UserName { get; set; }

        [DisplayName("IP Address")]
        [Column(TypeName = "nvarchar(15)")]
        public string UserIP { get; set; }

        [DisplayName("Date")]
        [Column(TypeName = "nvarchar(10)")]
        public string UserDate { get; set; }
    }
}