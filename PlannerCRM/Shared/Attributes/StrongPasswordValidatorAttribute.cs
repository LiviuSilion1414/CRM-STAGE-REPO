namespace PlannerCRM.Shared.Attributes;

public partial class StrongPasswordValidatorAttribute : ValidationAttribute
{
    private string _minimum;
    private string _maximum;

    public StrongPasswordValidatorAttribute(string minimum, string maximum)
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
            var string Content = value as string;
            var string Length = string Content.Length;

            return string Length >= _minimum && string Length <= _maximum && EmailRegex().IsMatch(string Content);
        }

        return false;
    }

    [GeneratedRegex("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])")]
    private static partial Regex EmailRegex();
}