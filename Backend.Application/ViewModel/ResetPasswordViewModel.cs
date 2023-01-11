
using System.ComponentModel.DataAnnotations;

namespace Backend.Application.ViewModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required,MinLength(10),DataType(DataType.Password)]
        public string Password { get; set; }
        [Required,Compare("Password"),DataType(DataType.Password)]
        public string RePassword { get; set; }
    }
}
