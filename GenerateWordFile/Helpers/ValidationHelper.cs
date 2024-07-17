using System;
using System.Text.RegularExpressions;
using GenerateWordFile.Data;

namespace GenerateWordFile.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        public static bool IsValidGender(string gender)
        {
            var validGenders = new List<string> { "Male", "Female", "Other" };
            return validGenders.Contains(gender);
        }

        public static bool IsValidBirthday(this DateTime birthday)
        {
            // Validate BirthDate
            if (birthday >= DateTime.Now.AddDays(-1))
            {
                return false;
            }
            if ( birthday < DateTime.Now.AddYears(-1000))
            {
                return false;
            }
            // Add other validations if needed

            return true;
        }
        public static bool PersonExists(this int id,AppDbContext _context)
        {
            return _context.People.Any(e => e.Id == id);
        }

    }
}