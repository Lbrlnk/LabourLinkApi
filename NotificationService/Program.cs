
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotificationService.Data;
using NotificationService.Hubs;
using NotificationService.Mapper;
using NotificationService.Middleware;
using NotificationService.Repository.InterestRequestRepository;
using NotificationService.Repository.NotificationRepository;
using NotificationService.Services.IntrestRequestService;
using NotificationService.Services.NotificationService;
using System.Text;
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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.SetIsOriginAllowed(_ => true) // Allows all origins while keeping credentials
                               .AllowAnyMethod()   // Allows any HTTP method (GET, POST, PUT, DELETE, etc.)
                               .AllowAnyHeader()   // Allows any headers
                               .AllowCredentials(); // Allows credentials like cookies or auth tokens
                    });
            });
            var jwtSecret = Environment.GetEnvironmentVariable("JWT-SECRET-KEY")
             ?? throw new InvalidOperationException("JWT-SECRET-KEY is not configured");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Environment.GetEnvironmentVariable("JWT-ISSUER"),
                        ValidAudience = Environment.GetEnvironmentVariable("JWT-AUDIENCE"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                        ClockSkew = TimeSpan.Zero
                    };
                });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ProfileService", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });



            //var jwtSecret = Environment.GetEnvironmentVariable("JWT-SECRET-KEY")
            //  ?? throw new InvalidOperationException("JWT-SECRET-KEY is not configured");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Environment.GetEnvironmentVariable("JWT-ISSUER"),
                        ValidAudience = Environment.GetEnvironmentVariable("JWT-AUDIENCE"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAllOrigins");
            app.UseMiddleware<TokenAccessingMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UserIdentificationMiddleware>();
            app.MapHub<NotificationHub>("/nothub").RequireAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
