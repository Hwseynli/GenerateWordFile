using System;
using System.ComponentModel.DataAnnotations;

public class PersonVM
{
    public int Id { get; set; }

    [Required]
    [StringLength(25)]
    public string FirstName { get; set; }

    [StringLength(25)]
    public string FatherName { get; set; }

    [Required]
    [StringLength(35)]
    public string LastName { get; set; }

    [DataType(DataType.DateTime)]
    [CustomValidation(typeof(PersonVM), nameof(ValidateBirthdate))]
    public DateTime Birthdate { get; set; }

    public Gender gender { get; set; }

    [Required,EmailAddress]
    public string Email { get; set; }

    public string FinCode { get; set; }

    public string CardNumber { get; set; }

    public static ValidationResult ValidateBirthdate(DateTime birthdate, ValidationContext context)
    {
        if (birthdate > DateTime.Now.AddDays(1))
        {
            return new ValidationResult("Birthdate cannot be in the future.");
        }
        return ValidationResult.Success;
    }
    public enum Gender
    {
        Male,
        Female,
        Other
    }
}