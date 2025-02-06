using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace LabourLinkAPIGateway
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Configuration.AddJsonFile("ocelot.jsaon", optional: false, reloadOnChange: true);
			builder.Services.AddOcelot(builder.Configuration);

			// Add services to the container.

			builder.Services.AddControllers();

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

			app.UseAuthorization();


			app.MapControllers();
			await app.UseOcelot();

			await app.RunAsync();
		}
	}
}