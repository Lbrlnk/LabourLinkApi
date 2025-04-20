
using AuthenticationService.Data;
using AuthenticationService.Helpers.JwtHelper;
using AuthenticationService.Mapper;
using AuthenticationService.Repositories;
using AuthenticationService.Sevices.AuthSerrvice;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json.Serialization;
using DotNetEnv;

using AuthenticationService.Middleware;

namespace AuthenticationService
{
    public class Program
    {
        public static void Main(string[] args)
        {

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                Env.Load();
            }
            var builder = WebApplication.CreateBuilder(args);
          
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            string ConnectionString= Environment.GetEnvironmentVariable("DB-CONNECTION-STRING") ?? throw new InvalidOperationException("connection string is not configured");
            string jwtSecretKey = Environment.GetEnvironmentVariable("JWT-SECRET-KEY") ?? throw new InvalidOperationException("jwt key is not configured") ;
           

          
       
        var secretKey = Encoding.UTF8.GetBytes(jwtSecretKey);

            builder.Services.AddDbContext<AuthenticationDbContext>(options =>

            options.UseSqlServer(
                ConnectionString,
                sqlOptions => sqlOptions.EnableRetryOnFailure()
                )
                );

            builder.Services.AddAutoMapper(typeof(MapperProfile));
            builder.Services.AddScoped<IJwtHelper, JwtHelper>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            
            builder.Services.AddHostedService<ProfileCompletionConsumerService>();

         
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
           
            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "LabourLink-Authentication", Version = "v1" });
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

            var audience = Environment.GetEnvironmentVariable("JWT-AUDIENCE") ?? throw new InvalidOperationException("JWT  audience is not configured"); 
            var issuer = Environment.GetEnvironmentVariable("JWT-ISSUER") ?? throw new InvalidOperationException("JWT  issuer is not configured");


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

            var allowed_origin = Environment.GetEnvironmentVariable("CORS-ORIGIN") ?? throw new InvalidOperationException("cors origin is not configured");

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins(allowed_origin) 
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials(); 
                    });
            });


            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<TokenAccessingMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UserIdentificationMiddleware>();


            app.MapControllers();

            app.Run();
        }
    }
}
