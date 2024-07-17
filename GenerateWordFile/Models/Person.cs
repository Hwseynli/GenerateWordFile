using System.ComponentModel.DataAnnotations;

namespace GenerateWordFile.Models
{
    public class Person:IDCards
    {
        [Required,MinLength(3),MaxLength(25)]
        public string FirstName { get; set; }
        [StringLength(35)]
        public string LastName { get; set; }
        [Required, DataType(DataType.DateTime)]
        public DateTime Birthday { get; set; }
        [Required,MinLength(3),MaxLength(25)]
        public string FatherName { get; set; }
        [Required,StringLength(10)]
        public string Gender { get; set; }
    }
}