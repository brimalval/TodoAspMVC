using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Dtos
{
    public class PasswordResetArgs
    {
        public int UserId { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DisplayName("Confirm password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
