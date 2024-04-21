namespace PlannerCRM.Client.Utilities.Validation;

[Authorize]
public static class ValidatorService
{
    public static bool Validate(object model, out Dictionary<string, List<string>> errors)
    {
        errors = new();

        if (model is null) return false;

        var properties = model
            .GetType()
            .GetProperties()
            .ToList();

        foreach (var property in properties)
        {
            var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();

            foreach (var attribute in validationAttributes)
            {
                var propertyValue = property.GetValue(model);

                var isValid = attribute.IsValid(propertyValue);
                var validationContext = new ValidationContext(property)
                {
                    MemberName = property.Name
                };

                if (!isValid)
                {
                    if (errors.ContainsKey(validationContext.MemberName))
                    {
                        errors[validationContext.MemberName]
                            .Add(attribute.ErrorMessage);
                    }
                    else
                    {
                        errors
                            .Add(validationContext.MemberName, new() { attribute.ErrorMessage });
                    }
                }
            }
        }

        return !errors.Any();
    }
}
