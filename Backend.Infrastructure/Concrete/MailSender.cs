using Backend.Application.Abstract;
using System.Net.Mail;
using System.Net;
using Backend.Domain.EntityClass;

namespace Backend.Infrastructure.Concrete
{
    public class MailSender:IMailSender
    {
        private IGenericRepository<SiteSetting> _Setting;
        public MailSender(IGenericRepository<SiteSetting> Setting)
        {
            _Setting = Setting;

        }
        public async Task<bool> Send(string EMail, string Title, string HtmlDescription)
        {
            var setting = await _Setting.GetByIdAsync(1, false);
            var client = new SmtpClient(setting.SmtpHost, setting.SmtpPort)
            {
                Credentials = new NetworkCredential(setting.SmtpUser, setting.SmtpPassword),
                EnableSsl = true
            };
            await client.SendMailAsync(
                new MailMessage(setting.SmtpUser, EMail, Title, HtmlDescription)
                {
                    IsBodyHtml = true

                }
                );
            return true;
        }
    }
}
