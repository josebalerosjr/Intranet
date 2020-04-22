using Intranet.Models.CorpComm;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Models.ViewModels
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
