
using Microsoft.EntityFrameworkCore;
using NextWebApi.Models;
using NextWebApi.Utils;

namespace NextWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //显示中文注释，添加xml文档注释
            builder.Services.AddSwaggerGen(m =>
            {
                string path=AppContext.BaseDirectory+"NextWebApi.xml";
                m.IncludeXmlComments(path, true);
            });
            //添加AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperSettings));
            //添加数据库上下文
            builder.Services.AddDbContext<NextToDoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
