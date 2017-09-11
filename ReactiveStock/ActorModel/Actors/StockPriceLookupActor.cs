using Akka.Actor;
using ReactiveStock.ActorModel.Messages;
using ReactiveStock.ExternalServices;
using System;

namespace ReactiveStock.ActorModel.Actors
{
    public class StockPriceLookupActor : ReceiveActor
    {
        private readonly IStockPriceServiceGateway _stockPriceServiceGateway;

        public StockPriceLookupActor(IStockPriceServiceGateway stockPriceServiceGateway)
        {
            this._stockPriceServiceGateway = stockPriceServiceGateway;
            this.Receive<RefreshStockPriceMessage>((m) => this.LookupStockPrice(m));
        }

        private void LookupStockPrice(RefreshStockPriceMessage message)
        {
            var latestPrice = _stockPriceServiceGateway.GetLatestPrice(message.StockSymbol);
            Sender.Tell(new UpdatedStockPriceMessage(latestPrice, DateTime.Now));
        }
    }
}
