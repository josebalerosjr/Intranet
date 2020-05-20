using Intranet.Models.CorpComm;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Models.ViewModels.CorpComm
{
    public class StationVM
    {
        public Station Station { get; set; }
        public IEnumerable<SelectListItem> StationTypeList { get; set; }
    }
}
