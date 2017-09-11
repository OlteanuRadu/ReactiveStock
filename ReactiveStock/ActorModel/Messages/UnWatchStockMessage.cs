using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveStock.ActorModel.Messages
{
    public class UnWatchStockMessage
    {
        public string StockSymbol { get; private set; }
        public UnWatchStockMessage(string stockMessage)
        {
            this.StockSymbol = stockMessage;
        }
    }
}
