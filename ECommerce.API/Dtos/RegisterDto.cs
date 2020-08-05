using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=^.{6,255}$)((?=.*\\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\\d)"+
        "(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.*", 
        ErrorMessage = "Password at least 1 uppercase, 1 lowercase, 1 number, 1 non alphabetic number and at least 6 character")]
        public string Password { get; set; }

        [Required]
        public string DisplayName { get; set; }
    }
}