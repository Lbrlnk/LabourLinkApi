
using AdminService.Data;
using AdminService.Mapper;
using AdminService.Repository.SkillRepository;
using AdminService.Services;
using Microsoft.EntityFrameworkCore;

namespace AdminService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            DotNetEnv.Env.Load();

            // Add configuration
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            // Get the connection string for AdminService
            var connectionString = Environment.GetEnvironmentVariable("DB_ADMIN");


            builder.Services.AddDbContext<AdminDbContext>(options =>
                options.UseSqlServer(
                connectionString,
                sqlOptions => sqlOptions.EnableRetryOnFailure()
                 )
                 );

            builder.Services.AddAutoMapper(typeof(MapperProfile));
            builder.Services.AddScoped<ISkillRepostory, SkillRepository>();
            builder.Services.AddScoped<ISkillService, SkillService>();


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
