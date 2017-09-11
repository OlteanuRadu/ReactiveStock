using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveStock.ActorModel.Messages
{
    public class RemoveChartSeriesMessage
    {
        public string StockSymbol { get; private set; }
        public RemoveChartSeriesMessage(string stockMessage)
        {
            this.StockSymbol = stockMessage;
        }
    }
}
