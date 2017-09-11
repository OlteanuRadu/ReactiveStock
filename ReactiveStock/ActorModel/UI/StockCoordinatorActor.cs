using Akka.Actor;
using ReactiveStock.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveStock.ActorModel.UI
{
    public class StockCoordinatorActor
    {
        private readonly IActorRef _chartingActorRef;
        public StockCoordinatorActor(IActorRef chartingActorRef)
        {
            this._chartingActorRef = chartingActorRef;
        }
    }
}
