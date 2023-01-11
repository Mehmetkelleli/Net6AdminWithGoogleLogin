
using System.ComponentModel.DataAnnotations;

namespace Backend.Application.ViewModel
{
    public class SiteConfigViewModel
    {
        public string? SiteDescriptions { get; set; }
        public string? SiteLogoUrl { get; set; }
        public string? SiteAdmin { get; set; }
        public string? MetaTags { get; set; }
        [Required]
        public string? Host { get; set; }
        [Required]
        public string? SmtpHost { get; set; }
        [Required]
        public int SmtpPort { get; set; }
        [Required]
        public string? SmtpUser { get; set; }
        [Required]
        public string? SmtpPassword { get; set; }
    }
}
