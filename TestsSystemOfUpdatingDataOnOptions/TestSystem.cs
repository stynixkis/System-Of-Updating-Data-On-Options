using Module.Black_Shoals.Classes;

namespace TestsSystemOfUpdatingDataOnOptions
{
    public class TestSystem
    {
        [Theory]
        [InlineData(40, 40, 0.1, 0.5, 0.3, 4.36)]
        [InlineData(140, 40, 0.11, 0.2, 0.25, 100.87)]
        [InlineData(70, 10, 0.2, 0.8, 0.7, 61.48)]
        [InlineData(10, 60, 0.1, 0.5, 0.3, 0)]
        public void VerifyCalculationPriceForCallOptionForEuropeanMarket(double currentPriceOfUnderlyingAsset, double strike,
            double riskFreeInterestRate, double timeToOptioneExpiration, double volatility, double expected)
        {
            CalculatingFairPriceOfEuropeanOption optionTest = new CalculatingFairPriceOfEuropeanOption(currentPriceOfUnderlyingAsset,
                strike, riskFreeInterestRate, timeToOptioneExpiration, volatility);

            Assert.Equal(expected, optionTest.PriceOptionCall);
        }

        [Theory]
        [InlineData(40, 40, 0.1, 0.5, 0.3, 2.41)]
        [InlineData(140, 40, 0.11, 0.2, 0.25, 0)]
        [InlineData(70, 10, 0.2, 0.8, 0.7, 0)]
        [InlineData(10, 60, 0.1, 0.5, 0.3, 47.07)]
        public void VerifyCalculationPriceForPutOptionForEuropeanMarket(double currentPriceOfUnderlyingAsset, double strike,
    double riskFreeInterestRate, double timeToOptioneExpiration, double volatility, double expected)
        {
            CalculatingFairPriceOfEuropeanOption optionTest = new CalculatingFairPriceOfEuropeanOption(currentPriceOfUnderlyingAsset,
                strike, riskFreeInterestRate, timeToOptioneExpiration, volatility);

            Assert.Equal(expected, optionTest.PriceOptionPut);
        }

        [Theory]
        [InlineData(40, 40, 0.1, 0.5, 0.3, new double[] { 0.7 }, new double[] { (2.0 / 12) }, 3.94)]
        [InlineData(140, 20, 0.2, 0.335, 0.24, new double[] { 0.7, 0.5 }, new double[] { (2.0 / 12), (6.0 / 12) }, 121.23)]
        [InlineData(240, 140, 0.1, 0.5, 0.3, new double[] { 0.4 }, new double[] { (4.0 / 12) }, 106.47)]
        [InlineData(80, 40, 0.44, 0.5, 0.22, new double[] { 0.7, 0.8, 0.9 }, new double[] { (2.0 / 12), (6.0 / 12), (8.0 / 12) }, 49.53)]
        public void VerifyCalculationPriceForCallOptionForAmericanMarket(double currentPriceOfUnderlyingAsset, double strike,
            double riskFreeInterestRate, double timeToOptioneExpiration, double volatility,
            double[] dividends, double[] dividendTimes, double expected)
        {
            CalculatingFairPriceOfAmericanOption optionTest = new CalculatingFairPriceOfAmericanOption(currentPriceOfUnderlyingAsset, strike,
            riskFreeInterestRate, timeToOptioneExpiration, volatility, dividends, dividendTimes);

            Assert.Equal(expected, optionTest.PriceOptionCall);
        }
        [Theory]
        [InlineData(40, 40, 0.1, 0.5, 0.3, 0.63374, 0.04984, 11.96243, -5.68742, 10.4935)]
        [InlineData(70, 10, 0.2, 0.8, 0.7, 0.99988, 7.84181, 21517.93274, -9415.79791, 6.8094)]
        public void VerifyCalculationGreeksForCallOption(double currentPriceOfUnderlyingAsset, double strike,
            double riskFreeInterestRate, double timeToOptioneExpiration, double volatility, 
            double delta, double gamma, double vega, double theta, double rho)
        {
            CalculatingFairPriceOfEuropeanOption optionTest = new CalculatingFairPriceOfEuropeanOption(currentPriceOfUnderlyingAsset,
                strike, riskFreeInterestRate, timeToOptioneExpiration, volatility);

            Assert.Equal(delta, optionTest.GreeksValue.DeltaOptionCall);
            Assert.Equal(vega, optionTest.GreeksValue.VegaOptionCall);
            Assert.Equal(gamma, optionTest.GreeksValue.GammaOptionCall);
            Assert.Equal(theta, optionTest.GreeksValue.ThetaOptionCall);
            Assert.Equal(rho, optionTest.GreeksValue.RhoOptionCall);
        }
        [Theory]
        [InlineData(40, 40, 0.1, 0.5, 0.3, -0.36626, 0.04984, 11.96243, -1.88251, -8.5311)]
        [InlineData(70, 10, 0.2, 0.8, 0.7, -0.00012, 7.84181, 21517.93274, -9414.09363, -0.0078)]
        public void VerifyCalculationGreeksForPutOption(double currentPriceOfUnderlyingAsset, double strike,
            double riskFreeInterestRate, double timeToOptioneExpiration, double volatility,
            double delta, double gamma, double vega, double theta, double rho)
        {
            CalculatingFairPriceOfEuropeanOption optionTest = new CalculatingFairPriceOfEuropeanOption(currentPriceOfUnderlyingAsset,
                strike, riskFreeInterestRate, timeToOptioneExpiration, volatility);

            Assert.Equal(delta, optionTest.GreeksValue.DeltaOptionPut);
            Assert.Equal(vega, optionTest.GreeksValue.VegaOptionPut);
            Assert.Equal(gamma, optionTest.GreeksValue.GammaOptionPut);
            Assert.Equal(theta, optionTest.GreeksValue.ThetaOptionPut);
            Assert.Equal(rho, optionTest.GreeksValue.RhoOptionPut);
        }
    }
}