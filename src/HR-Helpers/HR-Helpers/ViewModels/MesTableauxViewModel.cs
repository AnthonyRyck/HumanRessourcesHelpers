using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radzen;
using HR_Helpers.Services;
using HR_Helpers.Data;
using AccessData.Models;

namespace HR_Helpers.ViewModels
{
	public class MesTableauxViewModel : IMesTableauxViewModel
	{
		private NavigationManager NavigationManager;
		private IDataAccess dataAccess;
		private CurrentUserService currentUserService;

		///<inheritdoc cref="IMesTableauxViewModel.TousMesTableau"/>
		public IEnumerable<Tableau> TousMesTableau { get; set; }


		public MesTableauxViewModel(NavigationManager navigation, CurrentUserService userService, IDataAccess accessData)
		{
			NavigationManager = navigation;
			currentUserService = userService;
			dataAccess = accessData;

			GetMesTableaux().GetAwaiter().GetResult();
		}


		///<inheritdoc cref="IMesTableauxViewModel.AddTableau"/>
		public void AddTableau()
		{
			NavigationManager.NavigateTo("/nouveautableau", true);
		}

		///<inheritdoc cref="IMesTableauxViewModel.GetMesTableaux"/>
		public async Task GetMesTableaux()
		{
			TousMesTableau = await dataAccess.GetUserTableaux(currentUserService.UserId);
		}

		///<inheritdoc cref="IMesTableauxViewModel.OpenThisTableau(string)"/>
		public async Task OpenThisTableau(string idTableau)
		{
			NavigationManager.NavigateTo("/tableau/"+idTableau, true);
		}
	}
}
