﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Intranet.Models.SOA
{
    public class T014
    {
        [Key]
        public int Id { get; set; }
        public string KKBER { get; set; }
        public string WAERS { get; set; }
        public string STAFO { get; set; }
        public string PERIV { get; set; }
        public string CTLPC { get; set; }
        public string KLIMK { get; set; }
        public string SBGRP { get; set; }
        public string ALLCC { get; set; }
        public string KKBTX { get; set; }

    }
}
