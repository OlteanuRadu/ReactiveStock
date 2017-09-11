using Akka.Actor;
using GalaSoft.MvvmLight;
using OxyPlot;
using OxyPlot.Axes;
using ReactiveStock.ActorModel;
using ReactiveStock.ActorModel.Actors;
using ReactiveStock.ActorModel.UI;
using System.Collections.Generic;

namespace ReactiveStock.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {

        private IActorRef _chartingActorRef;
        private IActorRef _stocksCoordinatorActorRef;
        private PlotModel _plotModel;
        public Dictionary<string,StockToggleButtonViewModel> StockButtonViewModels { get; set; }

        public PlotModel PlotModel
        {
            get { return this._plotModel; }
            set { this.Set(() => PlotModel, ref this._plotModel, value); }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainWindowViewModel()
        {
            this.SetUpChartModel();
            this.InitializeActors();
            this.CreateStockButtonViewModels();
        }

        public void SetUpChartModel()
        {
            _plotModel = new PlotModel {
                LegendTitle = "Legend",
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopRight,
                LegendBackground = OxyColor.FromAColor(200,OxyColors.White),
                LegendBorder = OxyColors.Black
            };
            var stockDateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Date",
                StringFormat = "HH:mm:ss"
            };
            _plotModel.Axes.Add(stockDateTimeAxis);

            var stockPriceAxis = new LinearAxis {
                Minimum =0,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title ="Price"
            };

            _plotModel.Axes.Add(stockPriceAxis);
        }
        public void InitializeActors()
        {
            this._chartingActorRef =
                    ActorSystemReference.ActorSystem.ActorOf(Props.Create(() => 
                    new LineChartingActor(PlotModel)));

            _stocksCoordinatorActorRef =
                ActorSystemReference.ActorSystem.ActorOf(
                    Props.Create(() => 
                                new StocksCoordinatorActor(_chartingActorRef)),
                                "StocksCoordinator");
        }
        public void CreateStockButtonViewModels()
        {
            this.StockButtonViewModels = new Dictionary<string, StockToggleButtonViewModel>();
            this.CreateStockButtonViewModel("AAPL");
            this.CreateStockButtonViewModel("FB");
            this.CreateStockButtonViewModel("BMW");
            this.CreateStockButtonViewModel("TWTR");
        }

        public void CreateStockButtonViewModel(string stockSymbol)
        {
            var newVM = new StockToggleButtonViewModel(_stocksCoordinatorActorRef, stockSymbol);
            StockButtonViewModels.Add(stockSymbol, newVM);
        }
    }
}