
using Akka.Actor;
using ReactiveStock.ActorModel.Messages;
using System.Collections.Generic;

namespace ReactiveStock.ActorModel.Actors
{
    public class StocksCoordinatorActor :ReceiveActor
    {
        private readonly IActorRef _chartingActor;
        private readonly Dictionary<string, IActorRef> _stockActors;

        public StocksCoordinatorActor(IActorRef chartingActor)
        {
            this._chartingActor = chartingActor;
            this._stockActors = new Dictionary<string, IActorRef>();
            this.Receive<WatchStockMessage>(m => WatchStock(m));
            this.Receive<UnWatchStockMessage>(m => UnWatchStock(m));
        }

        private void WatchStock(WatchStockMessage m)
        {
            var childActorNeedsCreating = !_stockActors.ContainsKey(m.StockSymbol);
            if (childActorNeedsCreating)
            {
                var newChildActor = Context.ActorOf(Props.Create(() => new StockActor(m.StockSymbol)),
                    "StockActor_" + m.StockSymbol);
                _stockActors.Add(m.StockSymbol, newChildActor);
            }
            _chartingActor.Tell(new AddChartSeriesMessage(m.StockSymbol));
            if(_stockActors.TryGetValue(m.StockSymbol,out IActorRef neededActor))
            {
                neededActor.Tell(new SubscribeToNewStockPricesMessage(_chartingActor));
            }
        }
        private void UnWatchStock(UnWatchStockMessage m)
        {
            if (!_stockActors.ContainsKey(m.StockSymbol)) return;

            _chartingActor.Tell(new RemoveChartSeriesMessage(m.StockSymbol));
            if (_stockActors.TryGetValue(m.StockSymbol, out IActorRef neededActor))
            {
                neededActor.Tell(new UnSubscribeFromNewStockPricesMessage(_chartingActor));
            }
        }
    }
}
