using AccessData.Models;
using HR_Helpers.Data;
using HR_Helpers.Services;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.ViewModels
{
	public class TableauViewModel : ITableauViewModel
	{
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


		/// <see cref="ITableauViewModel."/>
		public RadzenGrid<ColonneModel> ColonneModelGrid { get; set; }

		public TableauViewModel(IDataAccess dataAccess, CurrentUserService currentUser)
		{
			DataAccess = dataAccess;
			CurrentUserService = currentUser;
		}

		/// <see cref="ITableauViewModel.LoadTableau(string)"/>
		public async Task LoadTableau(string idTableau)
		{
			TableauSelected = await DataAccess.GetTable(idTableau);
			ToutesLesEntrees = await DataAccess.GetValeurs(idTableau, CurrentUserService.UserId);

			InitNewData();
		}

		/// <see cref="ITableauViewModel.GetValueColonne(int)"/>
		public ValueColonne GetValueColonne(int idColonne)
		{
			return ValeursSaisies.FirstOrDefault(x => x.IdColonne == idColonne);
		}


		public void OpenDialog()
		{
			ShowNouvelleEntree = true;
		}

		public void OnCloseData()
		{
			ShowNouvelleEntree = false;
		}

		public void SaveAndClose()
		{


			ShowNouvelleEntree = false;
		}


		private void InitNewData()
		{
			ValeursSaisies = new List<ValueColonne>();
			foreach (var item in TableauSelected.Colonnes)
			{
				ValeursSaisies.Add(new ValueColonne()
				{
					IdColonne = item.IdColonne,
					IdTableau = TableauSelected.IdTableau,
					IdUser = CurrentUserService.UserId
				});
			}
		}
	}
}
