namespace PlannerCRM.Server.DataAccess;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<IdentityUser>().ToTable(nameof(Employees));
        modelBuilder.Entity<IdentityRole>().ToTable(nameof(EmployeeRoles));
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable(nameof(EmployeeWithRoles));
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable(nameof(EmployeeRoleClaims));
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable(nameof(EmployeeClaims));
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable(nameof(EmployeeLogins));
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable(nameof(EmployeeTokens));

        modelBuilder
            .Entity<Employee>()
            .Property(em => em.Id)
            .ValueGeneratedOnAdd();

        modelBuilder
            .Entity<EmployeeSalary>()
            .Property(em => em.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<EmployeeActivity>()
            .HasOne(e => e.Employee)
            .WithMany(ea => ea.EmployeeActivity)
            .HasForeignKey(ei => ei.EmployeeId);

        modelBuilder.Entity<EmployeeActivity>()
            .HasOne(a => a.Activity)
            .WithMany(ea => ea.EmployeeActivity)
            .HasForeignKey(ai => ai.ActivityId);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeRole> EmployeeRoles { get; set; }
    public DbSet<EmployeeWithRole> EmployeeWithRoles { get; set; }
    public DbSet<EmployeeRoleClaim> EmployeeRoleClaims { get; set; }
    public DbSet<EmployeeClaim> EmployeeClaims { get; set; }
    public DbSet<EmployeeLogin> EmployeeLogins { get; set; }
    public DbSet<EmployeeToken> EmployeeTokens { get; set; }

    public DbSet<EmployeeProfilePicture> ProfilePictures { get; set; }
    public DbSet<FirmClient> Clients { get; set; }
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<EmployeeActivity> EmployeeActivity { get; set; }

    public DbSet<WorkOrderCost> WorkOrderCosts { get; set; }
    public DbSet<WorkTimeRecord> WorkTimeRecords { get; set; }
    public DbSet<ClientWorkOrder> ClientWorkOrders { get; set; }
}