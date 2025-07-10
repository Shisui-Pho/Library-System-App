using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Infrastructure;
public class RequiredIfAttribute : ValidationAttribute, IClientModelValidator
{
    private string PropertyName { get; set; }
    private object DesiredValue { get; set; }

    public RequiredIfAttribute(string propertyName, object desiredValue)
    {
        PropertyName = propertyName;
        DesiredValue = desiredValue;
    }

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        var instance = context.ObjectInstance;
        var type = instance.GetType();
        var propertyValue = type.GetProperty(PropertyName)?.GetValue(instance, null);

        if (propertyValue?.ToString() == DesiredValue.ToString() && value == null)
        {
            return new ValidationResult(ErrorMessage);
        }
        return ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-requiredif", ErrorMessage);
        context.Attributes.Add("data-val-requiredif-property", PropertyName);
        context.Attributes.Add("data-val-requiredif-desiredvalue", DesiredValue.ToString());
    }
}
