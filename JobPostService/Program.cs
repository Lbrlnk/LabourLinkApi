using DotNetEnv;
using JobPostService.Data;
using JobPostService.Helpers.CloudinaryHelper;
using JobPostService.Helpers.CloudinaryHelper;
using JobPostService.Mapper;
using JobPostService.Repository;
using JobPostService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProfileService.Middlewares;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DotNetEnv.Env.Load();
builder.Configuration["CloudinarySettings:CloudName"] = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUDNAME");
builder.Configuration["CloudinarySettings:ApiKey"] = Environment.GetEnvironmentVariable("CLOUDINARY_APIKEY");
builder.Configuration["CloudinarySettings:ApiSecret"] = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");

builder.Configuration
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();


//var connectionString = Environment.GetEnvironmentVariable("DB_ADMIN");
var connectionString = Environment.GetEnvironmentVariable("shahidjob");
if (string.IsNullOrEmpty(connectionString))
{
	throw new Exception("Database connection string is missing.");
}


builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(
	connectionString,
	sqlOptions => sqlOptions.EnableRetryOnFailure()
	 )
	 );
builder.Services.AddAutoMapper(typeof(ProfileMapper));
builder.Services.AddScoped<ICloudinaryHelper, CloudinaryHelper>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddHttpClient<ProfileServiceClient>();

builder.Services.AddControllers();

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "LabouLink", Version = "v1" });
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



var secret = Encoding.UTF8.GetBytes("Laboulink21345665432@354*(45234567876543fgbfgnh");
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
app.UseCors("AllowSpecificOrigin");


app.UseAuthentication();

app.UseAuthorization();
app.UseMiddleware<TokenAccessingMiddleware>();
app.UseMiddleware<UserIdentificationMiddleware>();

app.MapControllers();

app.Run();