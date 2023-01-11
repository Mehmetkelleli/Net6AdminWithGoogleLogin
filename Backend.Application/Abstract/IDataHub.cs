namespace Backend.Application.Abstract
{
    public interface IDataHub
    {
        public Task SendNotification(string text);
    }
}
