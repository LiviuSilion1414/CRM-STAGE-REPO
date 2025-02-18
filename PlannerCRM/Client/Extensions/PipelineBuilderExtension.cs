﻿using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace PlannerCRM.Client.Extensions;

public static class PipelineBuilderExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {   
        services.AddSingleton<LocalStorageService>();

        services.AddScoped<AuthenticationStateProvider, AuthService>();
        services.AddScoped<AuthService>();
        services.AddScoped<AuthService>();

        services.AddScoped<DialogService>();
        services.AddScoped<NotificationService>();
        services.AddScoped<TooltipService>();
        services.AddScoped<ContextMenuService>();

        services.AddScoped<FetchService<ActivityDto>>();
        services.AddScoped<FetchService<EmployeeDto>>();
        services.AddScoped<FetchService<FirmClientDto>>();
        services.AddScoped<FetchService<RoleDto>, FetchService<RoleDto>>();
        services.AddScoped<FetchService<SalaryDto>>();
        services.AddScoped<FetchService<WorkOrderDto>>();
        services.AddScoped<FetchService<WorkOrderCostDto>>();
        services.AddScoped<FetchService<WorkTimeDto>>();
        services.AddScoped<FetchService<EmployeeLoginRecoveryDto>>();

        return services;
    }
}
