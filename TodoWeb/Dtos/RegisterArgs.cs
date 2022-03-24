using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Dtos
{
    public class RegisterArgs : IArgsDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Please enter a valid email.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DisplayName("Confirm password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
