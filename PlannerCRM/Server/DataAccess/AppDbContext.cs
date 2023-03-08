using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Server.Models;

namespace Gestionale.Server.DataAccess;

public class AppDbContext: IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EmployeeActivity>()
        .HasOne(e => e.Employee)
        .WithMany(ea => ea.EmployeeActivity)
        .HasForeignKey(ei => ei.EmployeeId);

        modelBuilder.Entity<EmployeeActivity>()
        .HasOne(a => a.Activity)
        .WithMany(ea => ea.EmployeeActivity)
        .HasForeignKey(ai => ai.ActivityId);
    }
    
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeActivity> EmployeeActivity { get; set; }
    public DbSet<WorkTimeRecord> WorkTimeRecords { get; internal set; }
}