using System;

namespace QuantConnect.Indicators
{
    /// <summary>
    /// Represents an abstraction of the Relative Strength Index (RSI) to signal creation of bullish or bearish trades
    /// </summary>
    public class RSIBullBearIndicator : Indicator, IIndicatorWarmUpPeriodProvider
    {
        private RelativeStrengthIndex _rsi;
        private const decimal LowerLimit = 30;
        private const decimal UpperLimit = 70;
        private decimal _currentRsi;
        private decimal _previousRsi;

        /// <summary>
        /// Initializes a new instance of the RSIBullBearIndicator class with the specified name and Relative Strength Indicator 
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        /// <param name="rsi">The Relative Strength Index used by this indicator</param>
        public RSIBullBearIndicator(String name, RelativeStrengthIndex rsi) 
            : base(name)
        {
            _rsi = rsi;
            WarmUpPeriod = rsi.WarmUpPeriod;
        }

        /// <summary>
        /// Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady => _rsi.IsReady;

        /// <summary>
        /// Required period, in data points, for the indicator to be ready and fully initialized.
        /// </summary>
        public int WarmUpPeriod { get; }

        /// <summary>
        /// Signals a bearish scenario to indicate a short position should be entered
        /// </summary>
        public bool BearishSignalTriggered()
        {
            if (_rsi.IsReady)
            {
                if (_previousRsi < UpperLimit && _currentRsi > UpperLimit)
                {
                    return true;
                }

                if (_previousRsi < LowerLimit && _currentRsi > LowerLimit)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Signals a bullish scenario to indicate a long position should be entered
        /// </summary>
        public bool BullishSignalTriggered()
        {
            if (_rsi.IsReady)
            {
                if (_previousRsi > UpperLimit && _currentRsi < UpperLimit)
                {
                    return true;
                }

                if (_previousRsi > LowerLimit && _currentRsi < LowerLimit)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator</returns>
        protected override decimal ComputeNextValue(IndicatorDataPoint input)
        {
            _previousRsi = _currentRsi;
            _currentRsi = _rsi;
            return _currentRsi;
        }

        /// <summary>
        /// Resets this indicator to its initial state
        /// </summary>
        public override void Reset()
        {
            _previousRsi = 0;
            _currentRsi = 0;
            base.Reset();
        }
    }
}
