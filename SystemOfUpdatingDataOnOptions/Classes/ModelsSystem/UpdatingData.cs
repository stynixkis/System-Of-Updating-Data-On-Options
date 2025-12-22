using log4net;
using Module.Black_Shoals.Classes;
using SystemOfUpdatingDataOnOptions.Classes.ModelsDBCalculationsOnOptions;
using SystemOfUpdatingDataOnOptions.Classes.ModelsDBFinancialOptionsSystem;

namespace SystemOfUpdatingDataOnOptions.Classes.ModelsSystem
{
    
    /// <summary>
    /// Класс обновления данных из базы данных по таймеру
    /// </summary>
    public static class UpdatingData
    {
        /// <summary>
        /// Экземпляр класса журнала логов
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
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
            Logging.ConfigureFileLogging();
            try
            {
                log.Info($"************************** НАЧАЛО ОБНОВЛЕНИЯ ДАННЫХ: {DateTime.Now} **************************");
                Console.WriteLine($"Начало обновления данных: {DateTime.Now}");
                UpdatingDataOnEuropeanOption();
                UpdatingDataOnAmericanOption();
                Console.WriteLine($"Обновление завершено: {DateTime.Now}");
                log.Info($"************************** КОНЕЦ ОБНОВЛЕНИЯ ДАННЫХ: {DateTime.Now} **************************");
            }
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при обновлении данных: {ex.Message}");

				Console.WriteLine("=== ПОЛНАЯ ИНФОРМАЦИЯ ОБ ОШИБКЕ ===");
				Console.WriteLine($"Сообщение: {ex.Message}");
				Console.WriteLine($"Тип исключения: {ex.GetType().Name}");
				Console.WriteLine($"StackTrace: {ex.StackTrace}");

				Console.WriteLine("=== КОНЕЦ ИНФОРМАЦИИ ОБ ОШИБКЕ ===");
                log.Error( ex );
			}
		}
        /// <summary>
        /// Обновление данных европейских опционов
        /// </summary>
        private static void UpdatingDataOnEuropeanOption()
        {
            Console.WriteLine("\nЕВРОПА\n");
            log.Info("Начало обработки европейских опционов");

            try
            {
                using (var db = new FinancialOptionsSystemContext())
                {
                    var europeanActiveIds = db.Actives
                        .Where(a => a.ActiveStyle.ToLower() == "european")
                        .Select(a => a.ActiveId)
                        .ToList();

                    if (!europeanActiveIds.Any())
                    {
                        Console.WriteLine("Нет европейских активов для обработки");
                        log.Warn("Нет европейских активов для обработки");
                        return;
                    }

                    var optionsEuropean = db.Options
                        .Where(o => europeanActiveIds.Contains(o.ActiveId)
                            && o.ExpirationDate >= DateOnly.FromDateTime(DateTime.Today))
                        .Distinct()
                        .ToList();

                    foreach (var option in optionsEuropean)
                    {
                        try
                        {
                            var valueActive = db.Actives.Where(p => p.ActiveId == option.ActiveId).FirstOrDefault();

                            if (valueActive == null)
                            {
                                Console.WriteLine($"Нет актива для опциона {option.OptionId}");
                                log.Warn($"Нет актива для опциона {option.OptionId}");
                                continue;
                            }

                            log.Info($"====================== ID option = {option.OptionId} ======================");

                            CalculatingFairPriceOfEuropeanOption optionEuropean = new CalculatingFairPriceOfEuropeanOption(
                                (double)valueActive.Price,
                                (double)option.Strike,
                                (double)valueActive.RiskFreeRate,
                                CalculateTimeExpiration(option.ExpirationDate),
                                (double)option.PredefinedIvForCall,
                                (double)option.PredefinedIvForPut);

                            Console.WriteLine($"\nOPTION = {option.OptionId}");
                            PrintEurop(optionEuropean);
                            Console.WriteLine("\n---------\n");

                            using (var dbc = new CalculationsOnOptionsContext())
                            {
                                try
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
                                        Console.WriteLine($"Запись для опциона {option.OptionId} с идентичными параметрами существует");
                                        log.Info($"Запись для опциона {option.OptionId} с идентичными параметрами существует");
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
                                        Console.WriteLine($"Запись {updatingOption.ResultId} для опциона {option.OptionId} добавлена");
                                        log.Info($"Запись {updatingOption.ResultId} для опциона {option.OptionId} добавлена");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Ошибка при работе с БД результатов для опциона {option.OptionId}: {ex.Message}");
                                    log.Error($"Ошибка при работе с БД результатов для опциона {option.OptionId}", ex);
                                    continue;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при обработке опциона {option.OptionId}: {ex.Message}");
                            log.Error($"Ошибка при обработке опциона {option.OptionId}", ex);
                            continue; 
                        }

                        log.Info("=======================================================");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка при обработке европейских опционов: {ex.Message}");
                log.Error("Критическая ошибка при обработке европейских опционов", ex);
            }
            finally
            {
                log.Info("Завершение обработки европейских опционов");
            }
        }
        /// <summary>
        /// Обновление данных американских опционов
        /// </summary>
        private static void UpdatingDataOnAmericanOption()
        {
            Console.WriteLine("\nАМЕРИКА\n");
            log.Info("Начало обработки американских опционов");

            try
            {
                using (var db = new FinancialOptionsSystemContext())
                {
                    var americanActiveIds = db.Actives
                        .Where(a => a.ActiveStyle.ToLower() == "american")
                        .Select(a => a.ActiveId)
                        .ToList();

                    if (!americanActiveIds.Any())
                    {
                        Console.WriteLine("Нет американских активов для обработки");
                        log.Warn("Нет американских активов для обработки");
                        return;
                    }

                    var optionsAmerican = db.Options
                        .Where(o => americanActiveIds.Contains(o.ActiveId)
                            && o.ExpirationDate >= DateOnly.FromDateTime(DateTime.Today))
                        .Distinct()
                        .ToList();

                    foreach (var option in optionsAmerican)
                    {
                        try
                        {
                            var valueActive = db.Actives.Where(p => p.ActiveId == option.ActiveId).FirstOrDefault();

                            if (valueActive == null)
                            {
                                Console.WriteLine($"Нет актива для опциона {option.OptionId}");
                                log.Warn($"Нет актива для опциона {option.OptionId}");
                                continue;
                            }

                            try
                            {
                                var dividends = db.Dividends
                                    .Where(p => p.ActiveId == valueActive.ActiveId && p.Date >= DateOnly.FromDateTime(DateTime.Today))
                                    .OrderBy(p => p.Date)
                                    .ToList();

                                Console.WriteLine($"\nOPTION = {option.OptionId}; Dividents count = {dividends.Count}\n");

                                double[] dividendAmounts = new double[dividends.Count];
                                double[] dividendTimes = new double[dividends.Count];

                                for (int i = 0; i < dividends.Count; i++)
                                {
                                    dividendAmounts[i] = (double)dividends[i].DividendAmount;
                                    dividendTimes[i] = CalculateTimeExpiration(dividends[i].Date);
                                }

                                log.Info($"====================== ID option = {option.OptionId} ======================");

                                CalculatingFairPriceOfAmericanOption optionAmerican = new CalculatingFairPriceOfAmericanOption(
                                    (double)valueActive.Price,
                                    (double)option.Strike,
                                    (double)valueActive.RiskFreeRate,
                                    CalculateTimeExpiration(option.ExpirationDate),
                                    (double)option.PredefinedIvForCall,
                                    dividendAmounts,
                                    dividendTimes);

                                PrintAmerica(optionAmerican);
                                Console.WriteLine("\n---------\n");

                                using (var dbc = new CalculationsOnOptionsContext())
                                {
                                    try
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
                                            Console.WriteLine($"Запись для опциона {option.OptionId} с идентичными параметрами существует");
                                            log.Info($"Запись для опциона {option.OptionId} с идентичными параметрами существует");
                                        }
                                        else
                                        {
                                            Result updatingOption = new Result();
                                            long maxId = dbc.Results.Any() ? dbc.Results.Max(r => r.ResultId) : 0;
                                            updatingOption.ResultId = maxId + 1;
                                            updatingOption.OptionName = option.OptionId;
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
                                            Console.WriteLine($"Запись {updatingOption.ResultId} для опциона {option.OptionId} добавлена");
                                            log.Info($"Запись {updatingOption.ResultId} для опциона {option.OptionId} добавлена");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Ошибка при работе с БД результатов для опциона {option.OptionId}: {ex.Message}");
                                        log.Error($"Ошибка при работе с БД результатов для опциона {option.OptionId}", ex);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка при расчёте опциона {option.OptionId}: {ex.Message}");
                                log.Error($"Ошибка при расчёте опциона {option.OptionId}", ex);
                                continue;
                            }

                            log.Info("=======================================================");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Общая ошибка при обработке опциона {option.OptionId}: {ex.Message}");
                            log.Error($"Общая ошибка при обработке опциона {option.OptionId}", ex);
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка при обработке американских опционов: {ex.Message}");
                log.Error("Критическая ошибка при обработке американских опционов", ex);
            }
            finally
            {
                log.Info("Завершение обработки американских опционов");
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
            log.Info("S = " + newOption.CurrentPriceOfUnderlyingAsset);
            Console.WriteLine("K = " + newOption.Strike);
            log.Info("K = " + newOption.Strike);
            Console.WriteLine("r = " + newOption.RiskFreeInterestRate);
            log.Info("r = " + newOption.RiskFreeInterestRate);
            Console.WriteLine("T = " + newOption.TimeToOptioneExpiration);
            log.Info("T = " + newOption.TimeToOptioneExpiration);
            Console.WriteLine("o` = " + newOption.Volatility);
            log.Info("o` = " + newOption.Volatility);

            Console.WriteLine("\nC = " + newOption.PriceOptionCall);
            log.Info("C = " + newOption.PriceOptionCall);
            Console.WriteLine("P = " + newOption.PriceOptionPut);
            log.Info("P = " + newOption.PriceOptionPut);

            Console.WriteLine("\nDelta C = " + newOption.GreeksValue.DeltaOptionCall);
            log.Info("Delta C = " + newOption.GreeksValue.DeltaOptionCall);
            Console.WriteLine("Delta P = " + newOption.GreeksValue.DeltaOptionPut);
            log.Info("Delta P = " + newOption.GreeksValue.DeltaOptionPut);
            Console.WriteLine("Gamma C = " + newOption.GreeksValue.GammaOptionCall);
            log.Info("Gamma C = " + newOption.GreeksValue.GammaOptionCall);
            Console.WriteLine("Gamma P = " + newOption.GreeksValue.GammaOptionPut);
            log.Info("Gamma P = " + newOption.GreeksValue.GammaOptionPut);
            Console.WriteLine("Vega C = " + newOption.GreeksValue.VegaOptionCall);
            log.Info("Vega C = " + newOption.GreeksValue.VegaOptionCall);
            Console.WriteLine("Vega P = " + newOption.GreeksValue.VegaOptionPut);
            log.Info("Vega P = " + newOption.GreeksValue.VegaOptionPut);
            Console.WriteLine("Teta C = " + newOption.GreeksValue.ThetaOptionCall);
            log.Info("Teta C = " + newOption.GreeksValue.ThetaOptionCall);
            Console.WriteLine("Teta P = " + newOption.GreeksValue.ThetaOptionPut);
            log.Info("Teta P = " + newOption.GreeksValue.ThetaOptionPut);
            Console.WriteLine("Ro C = " + newOption.GreeksValue.RhoOptionCall);
            log.Info("Ro C = " + newOption.GreeksValue.RhoOptionCall);
            Console.WriteLine("Ro P = " + newOption.GreeksValue.RhoOptionPut);
            log.Info("Ro P = " + newOption.GreeksValue.RhoOptionPut);
        }
        /// <summary>
        /// Метод отображения данных об американских опционах
        /// </summary>
        /// <param name="newOption">Американский опцион</param>
        private static void PrintAmerica(CalculatingFairPriceOfAmericanOption newOption)
        {
            Console.WriteLine("\nS = " + newOption.CurrentPriceOfUnderlyingAsset);
            log.Info("S = " + newOption.CurrentPriceOfUnderlyingAsset);
            Console.WriteLine("K = " + newOption.Strike);
            log.Info("K = " + newOption.Strike);
            Console.WriteLine("r = " + newOption.RiskFreeInterestRate);
            log.Info("r = " + newOption.RiskFreeInterestRate);
            Console.WriteLine("T = " + newOption.TimeToOptioneExpiration);
            log.Info("T = " + newOption.TimeToOptioneExpiration);
            Console.WriteLine("o` = " + newOption.Volatility);
            log.Info("o` = " + newOption.Volatility);

            Console.WriteLine("\nРазмеры дивидендов = [" + string.Join(", ", newOption.Dividends) + "]");
            log.Info("Размеры дивидендов = [" + string.Join(", ", newOption.Dividends) + "]");
            Console.WriteLine("Сроки до выплаты = [" + string.Join(", ", newOption.DividendTimes) + "]");
            log.Info("Сроки до выплаты = [" + string.Join(", ", newOption.DividendTimes) + "]");

            Console.WriteLine("\nC = " + newOption.PriceOptionCall);
            log.Info("C = " + newOption.PriceOptionCall);

            Console.WriteLine("\nDelta C = " + newOption.GreeksValue.DeltaOptionCall);
            log.Info("Delta C = " + newOption.GreeksValue.DeltaOptionCall);
            Console.WriteLine("Gamma = " + newOption.GreeksValue.GammaOptionCall);
            log.Info("Gamma = " + newOption.GreeksValue.GammaOptionCall);
            Console.WriteLine("Vega = " + newOption.GreeksValue.VegaOptionCall);
            log.Info("Vega = " + newOption.GreeksValue.VegaOptionCall);
            Console.WriteLine("Teta C = " + newOption.GreeksValue.ThetaOptionCall);
            log.Info("Teta C = " + newOption.GreeksValue.ThetaOptionCall);
            Console.WriteLine("Ro C = " + newOption.GreeksValue.RhoOptionCall);
            log.Info("Ro C = " + newOption.GreeksValue.RhoOptionCall);
        }
    }
}
