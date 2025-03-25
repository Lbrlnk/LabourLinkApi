
    using Ocelot.DependencyInjection;
    using Ocelot.Middleware;

    namespace LabourLinkAPIGateway
    {
        public class Program
        {
            public static async Task Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

                //builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

                builder.Configuration
                    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

                builder.Services.AddSignalR();
          
                builder.Services.AddOcelot(builder.Configuration) ;


                // Add services to the container.

                builder.Services.AddControllers();

                var allowedOrigin = builder.Environment.IsProduction()
                  ? Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGIN") // Read from env in Production
                  : "http://localhost:5173";


                if (string.IsNullOrEmpty(allowedOrigin))
                {
                    throw new Exception("CORS_ALLOWED_ORIGIN environment variable is not set in production.");
                }
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowSpecificOrigin",
                        builder =>
                        {
                            builder.WithOrigins(allowedOrigin)
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
                app.UseRouting();
                app.UseCors("AllowSpecificOrigin");
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseWebSockets();
                await app.UseOcelot();

                await app.RunAsync();
            }
        }
    }
