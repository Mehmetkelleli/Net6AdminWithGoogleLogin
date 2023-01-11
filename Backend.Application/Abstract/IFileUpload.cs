using Microsoft.AspNetCore.Http;

namespace Backend.Application.Abstract
{
    public interface IFileUpload
    {
        public Task<bool> Send(IFormFile Model, string RandomName, string extensions);
    }
}
