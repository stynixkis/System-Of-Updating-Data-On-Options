namespace SystemOfUpdatingDataOnOptions.Classes.ModelsDBFinancialOptionsSystem;

public partial class Dividend
{
    public long DividendId { get; set; }

    public DateOnly Date { get; set; }

    public decimal DividendAmount { get; set; }

    public long ActiveId { get; set; }

    public virtual Active Active { get; set; } = null!;
}
