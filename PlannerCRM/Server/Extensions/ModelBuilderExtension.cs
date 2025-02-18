﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Server.Extensions;

public static class ModelBuilderExtensions
{

    public static void ApplyUtcDateTimeConversion(this ModelBuilder modelBuilder)
    {
        var utcConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(), //when saving
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc) //when reading
        );

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var dateTimeProperties = entityType.ClrType
                .GetProperties()
                .Where(p => p.PropertyType == typeof(DateTime));

            foreach (var property in dateTimeProperties)
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(property.Name)
                    .HasConversion(utcConverter);
            }
        }
    }


    public static void ConfigureEnums(this ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Roles>();
    }

    public static void ConfigureRelationships(this ModelBuilder modelBuilder)
    {
        ConfigureFirmClientWorkOrders(modelBuilder);
        ConfigureWorkOrderCost(modelBuilder);
        ConfigureFirmClientWorkOrderCosts(modelBuilder);
        ConfigureWorkOrderActivities(modelBuilder);
        ConfigureEmployeeWorkTimes(modelBuilder);
        ConfigureActivitiesWorkTime(modelBuilder);
        ConfigureEmployeeActivities(modelBuilder);
        ConfigureEmployeesRoles(modelBuilder);
        ConfigureEmployeesSalaries(modelBuilder);
        ConfigureEmployeeLogins(modelBuilder);
    }

    private static void ConfigureFirmClientWorkOrders(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FirmClient>()
            .HasMany(c => c.WorkOrders)
            .WithOne(w => w.FirmClient)
            .HasForeignKey(w => w.FirmClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureWorkOrderCost(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkOrder>()
            .HasOne(w => w.WorkOrderCost)
            .WithOne(c => c.WorkOrder)
            .HasForeignKey<WorkOrderCost>(c => c.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureFirmClientWorkOrderCosts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FirmClient>()
            .HasMany(c => c.WorkOrderCosts)
            .WithOne(wc => wc.FirmClient)
            .HasForeignKey(wc => wc.FirmClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureWorkOrderActivities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkOrder>()
            .HasMany(w => w.Activities)
            .WithOne(a => a.WorkOrder)
            .HasForeignKey(a => a.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureEmployeeWorkTimes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.WorkTimes)
            .WithOne(wt => wt.Employee)
            .HasForeignKey(wt => wt.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureActivitiesWorkTime(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityWorkTime>()
            .HasKey(awt => awt.Id);

        modelBuilder.Entity<ActivityWorkTime>()
            .HasOne(awt => awt.Activity)
            .WithMany(a => a.ActivityWorkTimes)
            .HasForeignKey(awt => awt.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ActivityWorkTime>()
            .HasOne(awt => awt.WorkTime)
            .WithMany(wt => wt.ActivityWorkTimes)
            .HasForeignKey(awt => awt.WorkTimeId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureEmployeeActivities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeActivity>()
            .HasKey(ea => ea.Id);

        modelBuilder.Entity<EmployeeActivity>()
            .HasOne(ea => ea.Employee)
            .WithMany(e => e.EmployeeActivities)
            .HasForeignKey(ea => ea.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmployeeActivity>()
            .HasOne(ea => ea.Activity)
            .WithMany(a => a.EmployeeActivities)
            .HasForeignKey(ea => ea.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureEmployeesRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeRole>()
            .HasKey(er => er.Id);

        modelBuilder.Entity<EmployeeRole>()
            .HasOne(er => er.Employee)
            .WithMany(e => e.EmployeeRoles)
            .HasForeignKey(er => er.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmployeeRole>()
            .HasOne(er => er.Role)
            .WithMany(r => r.EmployeeRoles)
            .HasForeignKey(er => er.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureEmployeesSalaries(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeSalary>()
            .HasKey(es => es.Id);

        modelBuilder.Entity<EmployeeSalary>()
            .HasOne(es => es.Employee)
            .WithMany(e => e.EmployeeSalaries)
            .HasForeignKey(es => es.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmployeeSalary>()
            .HasOne(es => es.Salary)
            .WithMany(s => s.EmployeeSalaries)
            .HasForeignKey(es => es.SalaryId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureEmployeeLogins(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeLoginData>()
            .HasKey(eld => eld.Id);

        modelBuilder.Entity<EmployeeLoginData>()
            .HasOne(eld => eld.Employee)
            .WithMany(e => e.LoginData)
            .HasForeignKey(els => els.EmployeeId);
    }
}