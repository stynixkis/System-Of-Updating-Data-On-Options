using Module.Black_Shoals.Classes;

CalculatingFairPriceOfEuropeanOption newOption1 = new CalculatingFairPriceOfEuropeanOption(40, 40, 0.1, 0.5, 0.3);
PrintEurop(newOption1);

Console.WriteLine("\n-------------\n");

CalculatingFairPriceOfAmericanOption newOption = new CalculatingFairPriceOfAmericanOption(40, 40, 0.1, 0.5, 0.3, [0.7], [(2.0 / 12)]);
PrintAmerica(newOption);

Console.WriteLine("\n-------------\n");

CalculatingFairPriceOfEuropeanOption newOption2 = new CalculatingFairPriceOfEuropeanOption(40, 40, 0.1, 0.5, 0.3, 0.243);
PrintEurop(newOption2);

void PrintEurop(CalculatingFairPriceOfEuropeanOption newOption)
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

void PrintAmerica(CalculatingFairPriceOfAmericanOption newOption)
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