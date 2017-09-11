
using Akka.Actor;

namespace ReactiveStock.ActorModel.Messages
{
    public class UnSubscribeFromNewStockPricesMessage
    {
        public IActorRef Subscriber { get; private set; }
        public UnSubscribeFromNewStockPricesMessage(IActorRef unsubscribingActor)
        {
            this.Subscriber = unsubscribingActor;
        }
    }
}
