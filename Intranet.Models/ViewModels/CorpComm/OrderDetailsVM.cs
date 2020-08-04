using Intranet.Models.CorpComm;
using System.Collections.Generic;

namespace Intranet.Models.ViewModels.CorpComm
{
    public class OrderDetailsVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}