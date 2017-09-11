using Akka.Actor;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ReactiveStock.ActorModel;
using ReactiveStock.ActorModel.Messages;
using ReactiveStock.ActorModel.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReactiveStock.ViewModel
{
    public class StockToggleButtonViewModel : ViewModelBase
    {
        private string _buttonText;
        public string StockSymbol { get; set; }
        public ICommand ToggleCommand { get; set; }
        public IActorRef StockToggleButtonActorRef { get; private set; }

        public string ButtonText
        {
            get { return this._buttonText; }
            set { this.Set(() => ButtonText, ref this._buttonText, value); }
        }

        public StockToggleButtonViewModel(IActorRef stocksCoordinatorRef,string stockSymbol)
        {
            this.StockSymbol = stockSymbol;
            this.StockToggleButtonActorRef =
                ActorSystemReference
                             .ActorSystem
                             .ActorOf(Props.Create(() =>
                                        new StockToggleButtonActor(stocksCoordinatorRef, this, stockSymbol)));

            this.ToggleCommand = new RelayCommand(() =>
                               this.StockToggleButtonActorRef.Tell(new FlipToggleMessage()));

            UpdateButtonTextToOff();
        }

        public void UpdateButtonTextToOff()
        {
            this.ButtonText = ConstructButtonText(false);
        }
        public void UpdateButtonTextToOn()
        {
            this.ButtonText = ConstructButtonText(true);
        }

        public string ConstructButtonText(bool isToggledOn)
        {
            return $"{this.StockSymbol}{(isToggledOn ? "$(on)" : $"(off)")}";
        }
    }
}
