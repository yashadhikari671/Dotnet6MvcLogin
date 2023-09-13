using System.ComponentModel.DataAnnotations;

namespace Dotnet6MvcLogin.Models.DTO
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class otpvalidation
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string otp { get; set; }
    }
}
