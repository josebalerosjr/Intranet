﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Models
{
    public class InvLocation
    {
        [Key]
        public int LocId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Location")]
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