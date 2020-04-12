﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Models.CorpComm
{
    public class Email
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Email")]
        [RegularExpression(".+\\@.+\\..+",
            ErrorMessage = "Please enter a valid email address")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string EmailAddress { get; set; }

        [Required]
        [DisplayName("User Fullname")]
        [MaxLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string Decription { get; set; }

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