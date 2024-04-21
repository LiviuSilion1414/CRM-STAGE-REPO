namespace PlannerCRM.Shared.Constants;

public static class ConstantValues
{
    public static DateTime CURRENT_DATE { get => DateTime.Now; }
    public static int CURRENT_YEAR { get => DateTime.Now.Year; }
    public static int MINIMUM_YEAR { get => DateTime.Now.Year - MINIUM_AGE; }
    public static int MAXIMUM_YEAR { get => DateTime.Now.Year - MAXIMUMUM_AGE; }

    public const int MAX_PERIOD_OF_MONTHS_HOURLY_RATE = 6;
    public const int MINIMUM_HOURLY_RATE = 8;

    public const int MIN_WORKORDER_MONTH_CONTRACT = 3;
    public const int MAX_WORKORDER_MONTH_CONTRACT = 24;

    public const int MIN_ACTIVITY_MONTH_PERIOD = 1;
    public const int MAX_ACTIVITY_MONTH_PERIOD = 6;

    public const string ADMIN_EMAIL = "account.manager@gmail.com";
    public const Roles ADMIN_ROLE = Roles.ACCOUNT_MANAGER;

    public const int MAJOR_AGE = 18;
    public const int MAX_AGE = 55;

    public const int INVALID_ID = -1;
    public const int MINIMUM_MONTH = 1;
    public const int MAXIMUM_MONTH = 6;

    public const int MINIUM_AGE = 18;
    public const int MAXIMUMUM_AGE = 55;

    public const int PASS_MIN_LENGTH = 8;
    public const int PASS_MAX_LENGTH = 16;

    public const int PAGINATION_LIMIT = 5;

    public const string LOGIN_PAGE_SHORT = "/";
    public const string LOGIN_PAGE_LONG = "/login";
    public const string MODIFY = "Modifica";
    public const string CANCEL = "Annulla";
    public const string COLON = ":";

}