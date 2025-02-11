﻿namespace PlannerCRM.Shared.Constants.ApiEndpoints.Routes;

public class WorkOrderEndpointActions
{
    public const string SEARCH_WORKORDER_BY_TITLE = "searchWorkOrderByTitle/{title}";
    public const string FIND_ASSOCIATED_ACTIVITIES_BY_WORKORDERID = "findAssociatedActivitiesByWorkOrderId/{itemId}";
    public const string FIND_ASSOCIATED_WORKORDERS_BY_CLIENTID = "findAssociatedWorkOrdersByClientId/{itemId}";

    public static string SearchWorkOrderByTitle(string query) => 
        $"searchWorkOrderByTitle/{query}";
    public static string FindAssociatedActivitiesByWorkOrderId(int workorderid) => 
        $"findAssociatedActivitiesByWorkOrderId/{workorderid}";
    public static string FindAssociatedWorkordersByClientid(int workorderid) => 
        $"findAssociatedWorkOrdersByClientId/{workorderid}";
}