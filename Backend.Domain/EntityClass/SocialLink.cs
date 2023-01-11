using Backend.Domain.EntityClass.BaseClass;

namespace Backend.Domain.EntityClass
{
    public class SocialLink:Base
    {
        public string? Facebook { get; set; }
        public string? Instegram { get; set; }
        public string? Github { get; set; }
        public string? Google { get; set; }
        public string? Codepen { get; set; }
        public string? Linkedin { get; set; }
        public string? Url { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
