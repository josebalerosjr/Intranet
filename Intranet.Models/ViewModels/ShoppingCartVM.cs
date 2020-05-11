using Intranet.Models.CorpComm;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ListCart { get; set; }
        public IEnumerable<SelectListItem> EventList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public Event Event { get; set; }
    }
}
