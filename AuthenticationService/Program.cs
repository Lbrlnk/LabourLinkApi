
using AuthenticationService.Data;
using AuthenticationService.Helpers.CloudinaryHelper;
using AuthenticationService.Helpers.JwtHelper;
using AuthenticationService.Mapper;
using AuthenticationService.Repositories;
//using AuthenticationService.Services.ProfileCompletionConsumerService;
using AuthenticationService.Sevices.AuthSerrvice;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace AuthenticationService
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


            var ConnectionString = Environment.GetEnvironmentVariable("DB_USERS");


            // Add services to the container.

            builder.Services.AddDbContext<AuthenticationDbContext>( options =>

            options.UseSqlServer(
                ConnectionString,
                sqlOptions => sqlOptions.EnableRetryOnFailure()
                ) 
                );

            builder.Services.AddAutoMapper(typeof(MapperProfile));
            builder.Services.AddScoped<IJwtHelper, JwtHelper>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<ICloudinaryHelper, CloudinaryHelper>();
            //builder.Services.AddHostedService<ProfileCompletionConsumerService>();





            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            

            builder.Services.AddSwaggerGen();

            var secretKey = Encoding.UTF8.GetBytes(builder.Configuration["JWT_SECRET_KEY"]);
            var audience = builder.Configuration["JWT_AUDIENCE"];
            var issuer = builder.Configuration["JWT_ISSUER"];

            
            // Configure JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ClockSkew = TimeSpan.Zero 
                };
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5173")
                               .AllowCredentials()
                               .AllowAnyMethod()
                               .AllowAnyHeader();

                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
