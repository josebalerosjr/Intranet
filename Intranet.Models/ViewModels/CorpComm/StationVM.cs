using Intranet.Models.CorpComm;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Intranet.Models.ViewModels.CorpComm
{
    public class StationVM
    {
        public Station Station { get; set; }
        public IEnumerable<SelectListItem> StationTypeList { get; set; }
    }
}