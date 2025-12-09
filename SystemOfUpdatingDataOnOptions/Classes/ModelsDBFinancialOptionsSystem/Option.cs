namespace SystemOfUpdatingDataOnOptions.Classes.ModelsDBFinancialOptionsSystem;

public partial class Option
{
    public long OptionId { get; set; }

    public long OptionCallId { get; set; }

    public long OptionPutId { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public decimal Strike { get; set; }

    public decimal OptionCallPrice { get; set; }

    public decimal OptionPutPrice { get; set; }

    public long AtmPosition { get; set; }

    public decimal PredefinedIvForCall { get; set; }

    public decimal PredefinedIvForPut { get; set; }

    public decimal ExpectedDividendYield { get; set; }

    public decimal EstimatedAssetValue { get; set; }

    public decimal CallBid { get; set; }

    public decimal CallAsk { get; set; }

    public decimal PutBid { get; set; }

    public decimal PutAsk { get; set; }

    public int QuarterlyOptionFlag { get; set; }

    public long ActiveId { get; set; }

    public virtual Active Active { get; set; } = null!;
}
