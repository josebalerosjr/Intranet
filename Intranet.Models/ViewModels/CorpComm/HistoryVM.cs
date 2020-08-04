using Intranet.Models.CorpComm;
using System.Collections.Generic;

namespace Intranet.Models.ViewModels.CorpComm
{
    public class HistoryVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
        public IEnumerable<Collateral> Collaterals { get; set; }
    }
}