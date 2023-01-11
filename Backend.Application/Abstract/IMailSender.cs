namespace Backend.Application.Abstract
{
    public interface IMailSender
    {
        public Task<bool> Send(string EMail, string Title, string HtmlDescription);
    }
}
