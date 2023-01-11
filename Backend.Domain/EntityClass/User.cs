using Backend.Domain.EntityClass.BaseClass;
using Microsoft.AspNetCore.Identity;

namespace Backend.Domain.EntityClass
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? ImgUrl { get; set; }
        public string? FullImgUrl { get; set; }
        public bool Active { get; set; }
        public string? Age { get; set; }
        public string? Adress { get; set; }
        public string? Country { get; set; }
        public string? WebSite { get; set; }
        public SocialLink? SocialLink { get; set; }
    }
}
