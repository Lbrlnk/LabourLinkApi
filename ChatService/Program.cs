using ChatService.Data;
using ChatService.Hubs;
using ChatService.Mapper;
using ChatService.Middleware;
using ChatService.Repository;
using ChatService.Services.ChatService;
using ChatService.Services.ConversationServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);






DotNetEnv.Env.Load();
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var ConnectionString = Environment.GetEnvironmentVariable("DB-CONNECTION-STRING");
// Add services to the container.

builder.Services.AddDbContext<ChatDbContext>(options =>

options.UseSqlServer(
    ConnectionString,
     sqlOptions => sqlOptions.EnableRetryOnFailure()

    )
);

builder.Services.AddSingleton<IMongoClient>(new MongoClient(Environment.GetEnvironmentVariable("MongoDB")));

builder.Services.AddScoped<IMongoDatabase>(provider =>
{
    IMongoClient client = provider.GetRequiredService<IMongoClient>();
    return client.GetDatabase("NotificationDb");
});
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddSignalR();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IChatMessageService, ChatMessageService>();
builder.Services.AddScoped<IConversationService, ConversationService>();


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
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatLabrLink", Version = "v1" });
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

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the token isn't in the query, check for it in the Authorization header
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            }

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
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
app.UseMiddleware<TokenAccessingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UserIdentificationMiddleware>();
app.MapHub<ChatHub>("/chatHub").RequireAuthorization();
app.MapControllers();

app.Run();
