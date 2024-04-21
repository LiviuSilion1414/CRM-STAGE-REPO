namespace PlannerCRM.Shared.Attributes;

public class WorkOrderFinishDateRangeAttribute : ValidationAttribute
{
    private readonly int _minimum;
    private readonly int _maximum;

    public WorkOrderFinishDateRangeAttribute(int minimum, int maximum)
    {
        _minimum = minimum;
        _maximum = maximum;
    }

    public WorkOrderFinishDateRangeAttribute()
    { }

    public override bool IsValid(object value)
    {
        if (value is null) return false;

        if (value.GetType() == typeof(DateTime))
        {
            var date = Convert.ToDateTime(value);
            if (date > CURRENT_DATE)
            {
                if ((date.Month >= _minimum) && (date.Month <= _maximum))
                {
                    return true;
                }
            }
        }

        return false;
    }
}