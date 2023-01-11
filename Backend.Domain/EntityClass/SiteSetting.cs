using Backend.Domain.EntityClass.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.EntityClass
{
    public class SiteSetting:Base
    {
        public string? SiteDescriptions { get; set; }
        public string? SiteLogoUrl { get; set; }
        public string? SiteAdmin { get; set; }
        public string? MetaTags { get; set; }
        public string Host { get; set; }
        public string SmtpHost { get; set; }
        public  int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public string? GoogleScreetNumber { get; set; }
        public string? GoogleClientNumber { get; set; }
    }
}
