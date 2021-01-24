using AccessData.Models;
using HR_Helpers.Data;
using HR_Helpers.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Radzen;
using Radzen.Blazor;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.ViewModels
{
	public class NewTableViewModel : INewTableViewModel
	{
		#region Properties
				
		public Tableau NouveauTableau { get; set; }
		public ColonneModel NouvelleColonne { get; set; }

		public RadzenGrid<ColonneModel> ColonneModelGrid { get; set; }


		public CurrentUserService CurrentUserService { get; set; }
		public IDataAccess DataService { get; set; }
		public bool ShowNewColonne { get; set; }


		private string _saveValueType;
		private NotificationService _notificationService;

		#endregion

		public NewTableViewModel(CurrentUserService userService, IDataAccess dataAccess, NotificationService notification)
		{
			NouveauTableau = new Tableau();
			NouvelleColonne = new ColonneModel();
			ColonneModelGrid = new RadzenGrid<ColonneModel>();

			NouveauTableau.IdTableau = Guid.NewGuid();

			CurrentUserService = userService;
			DataService = dataAccess;
			_notificationService = notification;
		}

		#region Public Methods

		

		public async Task CreateTable()
		{
			try
			{
				NouveauTableau.IdUser = CurrentUserService.UserId;
				await DataService.SaveNewTable(NouveauTableau);

				var message = new NotificationMessage
				{
					Severity = NotificationSeverity.Success,
					Summary = "Enregistré",
					Detail = "Tableau enregistré avec succés",
					Duration = 4000
				};
				_notificationService.Notify(message);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erreur sur la création d'un tableau");
				Log.Error(" --NomTableau : " + NouveauTableau.NomDuTableau
					+ " --IdUser : " + NouveauTableau.IdUser
					+ " --IdTableau : " + NouveauTableau.IdTableau
					+ " --Description : " + NouveauTableau.Description);

				foreach (var item in NouveauTableau.Colonnes)
				{
					Log.Error("NomColonne : " + item.NomColonne 
							+ " --TableId : " + item.TableId
							+ " --IdColonne : " + item.IdColonne
							+ " --TypeData : " + item.TypeData
							+ " --Description : " + item.Description);
				}

				var message = new NotificationMessage
				{
					Severity = NotificationSeverity.Error,
					Summary = "Erreur",
					Detail = "Votre tableau n'a pas été enregistré, suite à une erreur en base de donnée.",
					Duration = 4000
				};
				_notificationService.Notify(message);
			}
		}

		public async Task OnClickAddColonne()
		{
			ShowNewColonne = true;
			await Task.Delay(1);
		}

		public async void CloseAndSaveNewColonne()
		{
			await SaveColonne();
			ShowNewColonne = false;
		}

		public void CloseNewColonne()
		{
			ShowNewColonne = false;
			NouvelleColonne = new ColonneModel();
		}

		public void OnChangeDataType(ChangeEventArgs e)
		{
			NouvelleColonne.TypeData = e.Value.ToString();
		}

		public async Task SaveAndNewColonne()
		{
			SaveColonne();
		}

		#endregion

		#region Private methods

		private async Task SaveColonne()
		{
			NouvelleColonne.TableId = NouveauTableau.IdTableau;

			if (NouveauTableau.Colonnes.Any())
				NouvelleColonne.IdColonne = NouveauTableau.Colonnes.Max(x => x.IdColonne) + 1;

			// Si la valeur n'est pas null, c'est que l'utilisateur a modifié la valeur.
			if (NouvelleColonne.TypeData != null)
			{
				_saveValueType = NouvelleColonne.TypeData;
			}
			else
			{
				// Sinon il a gardé la même valeur que l'autre.
				NouvelleColonne.TypeData = _saveValueType;
			}

			NouveauTableau.Colonnes.Add(NouvelleColonne);
			NouvelleColonne = new ColonneModel();

			if(NouveauTableau.Colonnes.Count > 1)
				await ColonneModelGrid.Reload();
		}

		#endregion
	}
}
