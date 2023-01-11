using Backend.Domain.EntityClass;
using System.ComponentModel.DataAnnotations;

namespace Backend.Application.ViewModel
{
    public class UserSettingViewModel
    {
        [Required,MinLength(3)]
        public string Name { get; set; }
        [Required, MinLength(3)]
        public string LastName { get; set; }
        [Required, MinLength(5)]
        public string UserName { get; set; }
        [Required, MinLength(5)]
        public string EMail { get; set; }
        [Required]
        public string ImgUrl { get; set; }
        [Required]
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string? Age { get; set; }
        public string? Adress { get; set; }
        public string? Country { get; set; }
        public string? WebSite { get; set; }
        public string? Phone { get; set; }
        public SocialLink? SocialLink { get; set; }
    }
}
