using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Validations;
using ProfileService.Data;
using ProfileService.Helper.CloudinaryHelper;
using ProfileService.Mapper;
using ProfileService.Middlewares;
using ProfileService.Repositories.EmployerRepository;
using ProfileService.Repositories.LabourRepository;
using ProfileService.Repositories.ReviewRepository;
using ProfileService.Services.EmployerService;
using ProfileService.Services.LabourService;
using RabbitMQ.Client;
using ProfileService.Services.ReviewService;
using System.Text;
using System.Text.Json.Serialization;
using ProfileService.Services.JobPostServiceClientService;
using ProfileService.Repositories.LabourWithinEmployer;
using DotNetEnv;
using ProfileService.Services.SkillAnalyticsServices;
using ProfileService.Services.ConversationService;
using ProfileService.Repositories.ChatConversationRepository;
using ProfileService.Services.RabbitMQ;


namespace ProfileService
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
            
            var connectionString = Environment.GetEnvironmentVariable("DB-CONNECTION-STRING")
                ?? throw new InvalidOperationException("DB-ConnectionString is not configured");

            builder.Services.AddDbContext<LabourLinkProfileDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure()
                )
            );

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.SetIsOriginAllowed(_ => true) 
                               .AllowAnyMethod()   
                               .AllowAnyHeader()   
                               .AllowCredentials(); 
                    });
            });

            builder.Services.AddHttpClient<JobPostServiceClient>();
            builder.Services.AddScoped<IEmployerLabour, EmployerLabour>();

            builder.Services.AddAutoMapper(typeof(MapperProfile));
            builder.Services.AddScoped<ICloudinaryHelper, CloudinaryHelper>();


            builder.Services.AddScoped<ISkillAnalyticsService, SkillAnalyticsService>();
            builder.Services.AddScoped<ILabourService, LabourService>();
            builder.Services.AddScoped<IEmployerService, EmployerService>();
			builder.Services.AddScoped<IConversationService, ConversationService>();
            builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();


            builder.Services.AddScoped<ILabourRepository, LabourRepository>();
            builder.Services.AddScoped<IEmployerRepository, EmployerRepository>();
			builder.Services.AddScoped<IChatConversationRepository, ChatConversationRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
           
            builder.Services.AddControllers().AddJsonOptions(options =>
             {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
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

            var allow_origin = Environment.GetEnvironmentVariable("CORS-ORIGIN") ?? throw new InvalidOperationException("cors origin not configured");
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins(allow_origin)
                               .AllowCredentials()
                               .AllowAnyMethod()
                               .AllowAnyHeader();

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


            var app = builder.Build();

            //if (!app.Environment.IsDevelopment())
            //{
            //    using var scope = app.Services.CreateScope();
            //    var db = scope.ServiceProvider.GetRequiredService<LabourLinkProfileDbContext>();
            //    db.Database.Migrate();
            //}



            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigin");
            app.UseMiddleware<TokenAccessingMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UserIdentificationMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}