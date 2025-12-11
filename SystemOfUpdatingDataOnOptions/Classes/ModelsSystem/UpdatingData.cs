using Module.Black_Shoals.Classes;
using SystemOfUpdatingDataOnOptions.Classes.ModelsDBCalculationsOnOptions;
using SystemOfUpdatingDataOnOptions.Classes.ModelsDBFinancialOptionsSystem;

namespace SystemOfUpdatingDataOnOptions.Classes.ModelsSystem
{
    /// <summary>
    /// Класс обновления данных из бд по таймеру
    /// </summary>
    public static class UpdatingData
    {
        /// <summary>
        /// Таймер
        /// </summary>
        private static Timer _timer;
        /// <summary>
        /// Конструктор класса для автоматического запуска таймера
        /// </summary>
        static UpdatingData()
        {
            _timer = new Timer(UpdatingDataOnOption, null, 0, 20000);
        }
        /// <summary>
        /// Метод для использования класса
        /// </summary>
        public static void MainUpdating()
        {
            Console.WriteLine("Запущена система актуализации данных. Время обновления - 5 минут");
        }
        /// <summary>
        /// Метод для запуска обновления данных по таймеру
        /// </summary>
        /// <param name="obj"></param>
        private static void UpdatingDataOnOption(Object obj)
        {
            try
            {
                Console.WriteLine($"Начало обновления данных: {DateTime.Now}");
                UpdatingDataOnEuropeanOption();
                UpdatingDataOnAmericanOption();
                Console.WriteLine($"Обновление завершено: {DateTime.Now}");
            }
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при обновлении данных: {ex.Message}");

				// ДЕТАЛЬНОЕ ЛОГИРОВАНИЕ ИСКЛЮЧЕНИЯ
				Console.WriteLine("=== ПОЛНАЯ ИНФОРМАЦИЯ ОБ ОШИБКЕ ===");
				Console.WriteLine($"Сообщение: {ex.Message}");
				Console.WriteLine($"Тип исключения: {ex.GetType().Name}");
				Console.WriteLine($"StackTrace: {ex.StackTrace}");

				if (ex.InnerException != null)
				{
					Console.WriteLine($"\nInnerException:");
					Console.WriteLine($"Сообщение: {ex.InnerException.Message}");
					Console.WriteLine($"Тип: {ex.InnerException.GetType().Name}");

					if (ex.InnerException.InnerException != null)
					{
						Console.WriteLine($"\nInnerInnerException:");
						Console.WriteLine($"Сообщение: {ex.InnerException.InnerException.Message}");
						Console.WriteLine($"Тип: {ex.InnerException.InnerException.GetType().Name}");
					}
				}

				Console.WriteLine("=== КОНЕЦ ИНФОРМАЦИИ ОБ ОШИБКЕ ===");
			}
		}
        /// <summary>
        /// Обновление данных европейских опционов
        /// </summary>
        private static void UpdatingDataOnEuropeanOption()
        {
            Console.WriteLine("\nЕВРОПА\n");

            using (var db = new FinancialOptionsSystemContext())
            {
                var europeanActiveIds = db.Actives
                    .Where(a => a.ActiveStyle.ToLower() == "european")
                    .Select(a => a.ActiveId)
                    .ToList();

                if (!europeanActiveIds.Any())
                {
                    Console.WriteLine("Нет европейских активов для обработки");
                    return;
                }

                var optionsEuropean = db.Options
                    .Where(o => europeanActiveIds.Contains(o.ActiveId)
                        && o.ExpirationDate >= DateOnly.FromDateTime(DateTime.Today))
                    .Distinct()
                    .ToList();


                foreach (var option in optionsEuropean)
                {
                    var valueActive = db.Actives.Where(p => p.ActiveId == option.ActiveId).FirstOrDefault();

                    if (valueActive == null)
                    {
                        Console.WriteLine("Нет актива, который соответствует данному опциону");
                        continue;
                    }

                    CalculatingFairPriceOfEuropeanOption optionEuropean = new CalculatingFairPriceOfEuropeanOption((double)valueActive.Price, (double)option.Strike,
                        (double)valueActive.RiskFreeRate, CalculateTimeExpiration(option.ExpirationDate), (double)option.PredefinedIvForCall, (double)option.PredefinedIvForPut);

                    PrintEurop(optionEuropean);
                    Console.WriteLine("\n---------\n");

                    using (var dbc = new CalculationsOnOptionsContext())
                    {
                        var recurringRecord = dbc.Results
                            .Where(p => p.OptionName == option.OptionId &&
                                        Math.Round(Convert.ToDecimal(optionEuropean.PriceOptionCall), 5) == p.CalculationCallOptionPrice &&
                                        Math.Round(Convert.ToDecimal(optionEuropean.PriceOptionPut), 5) == p.CalculationPutOptionPrice &&
                                        Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.DeltaOptionCall), 5) == p.CalculationDeltaForCallOption &&
                                        Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.DeltaOptionPut), 5) == p.CalculationDeltaForPutOption &&
                                        Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.GammaOptionCall), 5) == p.CalculationGammaForCallOption &&
                                        Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.GammaOptionPut), 5) == p.CalculationGammaForPutOption &&
                                        Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.VegaOptionCall), 5) == p.CalculationVegaForCallOption &&
                                        Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.VegaOptionPut), 5) == p.CalculationVegaForPutOption &&
                                        Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.ThetaOptionCall), 5) == p.CalculationThetaForCallOption &&
                                        Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.ThetaOptionPut), 5) == p.CalculationThetaForPutOption &&
                                        Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.RhoOptionCall), 5) == p.CalculationRhoForCallOption &&
                                        Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.RhoOptionPut), 5) == p.CalculationRhoForPutOption)
                            .FirstOrDefault();

                        if (recurringRecord != null)
                        {
                            Console.WriteLine("Запись с идентичными параметрами существует");
                        }
                        else
                        {
                            Result updatingOption = new Result();
                            long maxId = dbc.Results.Any() ? dbc.Results.Max(r => r.ResultId) : 0;
                            updatingOption.ResultId = maxId + 1;
                            updatingOption.OptionName = option.OptionId;
                            updatingOption.CalculationCallOptionPrice = Math.Round(Convert.ToDecimal(optionEuropean.PriceOptionCall), 5);
                            updatingOption.CalculationPutOptionPrice = Math.Round(Convert.ToDecimal(optionEuropean.PriceOptionPut), 5);
                            updatingOption.CalculationDeltaForCallOption = Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.DeltaOptionCall), 5);
                            updatingOption.CalculationDeltaForPutOption = Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.DeltaOptionPut), 5);
                            updatingOption.CalculationGammaForCallOption = Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.GammaOptionCall), 5);
                            updatingOption.CalculationGammaForPutOption = Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.GammaOptionPut), 5);
                            updatingOption.CalculationVegaForCallOption = Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.VegaOptionCall), 5);
                            updatingOption.CalculationVegaForPutOption = Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.VegaOptionPut), 5);
                            updatingOption.CalculationThetaForCallOption = Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.ThetaOptionCall), 5);
                            updatingOption.CalculationThetaForPutOption = Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.ThetaOptionPut), 5);
                            updatingOption.CalculationRhoForCallOption = Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.RhoOptionCall), 5);
                            updatingOption.CalculationRhoForPutOption = Math.Round(Convert.ToDecimal(optionEuropean.GreeksValue.RhoOptionPut), 5);
                            updatingOption.DateOfFixation = DateTime.Now;

                            dbc.Results.Add(updatingOption);
                            dbc.SaveChanges();
                            Console.WriteLine($"Запись {updatingOption.ResultId} добавлена");
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Обновление данных американских опционов
        /// </summary>
        private static void UpdatingDataOnAmericanOption()
        {
            Console.WriteLine("\nАМЕРИКА\n");

            using (var db = new FinancialOptionsSystemContext())
            {
                var americanActiveIds = db.Actives
                    .Where(a => a.ActiveStyle.ToLower() == "american")
                    .Select(a => a.ActiveId)
                    .ToList();

                if (!americanActiveIds.Any())
                {
                    Console.WriteLine("Нет американских активов для обработки");
                    return;
                }

                var optionsAmerican = db.Options
                    .Where(o => americanActiveIds.Contains(o.ActiveId)
                        && o.ExpirationDate >= DateOnly.FromDateTime(DateTime.Today))
                    .Distinct()
                    .ToList();

                foreach (var option in optionsAmerican)
                {
                    var valueActive = db.Actives.Where(p => p.ActiveId == option.ActiveId).FirstOrDefault();

                    if (valueActive == null)
                    {
                        Console.WriteLine("Нет актива, который соответствует данному опциону");
                        continue;
                    }

                    var dividends = db.Dividends.Where(p => p.ActiveId == valueActive.ActiveId && p.Date >= DateOnly.FromDateTime(DateTime.Today)).OrderBy(p => p.Date).ToList();
                    Console.WriteLine($"\nOPTION = {option.OptionId}; Dividents count = {dividends.Count}\n");

                    double[] dividendAmounts = new double[dividends.Count];
                    double[] dividendTimes = new double[dividends.Count];

                    for (int i = 0; i < dividends.Count; i++)
                    {
                        dividendAmounts[i] = (double)dividends[i].DividendAmount;
                        dividendTimes[i] = CalculateTimeExpiration(dividends[i].Date);
                    }

                    CalculatingFairPriceOfAmericanOption optionAmerican = new CalculatingFairPriceOfAmericanOption((double)valueActive.Price, (double)option.Strike,
                        (double)valueActive.RiskFreeRate, CalculateTimeExpiration(option.ExpirationDate), (double)option.PredefinedIvForCall, dividendAmounts, dividendTimes);

                    PrintAmerica(optionAmerican);
                    Console.WriteLine("\n---------\n");

                    using (var dbc = new CalculationsOnOptionsContext())
                    {
                        var recurringRecord = dbc.Results
                            .Where(p => p.OptionName == option.OptionId &&
                                        Math.Round(Convert.ToDecimal(optionAmerican.PriceOptionCall), 5) == p.CalculationCallOptionPrice &&
                                        Math.Round(Convert.ToDecimal(optionAmerican.GreeksValue.DeltaOptionCall), 5) == p.CalculationDeltaForCallOption &&
                                        Math.Round(Convert.ToDecimal(optionAmerican.GreeksValue.GammaOptionCall), 5) == p.CalculationGammaForCallOption &&
                                        Math.Round(Convert.ToDecimal(optionAmerican.GreeksValue.VegaOptionCall), 5) == p.CalculationVegaForCallOption &&
                                        Math.Round(Convert.ToDecimal(optionAmerican.GreeksValue.ThetaOptionCall), 5) == p.CalculationThetaForCallOption &&
                                        Math.Round(Convert.ToDecimal(optionAmerican.GreeksValue.RhoOptionCall), 5) == p.CalculationRhoForCallOption)
                            .FirstOrDefault();

                        if (recurringRecord != null)
                        {
                            Console.WriteLine("Запись с идентичными параметрами существует");
                        }
                        else
                        {
                            Result updatingOption = new Result();
                            long maxId = dbc.Results.Any() ? dbc.Results.Max(r => r.ResultId) : 0;
                            updatingOption.ResultId = maxId + 1; updatingOption.OptionName = option.OptionId;
                            updatingOption.CalculationCallOptionPrice = Math.Round(Convert.ToDecimal(optionAmerican.PriceOptionCall), 5);
                            updatingOption.CalculationPutOptionPrice = null;
                            updatingOption.CalculationDeltaForCallOption = Math.Round(Convert.ToDecimal(optionAmerican.GreeksValue.DeltaOptionCall), 5);
                            updatingOption.CalculationDeltaForPutOption = null;
                            updatingOption.CalculationGammaForCallOption = Math.Round(Convert.ToDecimal(optionAmerican.GreeksValue.GammaOptionCall), 5);
                            updatingOption.CalculationGammaForPutOption = null;
                            updatingOption.CalculationVegaForCallOption = Math.Round(Convert.ToDecimal(optionAmerican.GreeksValue.VegaOptionCall), 5);
                            updatingOption.CalculationVegaForPutOption = null;
                            updatingOption.CalculationThetaForCallOption = Math.Round(Convert.ToDecimal(optionAmerican.GreeksValue.ThetaOptionCall), 5);
                            updatingOption.CalculationThetaForPutOption = null;
                            updatingOption.CalculationRhoForCallOption = Math.Round(Convert.ToDecimal(optionAmerican.GreeksValue.RhoOptionCall), 5);
                            updatingOption.CalculationRhoForPutOption = null;
                            updatingOption.DateOfFixation = DateTime.Now;

                            dbc.Results.Add(updatingOption);
                            dbc.SaveChanges();
                            Console.WriteLine($"Запись {updatingOption.ResultId} добавлена");
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Метод вычисления срока действия опциона (время до экспирации)
        /// </summary>
        /// <param name="expirationDate">Дата экспирации</param>
        /// <returns></returns>
        private static double CalculateTimeExpiration(DateOnly expirationDate)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            int daysRemaining = expirationDate.DayNumber - today.DayNumber;
            double optionValidityPeriod = daysRemaining / 252.0;

            return optionValidityPeriod;
        }
        /// <summary>
        /// Метод отображения данных о европейских опционах
        /// </summary>
        /// <param name="newOption">Европейский опцион</param>
        private static void PrintEurop(CalculatingFairPriceOfEuropeanOption newOption)
        {
            Console.WriteLine("\nS = " + newOption.CurrentPriceOfUnderlyingAsset);
            Console.WriteLine("K = " + newOption.Strike);
            Console.WriteLine("r = " + newOption.RiskFreeInterestRate);
            Console.WriteLine("T = " + newOption.TimeToOptioneExpiration);
            Console.WriteLine("o` = " + newOption.Volatility);

            Console.WriteLine("\nC = " + newOption.PriceOptionCall);
            Console.WriteLine("P = " + newOption.PriceOptionPut);

            Console.WriteLine("\nDelta C = " + newOption.GreeksValue.DeltaOptionCall);
            Console.WriteLine("Delta P = " + newOption.GreeksValue.DeltaOptionPut);
            Console.WriteLine("Gamma C = " + newOption.GreeksValue.GammaOptionCall);
            Console.WriteLine("Gamma P = " + newOption.GreeksValue.GammaOptionPut);
            Console.WriteLine("Vega C = " + newOption.GreeksValue.VegaOptionCall);
            Console.WriteLine("Vega P = " + newOption.GreeksValue.VegaOptionPut);
            Console.WriteLine("Teta C = " + newOption.GreeksValue.ThetaOptionCall);
            Console.WriteLine("Teta P = " + newOption.GreeksValue.ThetaOptionPut);
            Console.WriteLine("Ro C = " + newOption.GreeksValue.RhoOptionCall);
            Console.WriteLine("Ro P = " + newOption.GreeksValue.RhoOptionPut);
        }
        /// <summary>
        /// Метод отображения данных об американских опционах
        /// </summary>
        /// <param name="newOption">Американский опцион</param>
        private static void PrintAmerica(CalculatingFairPriceOfAmericanOption newOption)
        {
            Console.WriteLine("\nS = " + newOption.CurrentPriceOfUnderlyingAsset);
            Console.WriteLine("K = " + newOption.Strike);
            Console.WriteLine("r = " + newOption.RiskFreeInterestRate);
            Console.WriteLine("T = " + newOption.TimeToOptioneExpiration);
            Console.WriteLine("o` = " + newOption.Volatility);

            Console.WriteLine("\nРазмеры дивидендов = [" + string.Join(", ", newOption.Dividends) + "]");
            Console.WriteLine("Сроки до выплаты = [" + string.Join(", ", newOption.DividendTimes) + "]");

            Console.WriteLine("\nC = " + newOption.PriceOptionCall);

            Console.WriteLine("\nDelta C = " + newOption.GreeksValue.DeltaOptionCall);
            Console.WriteLine("Gamma = " + newOption.GreeksValue.GammaOptionCall);
            Console.WriteLine("Vega = " + newOption.GreeksValue.VegaOptionCall);
            Console.WriteLine("Teta C = " + newOption.GreeksValue.ThetaOptionCall);
            Console.WriteLine("Ro C = " + newOption.GreeksValue.RhoOptionCall);
        }
    }
}
