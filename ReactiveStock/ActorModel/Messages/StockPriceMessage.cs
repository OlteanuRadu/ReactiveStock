using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveStock.ActorModel.Messages
{
    public class StockPriceMessage
    {
        public string StockSymbol { get; private set; }
        public decimal StockPrice { get; private set; }
        public DateTime Date { get; private set; }

        public StockPriceMessage(string stockSymbol,decimal stockPrice, DateTime date)
        {
            this.StockPrice = stockPrice;
            this.StockSymbol = stockSymbol;
            this.Date = date;
        }
    }
}
