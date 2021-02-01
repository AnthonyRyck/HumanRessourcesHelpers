using AccessData;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.Data
{
	public class DataInitializer
	{
		private static readonly string[] Roles = new string[] { "Admin", "Manager" };

		public static async Task InitData(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
		{
			foreach (var role in Roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(new IdentityRole(role));
				}
			}
			// Création de l'utilisateur Root.
			var user = await userManager.FindByNameAsync("root");
			if (user == null)
			{
				var poweruser = new IdentityUser
				{
					UserName = "root",
					Email = "root@email.com",
					EmailConfirmed = true
				};
				string userPwd = "Azerty123!";
				var createPowerUser = await userManager.CreateAsync(poweruser, userPwd);
				if (createPowerUser.Succeeded)
				{
					await userManager.AddToRoleAsync(poweruser, "Admin");
				}
			}
		}

		internal static async Task CreateTables(SqlContextAccess sqlContext)
		{
			try
			{
				string pathSql = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "CreationTables.txt");
				await sqlContext.CreateTables(pathSql);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erreur sur la création de la base de donnée.");
			}
		}
	}
}
