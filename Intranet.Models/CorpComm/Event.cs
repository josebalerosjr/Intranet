﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Models.CorpComm
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Event Name")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Description")]
        [MaxLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string Decription { get; set; }

        [Required]
        [DisplayName("Price Limit")]
        public int PriceLimit { get; set; }

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