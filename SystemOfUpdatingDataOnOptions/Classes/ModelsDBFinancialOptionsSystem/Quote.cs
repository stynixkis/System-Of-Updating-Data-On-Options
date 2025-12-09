namespace SystemOfUpdatingDataOnOptions.Classes.ModelsDBFinancialOptionsSystem;

public partial class Quote
{
    public long QuoteId { get; set; }

    public string Currency { get; set; } = null!;

    public int Day { get; set; }

    public decimal Quote1 { get; set; }

    public long ActiveId { get; set; }

    public virtual Active Active { get; set; } = null!;
}
