using System;
using System.Collections.Generic;
using QuantConnect.Data;
using QuantConnect.Indicators;
using QuantConnect.Interfaces;

namespace QuantConnect.Algorithm.CSharp
{
    /// <summary>
    /// Algorithm to generate orders according to the RSIBullBearIndicator
    /// </summary>
    public class StandardRelativeStrengthIndicatorAlgorithm : QCAlgorithm, IRegressionAlgorithmDefinition
    {
        private readonly Symbol _symbol = QuantConnect.Symbol.Create("AAPL", SecurityType.Equity, Market.USA);
        private RSIBullBearIndicator _rsiBullBearIndicator;
        private RelativeStrengthIndex _rsi;  

        /// <summary>
        /// Algorithm initialization
        /// </summary>
        public override void Initialize()
        {
            SetStartDate(1998, 01, 02);
            SetEndDate(2018, 07, 03);
            SetCash(10000);

            AddEquity(_symbol, Resolution.Daily);

            SetWarmUp(TimeSpan.FromDays(30));

            _rsi = RSI(_symbol, 14, MovingAverageType.Simple, Resolution.Daily);

            _rsiBullBearIndicator = RSIBB(_symbol, _rsi);
        }

        /// <summary>
        /// OnData event is the primary entry point for your algorithm. Each new data point will be pumped in here.
        /// </summary>
        /// <param name="data">Slice object keyed by symbol containing the stock data</param>
        public override void OnData(Slice data)
        {
            var holdings = Portfolio[_symbol].Quantity;

            if (Math.Abs(holdings) <= 10)
            {
                if (_rsiBullBearIndicator.BullishSignalTriggered())
                {
                    Buy(_symbol, 1);
                    Debug("Purchased 1 Unit of " + _symbol + "");
                }
                else if (_rsiBullBearIndicator.BearishSignalTriggered())
                {
                    Sell(_symbol, 1);
                    Debug("Sold 1 Unit of " + _symbol + "");
                }
            }
        }

        /// <summary>
        /// This is used by the regression test system to indicate if the open source Lean repository has the required data to run this algorithm.
        /// </summary>
        public bool CanRunLocally { get; } = true;

        /// <summary>
        /// This is used by the regression test system to indicate which languages this algorithm is written in.
        /// </summary>
        public Language[] Languages { get; } = { Language.CSharp };

        /// <summary>
        /// This is used by the regression test system to indicate what the expected statistics are from running the algorithm
        /// </summary>
        public Dictionary<string, string> ExpectedStatistics => new Dictionary<string, string>
        {
            {"Total Trades", "1"},
            {"Average Win", "0%"},
            {"Average Loss", "0%"},
            {"Compounding Annual Return", "36.057%"},
            {"Drawdown", "2.300%"},
            {"Expectancy", "0"},
            {"Net Profit", "0.394%"},
            {"Sharpe Ratio", "3.816"},
            {"Probabilistic Sharpe Ratio", "60.034%"},
            {"Loss Rate", "0%"},
            {"Win Rate", "0%"},
            {"Profit-Loss Ratio", "0"},
            {"Alpha", "0"},
            {"Beta", "0.997"},
            {"Annual Standard Deviation", "0.178"},
            {"Annual Variance", "0.032"},
            {"Information Ratio", "-3.872"},
            {"Tracking Error", "0"},
            {"Treynor Ratio", "0.68"},
            {"Total Fees", "$7.78"},
            {"Fitness Score", "0.031"},
            {"Kelly Criterion Estimate", "0"},
            {"Kelly Criterion Probability Value", "0"},
            {"Sortino Ratio", "-2.133"},
            {"Return Over Maximum Drawdown", "-8.479"},
            {"Portfolio Turnover", "0.25"},
            {"Total Insights Generated", "1"},
            {"Total Insights Closed", "0"},
            {"Total Insights Analysis Completed", "0"},
            {"Long Insight Count", "1"},
            {"Short Insight Count", "0"},
            {"Long/Short Ratio", "100%"},
            {"Estimated Monthly Alpha Value", "$0"},
            {"Total Accumulated Estimated Alpha Value", "$0"},
            {"Mean Population Estimated Insight Value", "$0"},
            {"Mean Population Direction", "0%"},
            {"Mean Population Magnitude", "0%"},
            {"Rolling Averaged Population Direction", "0%"},
            {"Rolling Averaged Population Magnitude", "0%"}
        };
    }
}
