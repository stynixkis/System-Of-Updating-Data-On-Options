using Microsoft.EntityFrameworkCore;

namespace SystemOfUpdatingDataOnOptions.Classes.ModelsDBCalculationsOnOptions;

public partial class CalculationsOnOptionsContext : DbContext
{
    public CalculationsOnOptionsContext()
    {
    }

    public CalculationsOnOptionsContext(DbContextOptions<CalculationsOnOptionsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Result> Results { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-U6LTUKT\\SQLEXPRESS;Initial Catalog=Calculations_on_options;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Result>(entity =>
        {
            entity.ToTable("Result");

            entity.Property(e => e.ResultId)
                .ValueGeneratedNever()
                .HasColumnName("Result_id");
            entity.Property(e => e.CalculationCallOptionPrice)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Calculation_Call_option_price");
            entity.Property(e => e.CalculationDeltaForCallOption)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Calculation_delta_for_Call_option");
            entity.Property(e => e.CalculationDeltaForPutOption)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Calculation_delta_for_Put_option");
            entity.Property(e => e.CalculationGammaForCallOption)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Calculation_gamma_for_Call_option");
            entity.Property(e => e.CalculationGammaForPutOption)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Calculation_gamma_for_Put_option");
            entity.Property(e => e.CalculationPutOptionPrice)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Calculation_Put_option_price");
            entity.Property(e => e.CalculationRhoForCallOption)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Calculation_rho_for_Call_option");
            entity.Property(e => e.CalculationRhoForPutOption)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Calculation_rho_for_Put_option");
            entity.Property(e => e.CalculationThetaForCallOption)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Calculation_theta_for_Call_option");
            entity.Property(e => e.CalculationThetaForPutOption)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Calculation_theta_for_Put_option");
            entity.Property(e => e.CalculationVegaForCallOption)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Calculation_vega_for_Call_option");
            entity.Property(e => e.CalculationVegaForPutOption)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Calculation_vega_for_Put_option");
            entity.Property(e => e.DateOfFixation).HasColumnName("Date_of_fixation");
            entity.Property(e => e.OptionName).HasColumnName("Option_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
