namespace SystemOfUpdatingDataOnOptions.Classes.ModelsDBFinancialOptionsSystem;

public partial class Active
{
    public long ActiveId { get; set; }

    public int Region { get; set; }

    public decimal Price { get; set; }

    public decimal VolumeOfDividends { get; set; }

    public int CalendarFrequency { get; set; }

    public decimal ProfitabilityOfDividends { get; set; }

    public string TypeOfActive { get; set; } = null!;

    public string ActiveStyle { get; set; } = null!;

    public string ActiveCurrency { get; set; } = null!;

    public decimal RiskFreeRate { get; set; }

    public virtual ICollection<Dividend> Dividends { get; set; } = new List<Dividend>();

    public virtual ICollection<Option> Options { get; set; } = new List<Option>();

    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}
