using AuthenticationService.Data;
using AuthenticationService.Helpers.JwtHelper;
using AuthenticationService.Mapper;
using AuthenticationService.Repositories;
using AuthenticationService.Sevices.AuthSerrvice;
using AuthenticationService.Sevices.ProfileCompletionConsumerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json.Serialization;
using EventBus.Implementations;

namespace AuthenticationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            if(builder.Environment.IsDevelopment())
            {
                DotNetEnv.Env.Load();
            }

            builder.Configuration
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddEnvironmentVariables();

            string ConnectionString;
            string jwtSecretKey;
            byte[] secretKey;
            string rabbitMqHost;
            string rabbitMqUsername;
            string rabbitMqPassword;
            if (builder.Environment.IsProduction())
            {
                try
                {
                    ConnectionString = Environment.GetEnvironmentVariable("DB-ConnectionString") ??
                                       throw new Exception("DB-ConnectionString not found in environment variables");

                    jwtSecretKey = Environment.GetEnvironmentVariable("JWT-SECRET-KEY") ??
                                   throw new Exception("JWT-SECRET-KEY not found in environment variables");

                    rabbitMqHost = Environment.GetEnvironmentVariable("RabbitMQ-Host") ??
                                   throw new Exception("RabbitMQ-Host not found in environment variables");

                    rabbitMqUsername = Environment.GetEnvironmentVariable("RabbitMQ-Username") ??
                                      throw new Exception("RabbitMQ-Username not found in environment variables");

                    rabbitMqPassword = Environment.GetEnvironmentVariable("RabbitMQ-Password") ??
                                      throw new Exception("RabbitMQ-Password not found in environment variables");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to read secrets: {ex.Message}");
                }
            }
            else
            {
                ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine(ConnectionString);
                jwtSecretKey = builder.Configuration["JwtSettings:SecretKey"];
                rabbitMqHost = builder.Configuration["RabbitMQ:Host"];
                rabbitMqUsername = builder.Configuration["RabbitMQ:Username"];
                rabbitMqPassword = builder.Configuration["RabbitMQ:Password"];
            }

            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new Exception("Database connection string is missing.");
            }
            
            if (string.IsNullOrEmpty(jwtSecretKey))
            {
                throw new Exception("JWT Secret Key is missing.");
            }

            secretKey = Encoding.UTF8.GetBytes(jwtSecretKey);

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
            
            builder.Services.AddSingleton<RabbitMQConnection>(sp =>
            {
                ;
                var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "RabbitMQ:Host", rabbitMqHost },
                        { "RabbitMQ:Username", rabbitMqUsername },
                        { "RabbitMQ:Password", rabbitMqPassword }
                    })
                    .Build();
                
                var connection = new RabbitMQConnection(config);
                connection.DeclareExchange("labourlink.events", ExchangeType.Direct);
                return connection;
            });
           
            builder.Services.AddHostedService<ProfileCompletionConsumer>();

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

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? builder.Configuration["JwtSettings:Audience"];
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? builder.Configuration["JwtSettings:Issuer"];

            if (string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(issuer))
            {
                throw new Exception("JWT Audience or Issuer is missing from configuration.");
            }

            builder.Services.AddScoped<IJwtHelper>(provider => 
            {
                var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "JwtSettings:SecretKey", jwtSecretKey },
                        { "JwtSettings:Issuer", issuer },
                        { "JwtSettings:Audience", audience }
                    })
                    .Build();
                return new JwtHelper(config);
            });

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