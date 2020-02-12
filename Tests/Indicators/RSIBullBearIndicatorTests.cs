using System;
using NUnit.Framework;
using QuantConnect.Indicators;

namespace QuantConnect.Tests.Indicators
{
    [TestFixture]
    public class RSIBullBearIndicatorTests
    {
        [Test]
        public void TestBearishSignalTriggered()
        {
            var startDate = new DateTime(2017, 1, 3);
            RelativeStrengthIndex rsi = new RelativeStrengthIndex("AAPL", 14, MovingAverageType.Simple);
            RSIBullBearIndicator indicator = new RSIBullBearIndicator("AAPL", rsi);
            IndicatorDataPoint dataPoint = new IndicatorDataPoint(startDate, Convert.ToDecimal(GetRandomNumber(127.8, 398.6)));

            Assert.False(indicator.BearishSignalTriggered());

            //prime up indicator with lots of data points
            for (int i = 0; i < 250; i++)
            {
                rsi.Update(dataPoint);
                indicator.Update(dataPoint);
                dataPoint = new IndicatorDataPoint(startDate = startDate.AddDays(1), Convert.ToDecimal(GetRandomNumber(127.8, 398.6)));
            }

            dataPoint = new IndicatorDataPoint(startDate = startDate.AddDays(1), Decimal.Subtract(dataPoint.Value, Convert.ToDecimal(121.8)));
            rsi.Update(dataPoint);
            indicator.Update(dataPoint);

            dataPoint = new IndicatorDataPoint(startDate = startDate.AddDays(1), Decimal.Add(dataPoint.Value, Convert.ToDecimal(249.8)));
            rsi.Update(dataPoint);
            indicator.Update(dataPoint); 

            Assert.True(indicator.BearishSignalTriggered());
        }

        [Test]
        public void TestBullishSignalTriggered()
        {
            var startDate = new DateTime(2017, 1, 3);
            RelativeStrengthIndex rsi = new RelativeStrengthIndex("AAPL", 14, MovingAverageType.Simple);
            RSIBullBearIndicator indicator = new RSIBullBearIndicator("AAPL", rsi);
            IndicatorDataPoint dataPoint = new IndicatorDataPoint(startDate, Convert.ToDecimal(GetRandomNumber(127.8, 398.6)));

            Assert.False(indicator.BullishSignalTriggered());

            //prime up indicator with lots of data points
            for (int i = 0; i < 250; i++)
            {
                rsi.Update(dataPoint);
                indicator.Update(dataPoint);
                Assert.False(indicator.BullishSignalTriggered());
                dataPoint = new IndicatorDataPoint(startDate = startDate.AddDays(1), Convert.ToDecimal(GetRandomNumber(127.8, 398.6)));
            }
            
            dataPoint = new IndicatorDataPoint(startDate = startDate.AddDays(1), Decimal.Add(dataPoint.Value, Convert.ToDecimal(249.8)));
            rsi.Update(dataPoint);
            indicator.Update(dataPoint);

            dataPoint = new IndicatorDataPoint(startDate = startDate.AddDays(1), Decimal.Subtract(dataPoint.Value, Convert.ToDecimal(121.8)));
            rsi.Update(dataPoint);
            indicator.Update(dataPoint);

            Assert.True(indicator.BullishSignalTriggered());
        }

        private double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
