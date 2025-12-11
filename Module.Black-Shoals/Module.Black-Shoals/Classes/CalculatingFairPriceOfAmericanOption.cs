namespace Module.Black_Shoals.Classes
{
    /// <summary>
    /// Класс для подсчета справедливой цены американского опциона
    /// </summary>
    public class CalculatingFairPriceOfAmericanOption : CalculatingFairPriceOfEuropeanOption
    {
        /// <summary>
        /// Стоимость опциона Put - недоступен
        /// </summary>
        public new double? PriceOptionPut
        {
            get
            {
                PriceOptionPut = null;
                string errorMessage = "ОШИБКА: Put опцион не рассчитывается для американских опционов";
                Console.WriteLine(errorMessage);
                return null;
            }
            set { PriceOptionPut = null; }
        }
        /// <summary>
        /// Размеры дивидендов
        /// </summary>
        public double[] Dividends { get; set; }
        /// <summary>
        /// Сроки до исполнения дифидендов
        /// </summary>
        public double[] DividendTimes { get; set; }
        /// <summary>
        /// КОнструктор класса
        /// </summary>
        /// <param name="currentPriceOfUnderlyingAsset">Рыночная цена базового актива</param>
        /// <param name="strike">Цена исполнения (страйк)</param>
        /// <param name="riskFreeInterestRate">Безрисковая процентная ставка</param>
        /// <param name="timeToOptioneExpiration">Время до экспирации</param>
        /// <param name="volatility">Волатильность</param>
        /// <param name="dividends">Размеры дивидендов</param>
        /// <param name="dividendTimes">Сроки до исполнения дивидендов</param>
        public CalculatingFairPriceOfAmericanOption(double currentPriceOfUnderlyingAsset, double strike,
            double riskFreeInterestRate, double timeToOptioneExpiration, double volatility, double[] dividends, double[] dividendTimes)
            : base(currentPriceOfUnderlyingAsset, strike, riskFreeInterestRate, timeToOptioneExpiration, volatility)
        {
            Dividends = dividends;
            DividendTimes = dividendTimes;

            if (dividendTimes.Length == 0 || dividends.Length == 0)
                PriceOptionCall = base.CalculatingPriceOption_Call();
            else
                PriceOptionCall = CalculatingPriceOption_Call();

            GreeksValue = new CalculatingGreeks(this);
        }
        /// <summary>
        /// Метод подсчета цены опциона Call
        /// </summary>
        /// <returns></returns>
        public new double CalculatingPriceOption_Call()
        {
            double[] europeanToLastDividend = FirstMethod();
            double[] europeanToMaturity = SecondMethod();

            if (europeanToMaturity[0] > europeanToLastDividend[0])
            {
                CurrentPriceOfUnderlyingAsset = europeanToMaturity[1];
                TimeToOptioneExpiration = europeanToMaturity[2];
            }
            else
            {
                CurrentPriceOfUnderlyingAsset = europeanToLastDividend[1];
            }

            D1 = Calculating_d1();
            D2 = Calculating_d2();

            PriceOptionCall = base.CalculatingPriceOption_Call();

            GreeksValue = new CalculatingGreeks(this);

            return Math.Max(europeanToMaturity[0], europeanToLastDividend[0]);
        }
        /// <summary>
        /// Первый метод расчёта, который гласит: Европейский колл с тем же сроком погашения, что и у оцениваемого американского колла, 
        /// но с ценой акций, уменьшенной на текущую стоимость дивидендов.
        /// </summary>
        /// <returns></returns>
        private double[] FirstMethod()
        {
            double lastDividendTime = DividendTimes[DividendTimes.Length - 1];
            double adjustedPrice = CurrentPriceOfUnderlyingAsset;

            for (int i = 0; i < Dividends.Length; i++)
            {
                adjustedPrice -= Dividends[i] * Math.Exp(-RiskFreeInterestRate * DividendTimes[i]);
            }

            double tempCurrentPrice = CurrentPriceOfUnderlyingAsset;
            CurrentPriceOfUnderlyingAsset = adjustedPrice;

            D1 = Calculating_d1();
            D2 = Calculating_d2();

            double europeanToLastDividend = base.CalculatingPriceOption_Call();

            Console.WriteLine("Европейский колл с тем же сроком погашения, что и у оцениваемого американского колла, но с ценой акций, уменьшенной на текущую стоимость дивидендов: "
                + europeanToLastDividend);
            Console.WriteLine("d1:" + D1 + "  d2: " + D2);
            CurrentPriceOfUnderlyingAsset = tempCurrentPrice;

            D1 = Calculating_d1();
            D2 = Calculating_d2();

            return [europeanToLastDividend, Math.Round(adjustedPrice, 2)];
        }
        /// <summary>
        /// Это второй метод расчёта, который гласит: 
        /// Европейский колл, срок действия которого истекает за день до выплаты дивидендов. 
        /// Этот метод начинается так же, как и предыдущий, за исключением того, что срок погашения опциона устанавливается на последний срок погашения перед последним дивидендом
        /// </summary>
        /// <returns></returns>
        private double[] SecondMethod()
        {
            if (Dividends.Length < 2 || DividendTimes.Length < 2)
                return [0, CurrentPriceOfUnderlyingAsset, TimeToOptioneExpiration];

            double presentValueOfDividendsAtExDividendDatee = Dividends[Dividends.Length - 2] * Math.Exp(-RiskFreeInterestRate * DividendTimes[DividendTimes.Length - 2]);
            double adjustedPrice = CurrentPriceOfUnderlyingAsset - presentValueOfDividendsAtExDividendDatee;

            double tempCurrentPrice = CurrentPriceOfUnderlyingAsset;
            CurrentPriceOfUnderlyingAsset = adjustedPrice;

            double tempTime = TimeToOptioneExpiration;
            TimeToOptioneExpiration = DividendTimes[DividendTimes.Length - 1];

            D1 = Calculating_d1();
            D2 = Calculating_d2();

            double europeanToLastDividend = base.CalculatingPriceOption_Call();

            Console.WriteLine("Европейский колл, срок действия которого истекает за день до выплаты дивидендов.: "
                + europeanToLastDividend);
            Console.WriteLine("d1:" + D1 + "  d2: " + D2);
            CurrentPriceOfUnderlyingAsset = tempCurrentPrice;
            TimeToOptioneExpiration = tempTime;

            D1 = Calculating_d1();
            D2 = Calculating_d2();

            return [europeanToLastDividend, Math.Round(adjustedPrice, 2), DividendTimes[DividendTimes.Length - 1]];
        }
    }
}
