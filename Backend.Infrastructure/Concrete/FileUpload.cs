using Backend.Application.Abstract;
using Microsoft.AspNetCore.Http;

namespace Backend.Infrastructure.Concrete
{
    public class FileUpload:IFileUpload
    {
        public async Task<bool> Send(IFormFile Model, string RandomName, string extensions)
        {
            var state = true;
            if (Path.GetExtension(Model.FileName) != $".{extensions}")
            {
                state = false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", RandomName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await Model.CopyToAsync(stream);
            }
            return state;
        }
    }
}
