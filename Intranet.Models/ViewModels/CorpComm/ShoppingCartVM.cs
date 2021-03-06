﻿using Intranet.Models.CorpComm;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Intranet.Models.ViewModels.CorpComm
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ListCart { get; set; }
        public IEnumerable<OrderHeader> ListOrderHeader { get; set; }
        public IEnumerable<SelectListItem> EventList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public Event Event { get; set; }
        public string SelectedEvent { get; set; }
    }
}