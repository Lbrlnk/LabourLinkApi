
using AdminService.Data;
using AdminService.Mapper;
using AdminService.Repository.MuncipalityRepository;
using AdminService.Services.MuncipalityService;
using AdminService.Repository.SkillRepository;
using AdminService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace AdminService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            DotNetEnv.Env.Load();

			Log.Logger = new LoggerConfiguration()
	        .WriteTo.Console() 
	        .WriteTo.File("LogInformation.txt") 
	        .CreateLogger();
			builder.Host.UseSerilog();
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
            builder.Services.AddScoped<IMuncipalityRepository, MuncipalityRepository>();
            builder.Services.AddScoped<IMuncipalityService, MuncipalityService>();
			builder.Services.AddAutoMapper(typeof(ProfileMapper));
			builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Kaalcharakk", Version = "v1" });
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


               
            var secret  =  Encoding.UTF8.GetBytes("Laboulink21345665432@354*(45234567876543fgbfgnh");
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
                    ValidIssuer = "Labourlink-Api",
                    ValidAudience = "Labourlink-Frontend",
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ClockSkew = TimeSpan.Zero // Optional: Removes the default 5-minute clock skew
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
