namespace PlannerCRM.Shared.Attributes;

public class MinimumHourlyRateAttribute : ValidationAttribute
{
    private readonly int _minimumHourlyRate;

    public MinimumHourlyRateAttribute(int minimumHourlyRate)
    {
        _minimumHourlyRate = minimumHourlyRate;
    }

    public MinimumHourlyRateAttribute()
    { }

    public override bool IsValid(object value)
    {
        if (value is null) return false;

        if (value.GetType() == typeof(decimal))
        {
            return (decimal)value >= _minimumHourlyRate;
        }

        return false;
    }
}