namespace PlannerCRM.Shared.Attributes;

public class BirthDayRangeAttribute : ValidationAttribute
{
    private readonly int _minimum;
    private readonly int _maximum;

    public BirthDayRangeAttribute(int minimum, int maximum)
    {
        _minimum = minimum;
        _maximum = maximum;
    }

    public BirthDayRangeAttribute()
    { }

    public override bool IsValid(object value)
    {
        if (value is null) return false;

        if (value.GetType() == typeof(DateTime))
        {
            var date = Convert.ToDateTime(value);
            var totalYears = CURRENT_DATE.Year - date.Year;

            return (totalYears >= _minimum) && (totalYears <= _maximum) && (date <= CURRENT_DATE);
        }

        return false;
    }
}