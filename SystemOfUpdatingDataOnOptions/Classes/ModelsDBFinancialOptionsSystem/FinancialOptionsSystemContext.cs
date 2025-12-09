using Microsoft.EntityFrameworkCore;

namespace SystemOfUpdatingDataOnOptions.Classes.ModelsDBFinancialOptionsSystem;

public partial class FinancialOptionsSystemContext : DbContext
{
    public FinancialOptionsSystemContext()
    {
    }

    public FinancialOptionsSystemContext(DbContextOptions<FinancialOptionsSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Active> Actives { get; set; }

    public virtual DbSet<Dividend> Dividends { get; set; }

    public virtual DbSet<Option> Options { get; set; }

    public virtual DbSet<Output> Outputs { get; set; }

    public virtual DbSet<Quote> Quotes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-U6LTUKT\\SQLEXPRESS;Initial Catalog=FinancialOptionsSystem;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Active>(entity =>
        {
            entity.HasKey(e => e.ActiveId).HasName("PK__Active__DDEB5331C34358F8");

            entity.ToTable("Active");

            entity.HasIndex(e => e.ActiveCurrency, "IX_Active_Currency");

            entity.HasIndex(e => e.Region, "IX_Active_Region");

            entity.HasIndex(e => e.TypeOfActive, "IX_Active_Type");

            entity.Property(e => e.ActiveId).HasColumnName("Active_ID");
            entity.Property(e => e.ActiveCurrency)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Active_currency");
            entity.Property(e => e.ActiveStyle)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Active_style");
            entity.Property(e => e.CalendarFrequency).HasColumnName("Calendar_frequency");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.ProfitabilityOfDividends)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("Profitability_of_dividends");
            entity.Property(e => e.RiskFreeRate).HasColumnType("decimal(10, 6)");
            entity.Property(e => e.TypeOfActive)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Type_of_active");
            entity.Property(e => e.VolumeOfDividends)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Volume_of_dividends");
        });

        modelBuilder.Entity<Dividend>(entity =>
        {
            entity.HasKey(e => e.DividendId).HasName("PK__Dividend__5707EF1E8F262827");

            entity.ToTable("Dividend");

            entity.HasIndex(e => e.ActiveId, "IX_Dividend_ActiveID");

            entity.HasIndex(e => new { e.ActiveId, e.Date }, "IX_Dividend_ActiveID_Date");

            entity.HasIndex(e => e.Date, "IX_Dividend_Date");

            entity.Property(e => e.DividendId).HasColumnName("Dividend_ID");
            entity.Property(e => e.ActiveId).HasColumnName("Active_ID");
            entity.Property(e => e.DividendAmount)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Dividend_amount");

            entity.HasOne(d => d.Active).WithMany(p => p.Dividends)
                .HasForeignKey(d => d.ActiveId)
                .HasConstraintName("FK_Dividend_Active");
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__Option__3260905E7A780536");

            entity.ToTable("Option");

            entity.HasIndex(e => e.ActiveId, "IX_Option_ActiveID");

            entity.HasIndex(e => new { e.ActiveId, e.ExpirationDate }, "IX_Option_ActiveID_Expiration");

            entity.HasIndex(e => e.OptionCallId, "IX_Option_CallID");

            entity.HasIndex(e => e.ExpirationDate, "IX_Option_ExpirationDate");

            entity.HasIndex(e => e.OptionPutId, "IX_Option_PutID");

            entity.HasIndex(e => e.Strike, "IX_Option_Strike");

            entity.Property(e => e.OptionId).HasColumnName("Option_ID");
            entity.Property(e => e.ActiveId).HasColumnName("Active_ID");
            entity.Property(e => e.AtmPosition).HasColumnName("ATM_position");
            entity.Property(e => e.CallAsk)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Call_Ask");
            entity.Property(e => e.CallBid)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Call_Bid");
            entity.Property(e => e.EstimatedAssetValue)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Estimated_asset_value");
            entity.Property(e => e.ExpectedDividendYield)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("Expected_dividend_yield");
            entity.Property(e => e.ExpirationDate).HasColumnName("Expiration_date");
            entity.Property(e => e.OptionCallId).HasColumnName("Option_Call_ID");
            entity.Property(e => e.OptionCallPrice)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Option_call_price");
            entity.Property(e => e.OptionPutId).HasColumnName("Option_Put_ID");
            entity.Property(e => e.OptionPutPrice)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Option_put_price");
            entity.Property(e => e.PredefinedIvForCall)
                .HasColumnType("decimal(10, 6)")
                .HasColumnName("Predefined_IV_for_call");
            entity.Property(e => e.PredefinedIvForPut)
                .HasColumnType("decimal(10, 6)")
                .HasColumnName("Predefined_IV_for_put");
            entity.Property(e => e.PutAsk)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Put_Ask");
            entity.Property(e => e.PutBid)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Put_Bid");
            entity.Property(e => e.QuarterlyOptionFlag).HasColumnName("Quarterly_option_flag");
            entity.Property(e => e.Strike).HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.Active).WithMany(p => p.Options)
                .HasForeignKey(d => d.ActiveId)
                .HasConstraintName("FK_Option_Active");
        });

        modelBuilder.Entity<Output>(entity =>
        {
            entity.HasKey(e => e.OutputId).HasName("PK__Output__7663A4D61101C581");

            entity.ToTable("Output");

            entity.HasIndex(e => e.Date, "IX_Output_Date");

            entity.HasIndex(e => new { e.Date, e.Region }, "IX_Output_Date_Region");

            entity.HasIndex(e => e.Region, "IX_Output_Region");

            entity.Property(e => e.OutputId).HasColumnName("Output_ID");
        });

        modelBuilder.Entity<Quote>(entity =>
        {
            entity.HasKey(e => e.QuoteId).HasName("PK__Quote__D2709B791FB21073");

            entity.ToTable("Quote");

            entity.HasIndex(e => e.ActiveId, "IX_Quote_ActiveID");

            entity.HasIndex(e => new { e.ActiveId, e.Day }, "IX_Quote_ActiveID_Day");

            entity.HasIndex(e => e.Day, "IX_Quote_Day");

            entity.Property(e => e.QuoteId).HasColumnName("Quote_ID");
            entity.Property(e => e.ActiveId).HasColumnName("Active_ID");
            entity.Property(e => e.Currency)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Quote1)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Quote");

            entity.HasOne(d => d.Active).WithMany(p => p.Quotes)
                .HasForeignKey(d => d.ActiveId)
                .HasConstraintName("FK_Quote_Active");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
