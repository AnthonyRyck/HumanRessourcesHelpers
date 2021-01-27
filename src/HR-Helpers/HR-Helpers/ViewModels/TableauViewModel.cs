using AccessData.Models;
using HR_Helpers.Data;
using HR_Helpers.Services;
using Radzen;
using Radzen.Blazor;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.ViewModels
{
	public class TableauViewModel : ITableauViewModel
	{
		#region Properties

		/// <see cref="ITableauViewModel.TableauSelected"/>
		public Tableau TableauSelected { get; set; }

		/// <see cref="ITableauViewModel.ToutesLesEntrees"/>
		public List<List<ValueColonne>> ToutesLesEntrees { get; set; }

		/// <see cref="ITableauViewModel.ValeursSaisies"/>
		public List<ValueColonne> ValeursSaisies { get; set; }

		///<see cref="ITableauViewModel.ShowNouvelleEntree"/>
		public bool ShowNouvelleEntree { get; set; }

		private IDataAccess DataAccess;
		private CurrentUserService CurrentUserService;
		private NotificationService _notificationService;

		/// <see cref="ITableauViewModel."/>
		public RadzenGrid<ColonneModel> ColonneModelGrid { get; set; }

		/// <summary>
		/// Correspond au numéro de ligne en cours.
		/// </summary>
		public int NumeroLigne { get; set; }

		#endregion

		#region Constructeur

		public TableauViewModel(IDataAccess dataAccess, CurrentUserService currentUser, NotificationService notificationSvc)
		{
			DataAccess = dataAccess;
			CurrentUserService = currentUser;
			_notificationService = notificationSvc;
		}

		#endregion

		#region Public Methods

		/// <see cref="ITableauViewModel.LoadTableau(string)"/>
		public async Task LoadTableau(string idTableau)
		{
			TableauSelected = await DataAccess.GetTable(idTableau);
			ToutesLesEntrees = await DataAccess.GetValeurs(idTableau, CurrentUserService.UserId);

			NumeroLigne = GetLastLineNumber();

			InitNewData();
		}

		/// <see cref="ITableauViewModel.GetValueColonne(int)"/>
		public ValueColonne GetValueColonne(int idColonne)
		{
			return ValeursSaisies.FirstOrDefault(x => x.IdColonne == idColonne);
		}

		/// <see cref="ITableauViewModel.OpenDialog"/>
		public void OpenDialog()
		{
			ShowNouvelleEntree = true;
		}

		/// <see cref="ITableauViewModel.OnCloseData"/>
		public void OnCloseData()
		{
			ShowNouvelleEntree = false;
			InitNewData();
		}

		/// <see cref="ITableauViewModel.SaveAndClose"/>
		public async void SaveAndClose()
		{
			try
			{
				await DataAccess.SaveData(ValeursSaisies);
				ToutesLesEntrees.Add(ValeursSaisies);
				ShowNouvelleEntree = false;
			}
			catch (Exception ex)
			{
				string errorMsg = "Erreur sur la sauvegarde la donnée";
				Log.Error(ex, errorMsg);
				_notificationService.Notify(NotificationSeverity.Error, "Erreur", errorMsg, 3000);
			}
		}

		/// <see cref="ITableauViewModel.SaveAndNewData"/>
		public async Task SaveAndNewData()
		{
			try
			{
				await DataAccess.SaveData(ValeursSaisies);
				ToutesLesEntrees.Add(ValeursSaisies);
				InitNewData();
			}
			catch (Exception ex)
			{
				string errorMsg = "Erreur sur la sauvegarde la donnée";
				Log.Error(ex, errorMsg);
				_notificationService.Notify(NotificationSeverity.Error, "Erreur", errorMsg, 3000);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Réinitialise "ValeursSaisies" pour une nouvelle entrée.
		/// </summary>
		private void InitNewData()
		{
			// Incrémente le futur numéro.
			NumeroLigne++;

			ValeursSaisies = new List<ValueColonne>();
			foreach (var item in TableauSelected.Colonnes)
			{
				ValeursSaisies.Add(new ValueColonne()
				{
					IdColonne = item.IdColonne,
					IdTableau = TableauSelected.IdTableau,
					IdUser = CurrentUserService.UserId,
					NumeroLigne = NumeroLigne
				});
			}		
		}


		private int GetLastLineNumber()
		{
			if(ToutesLesEntrees.Count == 0)
			{
				return 0;
			}

			int test = ToutesLesEntrees.Max(x => x.Max(y => y.NumeroLigne));
			return test;
		}

		#endregion

	}
}
