using AccessData.Models;
using BlazorDownloadFile;
using HR_Helpers.Data;
using HR_Helpers.Services;
using Microsoft.AspNetCore.Components;
using OfficeOpenXml;
using Radzen;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
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
		private IBlazorDownloadFileService DownloadFileService;
		private NavigationManager NavigationManager;

		/// <see cref="ITableauViewModel.IsUserProprietaire"/>
		public bool IsUserProprietaire { get; set; }

		/// <summary>
		/// Correspond au numéro de ligne en cours.
		/// </summary>
		public int NumeroLigne { get; set; }


		#endregion

		#region Constructeur

		public TableauViewModel(IDataAccess dataAccess, CurrentUserService currentUser, 
			NotificationService notificationSvc, IBlazorDownloadFileService blazorDownloadFileService,
			NavigationManager navigationManager)
		{
			DataAccess = dataAccess;
			CurrentUserService = currentUser;
			_notificationService = notificationSvc;
			DownloadFileService = blazorDownloadFileService;

			NavigationManager = navigationManager;
		}

		#endregion

		#region Public Methods

		/// <see cref="ITableauViewModel.LoadTableau(string)"/>
		public async Task LoadTableau(string idTableau)
		{
			TableauSelected = await DataAccess.GetTable(idTableau);
			IsUserProprietaire = (CurrentUserService.UserId == TableauSelected.IdUser);
			
			if(IsUserProprietaire)
			{
				ToutesLesEntrees = await DataAccess.GetValeurs(idTableau);
			}
			else
			{
				ToutesLesEntrees = await DataAccess.GetValeurs(idTableau, CurrentUserService.UserId);
			}


			NumeroLigne = GetLastLineNumber();

			InitNewData();
		}

		/// <see cref="ITableauViewModel.DeleteTableau"/>
		public async Task DeleteTableau()
		{
			await DataAccess.DeleteTableau(TableauSelected.IdTableau);

			// Revenir à la page des tableaux
			NavigationManager.NavigateTo("/mestableaux", true);
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

		/// <see cref="ITableauViewModel.OnChangeDate"/>
		public void OnChangeDate(ChangeEventArgs args, int idColonne)
		{
			var date = DateTime.Parse(args.Value.ToString()).ToString("d");
			GetValueColonne(idColonne).Value = date;
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

		/// <see cref="ITableauViewModel.ExportToExcel"/>
		public async Task ExportToExcel()
		{
			try
			{
				string fileName = TableauSelected.NomDuTableau + ".xlsx";

				using (var memStream = new MemoryStream())
				{
					using (var package = new ExcelPackage(memStream))
					{
						ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Ma Feuille");

						CreateColonnes(sheet, TableauSelected);
						AddValues(sheet, ToutesLesEntrees);

						byte[] fileTemp = package.GetAsByteArray();
						await DownloadFileService.DownloadFile(fileName, fileTemp, "application/octet-stream");
					}
				}
			}
			catch (Exception ex)
			{
				string errorMsg = "Erreur sur l'export du fichier Excel";
				Log.Error(ex, errorMsg);
				_notificationService.Notify(NotificationSeverity.Error, "Erreur", errorMsg, 3000);
			}
		}




		#endregion

		#region Private Methods

		/// <summary>
		/// Crée les colonnes dans la feuille Excel
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="tableau"></param>
		private void CreateColonnes(ExcelWorksheet sheet, Tableau tableau)
		{
			int i = 0;

			foreach (var colonne in tableau.Colonnes)
			{
				i++;
				string nomIndexColonne = GetColonneLetter(i) + 1;

				sheet.Cells[nomIndexColonne].Value = colonne.NomColonne;
				sheet.Cells[nomIndexColonne].Style.Font.Bold = true;

				sheet.Cells[nomIndexColonne].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
				sheet.Cells[nomIndexColonne].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
				sheet.Cells[nomIndexColonne].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
				sheet.Cells[nomIndexColonne].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
			}
		}

		/// <summary>
		/// Ajoute les valeurs dans le fichier Excel.
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="allValues"></param>
		private void AddValues(ExcelWorksheet sheet, List<List<ValueColonne>> allValues)
		{
			int i = 1;

			// Pour chaque ligne, il y a plusieurs valeurs pour chaque colonne.
			foreach (var ligne in allValues)
			{
				i++;
				foreach (var colonne in ligne)
				{
					string lettreColonne = GetColonneLetter(colonne.IdColonne);

					string indexCell = lettreColonne + i;

					if (TableauSelected.Colonnes.FirstOrDefault(x => x.IdColonne == colonne.IdColonne).TypeData == "number")
					{
						sheet.Cells[indexCell].Value = Convert.ToDecimal(colonne.Value);
					}
					else
					{
						sheet.Cells[indexCell].Value = colonne.Value;
					}
				}
			}
		}

		/// <summary>
		/// Donne la colonne Excel en fonction de l'index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		private string GetColonneLetter(int i)
		{
			switch (i)
			{
				case 1:
					return "A";
				case 2:
					return "B";
				case 3:
					return "C";
				case 4:
					return "D";
				case 5:
					return "E";
				case 6:
					return "F";
				case 7:
					return "G";
				case 8:
					return "H";
				case 9:
					return "I";
				case 10:
					return "J";
				case 11:
					return "K";
				case 12:
					return "L";
				case 13:
					return "M";
				case 14:
					return "N";
				case 15:
					return "O";
				case 16:
					return "P";
				case 17:
					return "Q";
				case 18:
					return "R";
				case 19:
					return "S";
				case 20:
					return "T";
				case 21:
					return "U";
				case 22:
					return "V";
				case 23:
					return "W";
				case 24:
					return "X";
				case 25:
					return "Y";
				case 26:
					return "Z";
				default:
					return "AA";
			}
		}

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

		/// <summary>
		/// Récupère le dernier numéro de ligne.
		/// </summary>
		/// <returns></returns>
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
