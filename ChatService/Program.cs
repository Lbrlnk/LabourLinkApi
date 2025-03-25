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




if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
DotNetEnv.Env.Load();

}

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var ConnectionString = Environment.GetEnvironmentVariable("LABOURLINK-DB");
// Add services to the container.

builder.Services.AddDbContext<ChatDbContext>(options =>

options.UseSqlServer(
    ConnectionString,
     sqlOptions => sqlOptions.EnableRetryOnFailure()

    )
);

//builder.Services.AddSingleton<IMongoClient>(new MongoClient(Environment.GetEnvironmentVariable("MongoDB")));

//builder.Services.AddScoped<IMongoDatabase>(provider =>
//{
//    IMongoClient client = provider.GetRequiredService<IMongoClient>();
//    return client.GetDatabase("NotificationDb");
//});
// Replace the existing MongoDB configuration section with this:

// Get the MongoDB connection string for Cosmos DB
// Get the MongoDB connection string for Cosmos DB
var cosmosConnectionString = Environment.GetEnvironmentVariable("COSMOS-CONNECTIONSTRING") ?? throw new InvalidOperationException("cosmos db connection string  is not configured");
string cosmos_database = Environment.GetEnvironmentVariable("COSMOS-DATABASE") ?? throw new InvalidOperationException("cosmos db Database  is not configured");

Console.WriteLine(cosmosConnectionString);
// Create a MongoClientSettings using the proper connection string format for Cosmos DB
var mongoClientSettings = MongoClientSettings.FromConnectionString(cosmosConnectionString);

// Cosmos DB specific settings
mongoClientSettings.ServerApi = new ServerApi(ServerApiVersion.V1);
mongoClientSettings.RetryWrites = false;
mongoClientSettings.ReadPreference = ReadPreference.Nearest;
mongoClientSettings.ConnectTimeout = TimeSpan.FromSeconds(30);
mongoClientSettings.SocketTimeout = TimeSpan.FromSeconds(30);

// Configure connection pooling
mongoClientSettings.MaxConnectionPoolSize = 100;
mongoClientSettings.MinConnectionPoolSize = 10;


builder.Services.AddSingleton<IMongoClient>(new MongoClient(cosmosConnectionString));
builder.Services.AddScoped<IMongoDatabase>(provider =>
{
    IMongoClient client = provider.GetRequiredService<IMongoClient>();
    return client.GetDatabase(cosmos_database);
});

builder.Services.AddAutoMapper(typeof(MapperProfile));

var azure_signalR_Connectionstring= Environment.GetEnvironmentVariable("AZURE-SIGNALR-CONNECTIONSTRING") ?? throw new InvalidOperationException("Azure SignalR coonectonstring is not configured");

builder.Services.AddSignalR().AddAzureSignalR(options =>
{
    options.ConnectionString = azure_signalR_Connectionstring;
    options.ServerStickyMode = Microsoft.Azure.SignalR.ServerStickyMode.Required;
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IChatMessageService, ChatMessageService>();
builder.Services.AddScoped<IConversationService, ConversationService>();

var allow_origin = Environment.GetEnvironmentVariable("ALLOW-ORIGIN") ?? throw new InvalidOperationException("Cors orgin is not configured");
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
var jwt_secret = Environment.GetEnvironmentVariable("JWT-SECRET-KEY") ?? throw new InvalidOperationException("JWT-SECRET-KEY is not configured");
string jwt_issuer = Environment.GetEnvironmentVariable("JWT-ISSUER") ?? throw new InvalidOperationException("JWT-ISSUER is not configured");
string jwt_audience= Environment.GetEnvironmentVariable("JWT-AUDIENCE") ?? throw new InvalidOperationException("JWT-AUDIENCE is not configured");
var secret = Encoding.UTF8.GetBytes(jwt_secret);
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
        ValidIssuer = jwt_issuer,
        ValidAudience = jwt_audience,
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
