using DotNetEnv;
using JobPostService.Data;
using JobPostService.Helpers.CloudinaryHelper;
using JobPostService.Mapper;
using JobPostService.Repository;
using JobPostService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DotNetEnv.Env.Load();
builder.Configuration["CloudinarySettings:CloudName"] = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
builder.Configuration["CloudinarySettings:ApiKey"] = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY");
builder.Configuration["CloudinarySettings:ApiSecret"] = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");

builder.Configuration
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();

// Get the connection string for AdminService
//var connectionString = Environment.GetEnvironmentVariable("DB_ADMIN");
var connectionString = Environment.GetEnvironmentVariable("DB_ADMIN");
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
builder.Services.AddSwaggerGen();

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