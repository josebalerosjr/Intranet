using Intranet.Models.CorpComm;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Intranet.Models.ViewModels.CorpComm
{
    public class CollateralVM
    {
        public Collateral Collateral { get; set; }
        public IEnumerable<SelectListItem> SizeList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> BrandList { get; set; }
        public IEnumerable<SelectListItem> LocationList { get; set; }
    }
}