using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.Attributes;

public class PasswordValidatorAttribute : ValidationAttribute
{
    private int _Minimum { get; set; }
    private int _Maximum { get; set; }

    public PasswordValidatorAttribute(int minimum, int maximum) {
       _Minimum = minimum;
       _Maximum = maximum;
    }

    public override bool IsValid(object value) {
        if (value.GetType() == typeof(string)) { 
            var passLength = value.ToString().Length;

            return ((passLength >= _Minimum) && (passLength <= _Maximum))
                ? true 
                : false;
        } else {
            return false;
        }
    }
}