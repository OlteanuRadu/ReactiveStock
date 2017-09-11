using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveStock.ActorModel.Messages
{
    public class WatchStockMessage
    {
        public string StockSymbol { get; private set; }
        public WatchStockMessage(string stockMessage)
        {
            this.StockSymbol = stockMessage;    
        }
    }
}
