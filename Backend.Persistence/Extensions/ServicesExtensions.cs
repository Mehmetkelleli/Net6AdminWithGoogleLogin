using Backend.Application.Abstract;
using Backend.Application.AutoMapper;
using Backend.Infrastructure.Concrete;
using Backend.Persistence.Concrete;
using Backend.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Persistence.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            //dependency injections
            services.AddDbContext<DataContext>(options => options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=GenericData;Integrated Security = True;MultipleActiveResultSets=true;"));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IFileUpload, FileUpload>();
            services.AddScoped<IMailSender, MailSender>();
            services.AddScoped<IDataHub, DataHubContext>();
            services.AddTransient<DataHubContext>();
            services.AddAutoMapper(typeof(AProfile));
        }
     }
  
}

