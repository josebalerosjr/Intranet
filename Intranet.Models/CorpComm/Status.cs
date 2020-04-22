using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Models.CorpComm
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Status")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        #region UserDetails

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

        #endregion
    }
}