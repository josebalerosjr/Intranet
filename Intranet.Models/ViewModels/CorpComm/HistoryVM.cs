using Intranet.Models.CorpComm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Models.ViewModels.CorpComm
{
    public class HistoryVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
        public IEnumerable<Collateral> Collaterals { get; set; }
    }
}
