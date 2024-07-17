using static PersonVM;
using System.ComponentModel.DataAnnotations;

namespace GenerateWordFile.ViewModels
{
    public class PersonUpdateVM
    {
        public int Id { get; set; }

        [StringLength(25)]
        public string FirstName { get; set; }

        [StringLength(25)]
        public string FatherName { get; set; }

        [StringLength(35)]
        public string LastName { get; set; }

        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(PersonVM), nameof(ValidateBirthdate))]
        public DateTime Birthdate { get; set; }

        public Gender gender { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}