using Akka.Actor;
using GalaSoft.MvvmLight;
using ReactiveStock.ActorModel.Messages;
using ReactiveStock.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveStock.ActorModel.UI
{
    public class StockToggleButtonActor : ReceiveActor
    {
        private readonly IActorRef _coordinatorActor;
        private readonly StockToggleButtonViewModel _vm;
              private readonly string _stockSymbol;
        public StockToggleButtonActor(IActorRef actor, StockToggleButtonViewModel vm, string stockSymbol)
        {
            this._vm = vm;
            this._coordinatorActor = actor;
            this._stockSymbol = stockSymbol;
            this.ToggledOff();
           
        }

        public void ToggledOff()
        {
            this.Receive<FlipToggleMessage>(m =>
            {
                _coordinatorActor.Tell(new WatchStockMessage(_stockSymbol));
                _vm.UpdateButtonTextToOn();
                this.Become(ToggleOn);
            });
        }
        public void ToggleOn()
        {

            this.Receive<FlipToggleMessage>(m =>
            {
                _coordinatorActor.Tell(new UnWatchStockMessage(_stockSymbol));
                _vm.UpdateButtonTextToOff();
                this.Become(ToggledOff);
            });
        }
    }
}
