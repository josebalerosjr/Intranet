using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Models
{
    public class ItemRegSHE
    {
        [Key]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Name")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Column(TypeName = "nvarchar(255)")]
        [DisplayName("Item Desc")]
        public string ItemDesc { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Mfr")]
        public string ManufName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Asset/SN")]
        public string AsstSerial { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        [DisplayName("P/N")]
        public string PartNum { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Column(TypeName = "nvarchar(32)")]
        [DisplayName("Type")]
        public string TypeName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("CAL Date")]
        public DateTime? CalDate { get; set; }

        [Required]
        [DisplayName("Qty")]
        public int Qty { get; set; }

        [Required]
        [DisplayName("Critical Level")]
        public int CritLevel { get; set; }

        [Required]
        [DisplayName("Unit")]
        [Column(TypeName = "nvarchar(32)")]
        public string UnitName { get; set; }

        [DisplayName("REM")]
        [Column(TypeName = "nvarchar(255)")]
        public string Remarks { get; set; }

        [Required]
        [DisplayName("Loc")]
        [Column(TypeName = "nvarchar(32)")]
        public string LocName { get; set; }

        [Required]
        [DisplayName("Encoder Name")]
        [Column(TypeName = "nvarchar(32)")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("IP Address")]
        [Column(TypeName = "nvarchar(15)")]
        public string UserIP { get; set; }

        [Required]
        [DisplayName("Date")]
        [Column(TypeName = "nvarchar(10)")]
        public string UserDate { get; set; }
    }
}