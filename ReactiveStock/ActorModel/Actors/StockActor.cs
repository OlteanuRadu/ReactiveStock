using Akka.Actor;
using Akka.DI.Core;
using ReactiveStock.ActorModel.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactiveStock.ActorModel.Actors
{
    public class StockActor : ReceiveActor
    {
        private readonly string _stockSymbol;
        private readonly HashSet<IActorRef> _subscribers;
        private readonly IActorRef _priceLookupChild;
        private decimal _stockPrice;
        private ICancelable _priceRefreshing;
        public StockActor(string stockSymbol)
        {
            this._stockSymbol = stockSymbol;
            this._subscribers = new HashSet<IActorRef>();
            this._priceLookupChild = Context.ActorOf(Context.DI().Props<StockPriceLookupActor>());

            this.Receive<SubscribeToNewStockPricesMessage>((m) => _subscribers.Add(m.Subscriber));
            this.Receive<UnSubscribeFromNewStockPricesMessage>((m) => _subscribers.Remove(m.Subscriber));
            this.Receive<RefreshStockPriceMessage>((m) => _priceLookupChild.Tell(m));
            this.Receive<UpdatedStockPriceMessage>(m =>
            {
                _stockPrice = m.Price;
                var stockPriceMessage = new StockPriceMessage(_stockSymbol, _stockPrice, m.Date);
                _subscribers.ToList().ForEach(_ => _.Tell(stockPriceMessage));
            });
        }
        protected override void PreStart()
        {
            this._priceRefreshing = Context
                                     .System
                                     .Scheduler
                                     .ScheduleTellRepeatedlyCancelable(
                                            TimeSpan.FromSeconds(1),
                                            TimeSpan.FromSeconds(1),
                                            Self,
                                            new RefreshStockPriceMessage(_stockSymbol),
                                            Self);
        }

        protected override void PostStop()
        {
            _priceRefreshing.Cancel(false);
            base.PostStop();
        }
    }
}
