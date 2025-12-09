namespace SystemOfUpdatingDataOnOptions.Classes.ModelsDBCalculationsOnOptions;

public partial class Result
{
    public long ResultId { get; set; }

    public long OptionName { get; set; }

    public DateTime DateOfFixation { get; set; }

    public decimal CalculationCallOptionPrice { get; set; }

    public decimal? CalculationPutOptionPrice { get; set; }

    public decimal CalculationDeltaForCallOption { get; set; }

    public decimal? CalculationDeltaForPutOption { get; set; }

    public decimal CalculationGammaForCallOption { get; set; }

    public decimal? CalculationGammaForPutOption { get; set; }

    public decimal CalculationThetaForCallOption { get; set; }

    public decimal? CalculationThetaForPutOption { get; set; }

    public decimal CalculationVegaForCallOption { get; set; }

    public decimal? CalculationVegaForPutOption { get; set; }

    public decimal CalculationRhoForCallOption { get; set; }

    public decimal? CalculationRhoForPutOption { get; set; }
}
