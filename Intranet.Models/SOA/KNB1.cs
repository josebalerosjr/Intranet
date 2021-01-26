using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.Models.SOA
{
    public class KNB1
    {
        [Key]
        public int Id { get; set; }
        public string KUNNR { get; set; }
        public string BUKRS { get; set; }
        public string KVERM { get; set; }

    }
}
