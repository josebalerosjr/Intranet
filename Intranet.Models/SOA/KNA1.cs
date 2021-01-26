﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.Models.SOA
{
    public class KNA1
    {
        [Key]
        public int Id { get; set; }
        public string KUNNR { get; set; }
        public string NAME1 { get; set; }
        public string STRAS { get; set; }
        public string ORT01 { get; set; }
        public string PSTLZ { get; set; }

    }
}
