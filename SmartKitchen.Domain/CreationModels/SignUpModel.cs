using System.ComponentModel.DataAnnotations;

namespace SmartKitchen.Domain.CreationModels
{
	public class SignUpModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Description = "Email")]
        [StringLength(128)]
        public string Email{ get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Description = "Username")]
        [StringLength(128)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Description = "Password")]
        [StringLength(128)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Description = "Confirm password")]
        [StringLength(128)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string Confirm { get; set; }
    }

}