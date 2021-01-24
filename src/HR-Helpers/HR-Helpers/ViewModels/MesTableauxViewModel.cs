using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radzen;

namespace HR_Helpers.ViewModels
{
	public class MesTableauxViewModel : IMesTableauxViewModel
	{
		private NavigationManager NavigationManager;


		public MesTableauxViewModel(NavigationManager navigation)
		{
			NavigationManager = navigation;
		}



		public void AddTableau()
		{
			NavigationManager.NavigateTo("/nouveautableau", true);
		}

	}
}
