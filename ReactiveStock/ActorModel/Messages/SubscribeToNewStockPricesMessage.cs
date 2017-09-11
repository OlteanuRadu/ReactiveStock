using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveStock.ActorModel.Messages
{
    class SubscribeToNewStockPricesMessage
    {
        public IActorRef Subscriber { get; private set; }
        public SubscribeToNewStockPricesMessage(IActorRef subscribingActor)
        {
            this.Subscriber = subscribingActor;
        }
    }
}
