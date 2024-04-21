namespace PlannerCRM.Shared.Attributes;

public class PasswordValidatorAttribute : ValidationAttribute
{
    private string _minimum;
    private string _maximum;

    public PasswordValidatorAttribute(string minimum, string maximum)
    {
        _minimum = minimum;
        _maximum = maximum;
    }

    public override bool IsValid(object value)
    {
        if (value is null) return false;

        if (value.GetType() == typeof(string))
        {
            var passLength = (value as string)
                .Count();

            return (passLength >= _minimum) || (passLength <= _maximum);
        }

        return false;
    }
}