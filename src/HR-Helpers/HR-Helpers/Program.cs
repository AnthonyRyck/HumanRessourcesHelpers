using AccessData;
using HR_Helpers.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers
{
	public class Program
	{
		public static void Main(string[] args)
		{
			string pathLog = Path.Combine(AppContext.BaseDirectory, "Logs");
			if (!Directory.Exists(pathLog))
			{
				Directory.CreateDirectory(pathLog);
			}

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("System", LogEventLevel.Warning)
				.WriteTo.RollingFile(Path.Combine(pathLog, "log-{Date}.txt"))
				.CreateLogger();
			try
			{
				var host = CreateHostBuilder(args).Build();

				var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
				using (var scope = scopeFactory.CreateScope())
				{
					var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
					var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
					var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

					var sqlContext = scope.ServiceProvider.GetRequiredService<SqlContextAccess>();

					if (db.Database.EnsureCreated())
					{
						DataInitializer.InitData(roleManager, userManager).Wait();

						// cr�er le reste de la base
						DataInitializer.CreateTables(sqlContext).Wait();
					}
				}

				host.Run();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erreur dans Main");
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
