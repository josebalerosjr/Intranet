using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Intranet.Models.CorpComm
{
    public class Request
    {
        public int Id { get; set; }
        public string LoginUser { get; set; }



        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public Status Status { get; set; }


    }
}
