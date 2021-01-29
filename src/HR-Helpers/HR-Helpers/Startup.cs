using AccessData;
using BlazorDownloadFile;
using HR_Helpers.Areas.Identity;
using HR_Helpers.Data;
using HR_Helpers.Services;
using HR_Helpers.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			string connectionDb = Configuration.GetConnectionString("MySqlConnection");

			services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionDb, ServerVersion.AutoDetect(connectionDb)));

			services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddRazorPages();
			services.AddServerSideBlazor();

			services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
			services.AddDatabaseDeveloperPageExceptionFilter();

			// Service SQL de AccessData.
			services.AddSingleton(new SqlContextAccess(connectionDb));

			// Service de Radzen
			services.AddScoped<DialogService>();
			services.AddScoped<NotificationService>();
			services.AddScoped<TooltipService>();
			services.AddScoped<ContextMenuService>();

			// Pour téléchargement de fichier.
			services.AddBlazorDownloadFile();

			services.AddHttpContextAccessor();
			services.AddScoped<CurrentUserService>();
			services.AddScoped<IDataAccess, DataService>();

			services.AddScoped<IUsersViewModel, UsersViewModel>();
			services.AddScoped<IGestionLog, GestionLogViewModel>();
			services.AddScoped<IMesTableauxViewModel, MesTableauxViewModel>();
			services.AddScoped<INewTableViewModel, NewTableViewModel>();
			services.AddScoped<ITableauViewModel, TableauViewModel>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});
		}
	}
}
