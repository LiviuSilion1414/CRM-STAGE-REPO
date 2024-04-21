namespace PlannerCRM.Shared.CustomExceptions;

public class UpdateRowSourceException : Exception
{

    public UpdateRowSourceException(string message)
        : base(message)
    { }

}