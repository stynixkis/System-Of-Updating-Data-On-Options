using Module.Black_Shoals.Service;

namespace Module.Black_Shoals.Classes
{
    /// <summary>
    /// Класс для подсчета справедливой цены европейского опциона
    /// </summary>
    public class CalculatingFairPriceOfEuropeanOption
    {
        /// <summary>
        /// Рыночная цена базового актива
        /// </summary>
        public double CurrentPriceOfUnderlyingAsset { get; set; }
        /// <summary>
        /// Цена исполнения (страйк)
        /// </summary>
        public double Strike { get; set; }
        /// <summary>
        /// Безрисковая процентная ставка
        /// </summary>
        public double RiskFreeInterestRate { get; set; }
        /// <summary>
        /// Время до экспирации
        /// </summary>
        public double TimeToOptioneExpiration { get; set; }
        /// <summary>
        /// Волатильность
        /// </summary>
        public double Volatility { get; set; }
        /// <summary>
        /// Коэффициент d1
        /// </summary>
        internal double D1 { get; set; }
        /// <summary>
        /// Коэффициент d2
        /// </summary>
        internal double D2 { get; set; }
        /// <summary>
        /// Стоимость опциона Call
        /// </summary>
        public double PriceOptionCall { get; set; }
        /// <summary>
        /// Стоимость опциона Put
        /// </summary>
        public double PriceOptionPut { get; private set; }
        /// <summary>
        /// Экземпляр класса, содержащий значения греков
        /// </summary>
        public CalculatingGreeks GreeksValue { get; set; }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="currentPriceOfUnderlyingAsset">Рыночная цена базового актива</param>
        /// <param name="strike">Цена исполнения (страйк)</param>
        /// <param name="riskFreeInterestRate">Безрисковая процентная ставка</param>
        /// <param name="timeToOptioneExpiration">Время до экспирации</param>
        /// <param name="volatility">Волатильность</param>
        public CalculatingFairPriceOfEuropeanOption(double currentPriceOfUnderlyingAsset, double strike,
            double riskFreeInterestRate, double timeToOptioneExpiration, double volatility)
        {
            CurrentPriceOfUnderlyingAsset = currentPriceOfUnderlyingAsset;
            Strike = strike;
            RiskFreeInterestRate = riskFreeInterestRate;
            TimeToOptioneExpiration = timeToOptioneExpiration;
            Volatility = volatility;

            D1 = Calculating_d1();
            D2 = Calculating_d2();

            PriceOptionCall = CalculatingPriceOption_Call();
            PriceOptionPut = CalculatingPriceOption_Put();

            GreeksValue = new CalculatingGreeks(this);
        }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="currentPriceOfUnderlyingAsset">Рыночная цена</param>
        /// <param name="strike">Цена исполнения (страйк)</param>
        /// <param name="riskFreeInterestRate">Безрисковая процентная ставка</param>
        /// <param name="timeToOptioneExpiration">Время до экспирации</param>
        /// <param name="volatilityForCall">Волатильность для опциона Call</param>
        /// <param name="volatilityForPut">Волатильность для опциона Put</param>
        public CalculatingFairPriceOfEuropeanOption(double currentPriceOfUnderlyingAsset, double strike,
    double riskFreeInterestRate, double timeToOptioneExpiration, double volatilityForCall, double volatilityForPut)
        {
            CurrentPriceOfUnderlyingAsset = currentPriceOfUnderlyingAsset;
            Strike = strike;
            RiskFreeInterestRate = riskFreeInterestRate;
            TimeToOptioneExpiration = timeToOptioneExpiration;
            Volatility = (volatilityForCall + volatilityForPut) / 2.0;

            D1 = Calculating_d1(volatilityForCall);
            D2 = Calculating_d2(volatilityForCall);

            double d1_call = D1; double d2_call = D2;

            PriceOptionCall = CalculatingPriceOption_Call();

            D1 = Calculating_d1(volatilityForPut);
            D2 = Calculating_d2(volatilityForPut);

            PriceOptionPut = CalculatingPriceOption_Put();

            GreeksValue = new CalculatingGreeks(CurrentPriceOfUnderlyingAsset, Strike, RiskFreeInterestRate, TimeToOptioneExpiration,
                volatilityForCall, volatilityForPut, d1_call, d2_call, D1, D2);
        }

        /// <summary>
        /// Метод подсчета коэффициента d1
        /// </summary>
        /// <returns></returns>
        protected double Calculating_d1()
        {
            double valueOne = Math.Log(CurrentPriceOfUnderlyingAsset / Strike);
            double valueTwo = RiskFreeInterestRate + (Math.Pow(Volatility, 2) / 2);
            double valueNumerator = valueOne + valueTwo * TimeToOptioneExpiration;
            double valueDenominator = Volatility * Math.Sqrt(TimeToOptioneExpiration);
            var result = (valueNumerator / valueDenominator);
			return Math.Round(result, 5);
        }
        /// <summary>
        ///  Метод подсчета коэффициента d1 с передаваемой волатильностью
        /// </summary>
        /// <param name="volatility">Волатильность</param>
        /// <returns></returns>
        protected double Calculating_d1(double volatility)
        {
            double valueOne = Math.Log(CurrentPriceOfUnderlyingAsset / Strike);
            double valueTwo = RiskFreeInterestRate + (Math.Pow(volatility, 2) / 2);
            double valueNumerator = valueOne + valueTwo * TimeToOptioneExpiration;
            double valueDenominator = volatility * Math.Sqrt(TimeToOptioneExpiration);
            var result = (valueNumerator / valueDenominator);
			return Math.Round(result, 5);
        }
        /// <summary>
        /// Метод подсчета коэффициента d2
        /// </summary>
        /// <returns></returns>
        protected double Calculating_d2()
        {
            return Math.Round((D1 - Volatility * Math.Sqrt(TimeToOptioneExpiration)), 5);
        }
        /// <summary>
        /// Метод подсчета коэффициента d2 с передаваемой волатильностью
        /// </summary>
        /// <param name="volatility">Волатильность</param>
        /// <returns></returns>
        protected double Calculating_d2(double volatility)
        {
            return Math.Round((D1 - volatility * Math.Sqrt(TimeToOptioneExpiration)), 5);
        }
        /// <summary>
        /// Метод подсчета цены опциона Call
        /// </summary>
        /// <returns></returns>
        protected double CalculatingPriceOption_Call()
        {
            double valueOne = CurrentPriceOfUnderlyingAsset * Methods.StandardNormalCDF(D1);
            double valueTwo = Strike * Math.Exp(-RiskFreeInterestRate * TimeToOptioneExpiration) * Methods.StandardNormalCDF(D2);
            var result = (valueOne - valueTwo);
            return Math.Round(result, 2);
        }
        /// <summary>
        /// Метод подсчета цены опциона Put
        /// </summary>
        /// <returns></returns>
        private double CalculatingPriceOption_Put()
        {
            double valueOne = CurrentPriceOfUnderlyingAsset * Methods.StandardNormalCDF(-D1);
            double valueTwo = Strike * Math.Exp(-RiskFreeInterestRate * TimeToOptioneExpiration) * Methods.StandardNormalCDF(-D2);
            var result = (valueTwo - valueOne);
			return Math.Round(result, 2);
        }
    }
}
