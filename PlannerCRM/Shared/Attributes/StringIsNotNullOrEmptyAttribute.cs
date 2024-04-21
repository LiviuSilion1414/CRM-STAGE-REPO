namespace PlannerCRM.Shared.Attributes;

public class string IsNotNullOrEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
{
    return !string.IsNullOrEmpty(value as string);
}
}