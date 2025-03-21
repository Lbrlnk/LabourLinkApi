

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

            if (builder.Environment.IsDevelopment())
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
                    ConnectionString = "data source = DESKTOP-GUAL8JH; database=LabourLink-DB; Trusted_Connection=True; TrustServerCertificate=True; MultipleActiveResultSets=true;";

                    jwtSecretKey = Environment.GetEnvironmentVariable("JWT-SECRET-KEY") ??
                                   throw new Exception("JWT-SECRET-KEY not found in environment variables");

                    rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ-HOST") ??
                                   throw new Exception("RabbitMQ-Host not found in environment variables");

                    rabbitMqUsername = Environment.GetEnvironmentVariable("RABBITMQ-USERNAME") ??
                                      throw new Exception("RabbitMQ-Username not found in environment variables");

                    rabbitMqPassword = Environment.GetEnvironmentVariable("RABBITMQ-PASSWORD") ??
                                      throw new Exception("RabbitMQ-Password not found in environment variables");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to read secrets: {ex.Message}");
                }
            }
            else
            {
                ConnectionString = "data source = DESKTOP-GUAL8JH; database=LabourLink-DB; Trusted_Connection=True; TrustServerCertificate=True; MultipleActiveResultSets=true;";
                Console.WriteLine(ConnectionString);
                //jwtSecretKey = builder.Configuration["JWT-SECRET-KEY"];
                jwtSecretKey = "Laboulink21345665432@354*(45234567876543fgbfgnh";
                rabbitMqHost = builder.Configuration["RABBITMQ-HOST"];
                rabbitMqUsername = builder.Configuration["RABBITMQ-USERNAME"];
                rabbitMqPassword = builder.Configuration["RABBITMQ-PASSWORD"];
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
                        { "RABBITMQ-HOST", rabbitMqHost },
                        { "RABBITMQ-USERNAME", rabbitMqUsername },
                        { "RABBITMQ-PASSWORD", rabbitMqPassword }
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

            var audience = Environment.GetEnvironmentVariable("JWT-AUDIENCE") ?? builder.Configuration["JwtSettings:Audience"];
            var issuer = Environment.GetEnvironmentVariable("JWT-ISSUER") ?? builder.Configuration["JwtSettings:Issuer"];

            if (string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(issuer))
            {
                throw new Exception("JWT Audience or Issuer is missing from configuration.");
            }

            builder.Services.AddScoped<IJwtHelper>(provider =>
            {
                var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "JWT-SECRET-KEY", jwtSecretKey },
                        { "JWT-ISSUER", issuer },
                        { "JWT-AUDIENCE", audience }
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
