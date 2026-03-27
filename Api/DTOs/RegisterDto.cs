using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "The email format is not valid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A password is required.")]

        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "The password must have at least 8 characters, one uppercase letter, one number, and one special character.")]
        public string Password { get; set; } = string.Empty;
    }
}
