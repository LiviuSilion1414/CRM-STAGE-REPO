namespace PlannerCRM.Shared.Attributes;

public class IsNotZeroAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        return
            value is not null &&
            int.TryParse(value as string, out var res) &&
            res is not 0;
    }
}