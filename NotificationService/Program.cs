
using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Hubs;
using NotificationService.Mapper;
using NotificationService.Repository.InterestRequestRepository;
using NotificationService.Repository.NotificationRepository;
using NotificationService.Services.IntrestRequestService;
using NotificationService.Services.NotificationService;
using System.Text.Json.Serialization;

namespace NotificationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            DotNetEnv.Env.Load();
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var ConnectionString = Environment.GetEnvironmentVariable("LABOURLINK_DB");
            // Add services to the container.
            builder.Services.AddSignalR();
            
            builder.Services.AddAutoMapper(typeof(MapperProfile));
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                ConnectionString,
                 sqlOptions => sqlOptions.EnableRetryOnFailure()

                )
            );




            builder.Services.AddScoped<IInterestRequestRepository, InterestRequestRepository>();
            builder.Services.AddScoped<IInterestRequestService, InterestRequestService>();
            builder.Services.AddScoped<INotificationService,NotificationServices>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
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
            app.MapHub<NotificationHub>("/nothub");

            app.Run();
        }
    }
}
