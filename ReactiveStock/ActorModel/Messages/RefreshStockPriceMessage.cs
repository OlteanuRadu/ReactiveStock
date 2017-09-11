using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveStock.ActorModel.Messages
{

    public class RefreshStockPriceMessage
    {
        public string StockSymbol { get; private set; }
        public RefreshStockPriceMessage(string stockSymbol)
        {
            this.StockSymbol = stockSymbol;
        }
    }
}
