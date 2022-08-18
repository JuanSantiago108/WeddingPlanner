
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models;

public class FutureDate: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }
        DateTime date = (DateTime)value;


        if (date <= DateTime.Now )
        {
            return new ValidationResult("date cant be a past date");
        }
        return ValidationResult.Success;


    }
}
