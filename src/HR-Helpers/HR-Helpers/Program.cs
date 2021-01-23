using HR_Helpers.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();

			var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
			using (var scope = scopeFactory.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

				if (db.Database.EnsureCreated())
				{
					DataInitializer.InitData(roleManager, userManager).Wait();
				}
			}

			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
