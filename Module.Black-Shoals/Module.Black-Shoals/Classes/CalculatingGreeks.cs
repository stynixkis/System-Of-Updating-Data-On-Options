using Module.Black_Shoals.Service;

namespace Module.Black_Shoals.Classes
{
    /// <summary>
    /// Класс для подсчета греков
    /// </summary>
    public class CalculatingGreeks
    {
        /// <summary>
        /// Грек Дельта, подсчитываемый для опциона Call
        /// </summary>
        public double DeltaOptionCall { get; set; }
        /// <summary>
        /// Грек Дельта, подсчитываемый для опциона Put (приватное поле)
        /// </summary>
        private double? _deltaOptionPut;
        /// <summary>
        /// Грек Дельта, подсчитываемый для опциона Put (свойство)
        /// </summary>
        public double? DeltaOptionPut
        {
            get
            {
                if (_deltaOptionPut == null)
                {
                    string errorMessage = "ОШИБКА: Греки для Put опциона не рассчитываются для американских опционов";
                    Console.WriteLine(errorMessage);
                    return null;
                }
                else
                    return _deltaOptionPut.Value;
            }
            set { _deltaOptionPut = value; }
        }
        /// <summary>
        /// Грек Гамма, подсчитываемый для опциона Call
        /// </summary>
        public double GammaOptionCall { get; set; }
        /// <summary>
        /// Грек Гамма, подсчитываемый для опциона Put (приватное поле)
        /// </summary>
        private double? _gammaOptionPut;
        /// <summary>
        /// Грек Гамма, подсчитываемый для опциона Put (свойство)
        /// </summary>
        public double? GammaOptionPut
        {
            get
            {
                if (_gammaOptionPut == null)
                {
                    string errorMessage = "ОШИБКА: Греки для Put опциона не рассчитываются для американских опционов";
                    Console.WriteLine(errorMessage);
                    return null;
                }
                else
                    return _gammaOptionPut.Value;
            }
            set { _gammaOptionPut = value; }
        }
        /// <summary>
        /// Грек Вега, подсчитываемый для опциона Call
        /// </summary>
        public double VegaOptionCall { get; set; }
        /// <summary>
        /// Грек Вега, подсчитываемый для опциона Put (приватное поле)
        /// </summary>
        private double? _vegaOptionPut;
        /// <summary>
        /// Грек Вега, подсчитываемый для опциона Put (свойство)
        /// </summary>
        public double? VegaOptionPut
        {
            get
            {
                if (_vegaOptionPut == null)
                {
                    string errorMessage = "ОШИБКА: Греки для Put опциона не рассчитываются для американских опционов";
                    Console.WriteLine(errorMessage);
                    return null;
                }
                else
                    return _vegaOptionPut.Value;
            }
            set { _vegaOptionPut = value; }
        }
        /// <summary>
        /// Грек Тета, подсчитываемый для опциона Call
        /// </summary>
        public double ThetaOptionCall { get; set; }
        /// <summary>
        /// Грек Тета, подсчитываемый для опциона Put (приватное поле)
        /// </summary>
        private double? _thetaOptionPut;
        /// <summary>
        /// Грек Тета, подсчитываемый для опциона Put (свойство)
        /// </summary>
        public double? ThetaOptionPut
        {
            get
            {
                if (_thetaOptionPut == null)
                {
                    string errorMessage = "ОШИБКА: Греки для Put опциона не рассчитываются для американских опционов";
                    Console.WriteLine(errorMessage);
                    return null;
                }
                else
                    return _thetaOptionPut.Value;
            }
            set { _thetaOptionPut = value; }
        }

        /// <summary>
        /// Грек Ро, подсчитываемый для опциона Call
        /// </summary>
        public double RhoOptionCall { get; set; }
        /// <summary>
        /// Грек Ро, подсчитываемый для опциона Put (приватное поле)
        /// </summary>
        private double? _rhoOptionPut;
        /// <summary>
        /// Грек Ро, подсчитываемый для опциона Put (свойство)
        /// </summary>
        public double? RhoOptionPut
        {
            get
            {
                if (_rhoOptionPut == null)
                {
                    string errorMessage = "ОШИБКА: Греки для Put опциона не рассчитываются для американских опционов";
                    Console.WriteLine(errorMessage);
                    return null;
                }
                else
                    return _rhoOptionPut.Value;
            }
            set { _rhoOptionPut = value; }
        }
        /// <summary>
        /// Конструктор класса, принимающий параметры
        /// </summary>
        /// <param name="currentPriceOfUnderlyingAsset">Рыночная цена базового актива</param>
        /// <param name="strike">Цена исполнения (страйк)</param>
        /// <param name="riskFreeInterestRate">Безрисковая процентная ставка</param>
        /// <param name="timeToOptioneExpiration">Время до экспирации</param>
        /// <param name="volatility">Волатильность</param>
        /// <param name="d1">Коэффициент d1</param>
        /// <param name="d2">Коэффициент d2</param>
        public CalculatingGreeks(double currentPriceOfUnderlyingAsset, double strike,
            double riskFreeInterestRate, double timeToOptioneExpiration, double volatility,
            double d1, double d2)
        {
            DeltaOptionCall = Math.Round((CalculatingDeltaCall(d1)), 5);
            DeltaOptionPut = Math.Round((CalculatingDeltaPut(d1)), 5);
            GammaOptionCall = Math.Round((CalculatingGamma(d1, currentPriceOfUnderlyingAsset, volatility, timeToOptioneExpiration)), 5);
            GammaOptionPut = GammaOptionCall;
            VegaOptionCall = Math.Round((CalculatingVega(d1, currentPriceOfUnderlyingAsset, timeToOptioneExpiration)), 5);
            VegaOptionPut = VegaOptionCall;
            ThetaOptionCall = Math.Round((CalculatingTetaCall(currentPriceOfUnderlyingAsset, strike, riskFreeInterestRate,
                timeToOptioneExpiration, volatility, d1, d2)), 5);
            ThetaOptionPut = Math.Round((CalculatingTetaPut(currentPriceOfUnderlyingAsset, strike, riskFreeInterestRate,
                timeToOptioneExpiration, volatility, d1, d2)), 5);
            RhoOptionCall = Math.Round((CalculatingRoCall(strike, riskFreeInterestRate, timeToOptioneExpiration, d2)), 5);
            RhoOptionPut = Math.Round((CalculatingRoPut(strike, riskFreeInterestRate, timeToOptioneExpiration, d2)), 5);
        }
        /// <summary>
        /// Конструктор класса, принимающий параметры
        /// </summary>
        /// <param name="currentPriceOfUnderlyingAsset">Рыночная цена базового актива</param>
        /// <param name="strike">Цена имполнения (страйк)</param>
        /// <param name="riskFreeInterestRate">Безрисковая процентная ставка</param>
        /// <param name="timeToOptioneExpiration">Время до экспирации</param>
        /// <param name="volatility_call">Волатильность для Call опциона</param>
        /// <param name="volatility_put">Волатильность для Put опциона</param>
        /// <param name="d1_call">Коэффициент d1 для опциона Call</param>
        /// <param name="d2_call">Коэффициент d2 для опциона Call</param>
        /// <param name="d1_put">Коэффициент d1 для опциона Put</param>
        /// <param name="d2_put">Коэффициент d2 для опциона Put</param>
        public CalculatingGreeks(double currentPriceOfUnderlyingAsset, double strike,
            double riskFreeInterestRate, double timeToOptioneExpiration, double volatility_call, double volatility_put,
            double d1_call, double d2_call, double d1_put, double d2_put)
        {
            DeltaOptionCall = Math.Round((CalculatingDeltaCall(d1_call)), 5);
            DeltaOptionPut = Math.Round((CalculatingDeltaPut(d1_put)), 5);
            GammaOptionCall = Math.Round((CalculatingGamma(d1_call, currentPriceOfUnderlyingAsset, volatility_call, timeToOptioneExpiration)), 5);
            GammaOptionPut = Math.Round((CalculatingGamma(d1_put, currentPriceOfUnderlyingAsset, volatility_put, timeToOptioneExpiration)), 5);
            VegaOptionCall = Math.Round((CalculatingVega(d1_call, currentPriceOfUnderlyingAsset, timeToOptioneExpiration)), 5);
            VegaOptionPut = Math.Round((CalculatingVega(d1_put, currentPriceOfUnderlyingAsset, timeToOptioneExpiration)), 5);
            ThetaOptionCall = Math.Round((CalculatingTetaCall(currentPriceOfUnderlyingAsset, strike, riskFreeInterestRate,
                timeToOptioneExpiration, volatility_call, d1_call, d2_call)), 5);
            ThetaOptionPut = Math.Round((CalculatingTetaPut(currentPriceOfUnderlyingAsset, strike, riskFreeInterestRate,
                timeToOptioneExpiration, volatility_put, d1_put, d2_put)), 5);
            RhoOptionCall = Math.Round((CalculatingRoCall(strike, riskFreeInterestRate, timeToOptioneExpiration, d2_call)), 5);
            RhoOptionPut = Math.Round((CalculatingRoPut(strike, riskFreeInterestRate, timeToOptioneExpiration, d2_put)), 5);
        }

        /// <summary>
        /// Конструктор класса, принимающий экземпляр класса для подсчета европейского опциона
        /// </summary>
        /// <param name="europeanOption">Экземпляр класса CalculatingFairPriceOfEuropeanOption</param>
        public CalculatingGreeks(CalculatingFairPriceOfEuropeanOption europeanOption)
        {
            DeltaOptionCall = Math.Round((CalculatingDeltaCall(europeanOption.D1)), 5);
            DeltaOptionPut = Math.Round((CalculatingDeltaPut(europeanOption.D1)), 5);
            GammaOptionCall = Math.Round((CalculatingGamma(europeanOption.D1, europeanOption.CurrentPriceOfUnderlyingAsset, europeanOption.Volatility, europeanOption.TimeToOptioneExpiration)), 5);
            GammaOptionPut = GammaOptionCall;
            VegaOptionCall = Math.Round((CalculatingVega(europeanOption.D1, europeanOption.CurrentPriceOfUnderlyingAsset, europeanOption.TimeToOptioneExpiration)), 5);
            VegaOptionPut = VegaOptionCall;
            ThetaOptionCall = Math.Round((CalculatingTetaCall(europeanOption.CurrentPriceOfUnderlyingAsset, europeanOption.Strike, europeanOption.RiskFreeInterestRate,
                europeanOption.TimeToOptioneExpiration, europeanOption.Volatility, europeanOption.D1, europeanOption.D2)), 5);
            ThetaOptionPut = Math.Round((CalculatingTetaPut(europeanOption.CurrentPriceOfUnderlyingAsset, europeanOption.Strike, europeanOption.RiskFreeInterestRate,
                europeanOption.TimeToOptioneExpiration, europeanOption.Volatility, europeanOption.D1, europeanOption.D2)), 5);
            RhoOptionCall = Math.Round((CalculatingRoCall(europeanOption.Strike, europeanOption.RiskFreeInterestRate, europeanOption.TimeToOptioneExpiration, europeanOption.D2)), 5);
            RhoOptionPut = Math.Round((CalculatingRoPut(europeanOption.Strike, europeanOption.RiskFreeInterestRate, europeanOption.TimeToOptioneExpiration, europeanOption.D2)), 5);
        }
        /// <summary>
        /// Конструктор класса, принимающий экземпляр класса для подсчета американского опциона
        /// </summary>
        /// <param name="americanOption">Экземпляр класса CalculatingFairPriceOfAmericanOption</param>
        public CalculatingGreeks(CalculatingFairPriceOfAmericanOption americanOption)
        {
            DeltaOptionCall = Math.Round((CalculatingDeltaCall(americanOption.D1)), 5);
            DeltaOptionPut = null;
            GammaOptionCall = Math.Round((CalculatingGamma(americanOption.D1, americanOption.CurrentPriceOfUnderlyingAsset, americanOption.Volatility, americanOption.TimeToOptioneExpiration)), 5);
            GammaOptionPut = null;
            VegaOptionCall = Math.Round((CalculatingVega(americanOption.D1, americanOption.CurrentPriceOfUnderlyingAsset, americanOption.TimeToOptioneExpiration)), 5);
            VegaOptionPut = null;
            ThetaOptionCall = Math.Round((CalculatingTetaCall(americanOption.CurrentPriceOfUnderlyingAsset, americanOption.Strike, americanOption.RiskFreeInterestRate,
                americanOption.TimeToOptioneExpiration, americanOption.Volatility, americanOption.D1, americanOption.D2)), 5);
            ThetaOptionPut = null;
            RhoOptionCall = Math.Round((CalculatingRoCall(americanOption.Strike, americanOption.RiskFreeInterestRate, americanOption.TimeToOptioneExpiration, americanOption.D2)), 5);
            RhoOptionPut = null;
        }

        /// <summary>
        /// Метод подсчета грека Дельта для опциона Call
        /// </summary>
        /// <param name="value">Коэффициент d1</param>
        /// <returns></returns>
        private double CalculatingDeltaCall(double value)
        {
            return Math.Round(Methods.StandardNormalCDF(value), 5);
        }
        /// <summary>
        /// Метод подсчета грека Дельта для опциона Put
        /// </summary>
        /// <param name="value">Коэффициент d1</param>
        /// <returns></returns>
        private double CalculatingDeltaPut(double value)
        {
            return Math.Round((Methods.StandardNormalCDF(value) - 1), 5);
        }
        /// <summary>
        /// Метод подсчета грека Гамма
        /// </summary>
        /// <param name="d1">Коэффициент d1</param>
        /// <param name="currentPriceOfUnderlyingAsset">Рыночная цена базового актива</param>
        /// <param name="volatility">Волатильность</param>
        /// <param name="timeToOptioneExpiration">Время до экспирации</param>
        /// <returns></returns>
        private double CalculatingGamma(double d1, double currentPriceOfUnderlyingAsset,
            double volatility, double timeToOptioneExpiration)
        {
            double valueOne = Methods.StandardNormalCDFDerivative(d1);
            double valueTwo = currentPriceOfUnderlyingAsset * volatility * Math.Sqrt(timeToOptioneExpiration);
            var result = valueOne / valueTwo;

			return Math.Round(result, 5);
        }
        /// <summary>
        /// Метод подсчета грека Вега
        /// </summary>
        /// <param name="d1">Коэффициент d1</param>
        /// <param name="currentPriceOfUnderlyingAsset">Рыночная цена базового актива</param>
        /// <param name="timeToOptioneExpiration">Время до экспирации</param>
        /// <returns></returns>
        private double CalculatingVega(double d1, double currentPriceOfUnderlyingAsset,
            double timeToOptioneExpiration)
        {
            return Math.Round((currentPriceOfUnderlyingAsset * Methods.StandardNormalCDFDerivative(d1) * Math.Sqrt(timeToOptioneExpiration)), 5);
        }
        /// <summary>
        /// Метод подсчета грека Тета для опциона Call
        /// </summary>
        /// <param name="currentPriceOfUnderlyingAsset">Рыночная цена базового актива</param>
        /// <param name="strike">Цена исполнения (страйк)</param>
        /// <param name="riskFreeInterestRate">Безрисковая процентная ставка</param>
        /// <param name="timeToOptioneExpiration">Время до экспирации</param>
        /// <param name="volatility">Волатильность</param>
        /// <param name="d1">Коэффициент d1</param>
        /// <param name="d2">Коэффициент d2</param>
        /// <returns></returns>
        private double CalculatingTetaCall(double currentPriceOfUnderlyingAsset, double strike,
            double riskFreeInterestRate, double timeToOptioneExpiration, double volatility,
            double d1, double d2)
        {
            double valueOne = currentPriceOfUnderlyingAsset * Methods.StandardNormalCDFDerivative(d1) * volatility;
            double valueTwo = valueOne / (2 * Math.Sqrt(timeToOptioneExpiration));
            double valueThree = riskFreeInterestRate * strike * Math.Exp(-riskFreeInterestRate * timeToOptioneExpiration) * Methods.StandardNormalCDF(d2);
            var result = (-valueTwo - valueThree);

			return Math.Round(result, 5);
        }
        /// <summary>
        /// Метод подсчета грека Тета для опциона Put
        /// </summary>
        /// <param name="currentPriceOfUnderlyingAsset">Рыночная цена базового актива</param>
        /// <param name="strike">Цена исполнения (страйк)</param>
        /// <param name="riskFreeInterestRate">Безрисковая процентная ставка</param>
        /// <param name="timeToOptioneExpiration">Время до экспирации</param>
        /// <param name="volatility">Волатильность</param>
        /// <param name="d1">Коэффициент d1</param>
        /// <param name="d2">Коэффициент d2</param>
        /// <returns></returns>
        private double CalculatingTetaPut(double currentPriceOfUnderlyingAsset, double strike,
            double riskFreeInterestRate, double timeToOptioneExpiration, double volatility,
            double d1, double d2)
        {
            double valueOne = currentPriceOfUnderlyingAsset * Methods.StandardNormalCDFDerivative(d1) * volatility;
            double valueTwo = valueOne / (2 * Math.Sqrt(timeToOptioneExpiration));
            double valueThree = riskFreeInterestRate * strike * Math.Exp(-riskFreeInterestRate * timeToOptioneExpiration) * Methods.StandardNormalCDF(-d2);
            var result = (-valueTwo + valueThree); 
            return Math.Round(result, 5);
        }
        /// <summary>
        /// Метод подсчета грека Ро для опциона Call
        /// </summary>
        /// <param name="strike">Цена исполнения (страйк)</param>
        /// <param name="riskFreeInterestRate">Безрисковая процентная ставка</param>
        /// <param name="timeToOptioneExpiration">Время до экспирации</param>
        /// <param name="d2">Коэффициент d2</param>
        /// <returns></returns>
        private double CalculatingRoCall(double strike, double riskFreeInterestRate, double timeToOptioneExpiration, double d2)
        {
            return Math.Round((strike * timeToOptioneExpiration * Math.Exp(-riskFreeInterestRate * timeToOptioneExpiration) * Methods.StandardNormalCDF(d2)), 4);
        }
        /// <summary>
        /// Метод подсчета грека Ро для опциона Put
        /// </summary>
        /// <param name="strike">Цена исполнения (страйк)</param>
        /// <param name="riskFreeInterestRate">Безрисковая процентная ставка</param>
        /// <param name="timeToOptioneExpiration">Время до экспирации</param>
        /// <param name="d2">Коэффициент d2</param>
        /// <returns></returns>
        private double CalculatingRoPut(double strike, double riskFreeInterestRate, double timeToOptioneExpiration, double d2)
        {
            return Math.Round((-strike * timeToOptioneExpiration * Math.Exp(-riskFreeInterestRate * timeToOptioneExpiration) * Methods.StandardNormalCDF(-d2)), 4);
        }
    }
}
