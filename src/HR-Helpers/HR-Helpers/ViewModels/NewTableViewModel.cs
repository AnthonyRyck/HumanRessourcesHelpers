﻿using AccessData.Models;
using Blazored.Modal;
using Blazored.Modal.Services;
using HR_Helpers.Composants;
using HR_Helpers.Data;
using HR_Helpers.Services;
using HR_Helpers.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Radzen;
using Radzen.Blazor;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.Data.ModelValidation
{
	public class NewTableViewModel : INewTableViewModel
	{
		#region Properties

		/// <see cref="INewTableViewModel.NouveauTableau"/>
		public Tableau NouveauTableau { get; set; }
		
		public TableauModelValidation TableauModel { get; set; }
		public EditContext EditContextValidation { get; set; }

		/// <see cref="INewTableViewModel.NouvelleColonne"/>
		public ColonneModel NouvelleColonne { get; set; }



		/// <see cref="INewTableViewModel.ShowNewColonne"/>
		public bool ShowNewColonne { get; set; }

		public CurrentUserService CurrentUserService { get; set; }
		public IDataAccess DataService { get; set; }


		private string _saveValueType;
		private NotificationService _notificationService;
		private NavigationManager _navigationManager;
		private IModalService ModalService;


		#endregion

		public NewTableViewModel(CurrentUserService userService, IDataAccess dataAccess,
			NotificationService notification, NavigationManager navigationManager,
			IModalService modalSvc)
		{
			TableauModel = new TableauModelValidation();
			TableauModel.IdTable = Guid.NewGuid();

			NouvelleColonne = new ColonneModel();

			CurrentUserService = userService;
			DataService = dataAccess;
			_notificationService = notification;

			ModalService = modalSvc;
			_navigationManager = navigationManager;
		}

		#region Public Methods

		/// <see cref="INewTableViewModel.CreateTable"/>
		public async Task CreateTable()
		{
			try
			{
				if (!EditContextValidation.Validate())
					return;

				NouveauTableau = TableauModel.ToTableau(CurrentUserService.UserId);
				await DataService.SaveNewTable(NouveauTableau);

				var message = new NotificationMessage
				{
					Severity = NotificationSeverity.Success,
					Summary = "Enregistré",
					Detail = "Tableau enregistré avec succés",
					Duration = 4000
				};
				_notificationService.Notify(message);

				_navigationManager.NavigateTo("/mestableaux", true);
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

		/// <see cref="INewTableViewModel.OnClickAddColonne"/>
		public async Task OnClickAddColonne()
		{
			ShowNewColonne = true;
			await Task.Delay(1);
		}

		/// <see cref="INewTableViewModel.CloseAndSaveNewColonne"/>
		public async void CloseAndSaveNewColonne()
		{
			await SaveColonne();
			ShowNewColonne = false;
		}

		/// <see cref="INewTableViewModel.CloseNewColonne"/>
		public void CloseNewColonne()
		{
			ShowNewColonne = false;
			NouvelleColonne = new ColonneModel();
		}

		public void OnChangeDataType(ChangeEventArgs e)
		{
			NouvelleColonne.TypeData = e.Value.ToString();
		}

		/// <see cref="INewTableViewModel.SaveAndNewColonne"/>
		public async Task SaveAndNewColonne()
		{
			await SaveColonne();
		}

		/// <see cref="INewTableViewModel.EditColonne(int, Guid)"/>
		public async Task EditColonne(int idColonne, Guid idTableau)
		{
			try
			{
				var colonneSelected = TableauModel.Colonnes.FirstOrDefault(x => x.IdColonne == idColonne && x.TableId == idTableau);

				var parameters = new ModalParameters();
				parameters.Add(nameof(EditColonneComponent.Colonne), colonneSelected);

				var messageForm = ModalService.Show<EditColonneComponent>("Edition", parameters);
				var result = await messageForm.Result;

				if (!result.Cancelled)
				{
					colonneSelected = (ColonneModel)result.Data;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erreur sur l'édition d'une colonne.");
			}
		}

		/// <see cref="INewTableViewModel.DeleteColonne(int)"/>
		public void DeleteColonne(int idColonne)
		{
			try
			{
				// Suppression de la colonne du tableau
				TableauModel.Colonnes.RemoveAll(x => x.IdColonne == idColonne);

				// Recalcul des IdColonne pour les autres
				var temp = TableauModel.Colonnes.Where(x => x.IdColonne > idColonne);
				foreach (var item in temp)
				{
					item.IdColonne = item.IdColonne - 1;
				}

				DetermineNewId();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erreur sur la suppression d'une colonne.");
			}
		}

		#endregion

		#region Private methods

		private async Task SaveColonne()
		{
			NouvelleColonne.TableId = TableauModel.IdTable;

			if (TableauModel.Colonnes == null)
			{
				TableauModel.Colonnes = new List<ColonneModel>();
			}

			if (TableauModel.Colonnes.Any())
			{
				NouvelleColonne.IdColonne = TableauModel.Colonnes.Max(x => x.IdColonne) + 1;
			}


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

			TableauModel.Colonnes.Add(NouvelleColonne);
			NouvelleColonne = new ColonneModel();
		}

		private void DetermineNewId()
		{
			if (TableauModel.Colonnes.Any())
			{
				NouvelleColonne.IdColonne = TableauModel.Colonnes.Max(x => x.IdColonne) + 1;
			}
		}

		#endregion
	}
}
