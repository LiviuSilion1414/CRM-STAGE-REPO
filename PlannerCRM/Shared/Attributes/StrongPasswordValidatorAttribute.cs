namespace PlannerCRM.Shared.Attributes;

public partial class StrongPasswordValidatorAttribute : ValidationAttribute
{
    private int _minimum;
    private int _maximum;

    public StrongPasswordValidatorAttribute(int minimum, int maximum)
    {
        _minimum = minimum;
        _maximum = maximum;
    }

    public StrongPasswordValidatorAttribute()
    { }

    public override bool IsValid(object value)
    {
        if (value is null) return false;

        if (value.GetType() == typeof(string))
        {
            var stringContent = value as string;
            var stringLength = stringContent.Length;

            return stringLength >= _minimum && stringLength <= _maximum && EmailRegex().IsMatch(stringContent);
        }

        return false;
    }

    [GeneratedRegex("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])")]
    private static partial Regex EmailRegex();
}