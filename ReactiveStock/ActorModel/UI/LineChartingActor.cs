using Akka.Actor;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using ReactiveStock.ActorModel.Messages;
using System.Collections.Generic;

namespace ReactiveStock.ActorModel.UI
{
    public class LineChartingActor :ReceiveActor
    {
        private readonly PlotModel _chartModel;
        private readonly Dictionary<string, LineSeries> _series;
        public LineChartingActor(PlotModel chartModel)
        {
            _chartModel = chartModel;
            _series = new Dictionary<string, LineSeries>();
            this.Receive<AddChartSeriesMessage>(m => this.AddSeriesToChart(m));
            this.Receive<RemoveChartSeriesMessage>(m => this.RemoveSeriesFromChart(m));
            this.Receive<StockPriceMessage>(m => this.HandleNewStockPrice(m));
        }

        public void AddSeriesToChart(AddChartSeriesMessage m)
        {
            if (!_series.ContainsKey(m.StockSymbol))
            {
                var newLineSeries = new LineSeries {
                    StrokeThickness = 2,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Black,
                    MarkerType = MarkerType.None,
                    CanTrackerInterpolatePoints =false,
                    Title = m.StockSymbol,
                    Smooth =false
                };

                _series.Add(m.StockSymbol, newLineSeries);
                _chartModel.Series.Add(newLineSeries);
                RefreshChart();
            }
        }

        private void RemoveSeriesFromChart(RemoveChartSeriesMessage m)
        {
            if (!_series.ContainsKey(m.StockSymbol))
            {
                var seriesToRemove = _series[m.StockSymbol];
                _chartModel.Series.Remove(seriesToRemove);
                _series.Remove(m.StockSymbol);
                RefreshChart();
            }
        }

        public void RefreshChart()
        {
            _chartModel.InvalidatePlot(true);
        }

        public void HandleNewStockPrice(StockPriceMessage m)
        {
            if (_series.ContainsKey(m.StockSymbol))
            {
                var series = _series[m.StockSymbol];
                var newDatePoint = new DataPoint(DateTimeAxis.ToDouble(m.Date), LinearAxis.ToDouble(m.StockPrice));

                if(series.Points.Count > 10)
                {
                    series.Points.RemoveAt(0);
                }
                series.Points.Add(newDatePoint);
                RefreshChart();
            }
        }
    }
}
