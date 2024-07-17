using System.ComponentModel.DataAnnotations;

namespace GenerateWordFile.Models
{
    public class IDCards
    {
        [Key]
        public int Id { get; set; }
        [StringLength(7)]
        public string? FinCode { get; set; }
        [StringLength(9)]
        public string? CardNumber { get; set; }
        [Required, DataType(DataType.EmailAddress), RegularExpression(@"^[a-zA-Z0-9_.+-]+@email\.com$", ErrorMessage = "Email must be a valid @email.com address.")]
        public string EmailAddress { get; set; }
    }
}

