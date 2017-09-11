using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveStock.ExternalServices
{
    public class RandomStockPriceServiceGateway : IStockPriceServiceGateway
    {
        decimal _lastRandomPrice = 20;
        private readonly Random _randm = new Random();
        public decimal GetLatestPrice(string stockSymbol)
        {
            var newPrice = _lastRandomPrice + _randm.Next(-5, 5);
            if (newPrice < 0)
            {
                newPrice = 5;
            }
            else if (newPrice > 50)
            {
                newPrice = 45;
            }
            _lastRandomPrice = newPrice;
            return newPrice;
        }
    }
}
