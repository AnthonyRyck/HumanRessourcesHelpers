using AccessData.Models;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.ViewModels
{
	public interface ITableauViewModel
	{
		/// <summary>
		/// Tableau sélectionné
		/// </summary>
		Tableau TableauSelected { get; set; }

		/// <summary>
		/// Pour afficher le CustomDialog
		/// </summary>
		bool ShowNouvelleEntree { get; set; }

		/// <summary>
		/// Liste de toutes les valeurs.
		/// </summary>
		List<List<ValueColonne>> ToutesLesEntrees { get; set; }

		/// <summary>
		/// Valeur saisie pour l'utilisateur
		/// </summary>
		List<ValueColonne> ValeursSaisies { get; set; }

		/// <summary>
		/// Indicateur si l'utilisateur est le propriétaire du tableau.
		/// </summary>
		bool IsUserProprietaire { get; set; }

		/// <summary>
		/// Indicateur si le tableau est "terminé".
		/// </summary>
		bool IsOutDated { get; set; }

		/// <summary>
		/// Charge le tableau avec l'ID donnée en paramètre
		/// </summary>
		/// <param name="idTableau"></param>
		/// <returns></returns>
		Task LoadTableau(string idTableau);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="idColonne"></param>
		/// <returns></returns>
		ValueColonne GetValueColonne(int idColonne);

		/// <summary>
		/// Permet d'ouvrir le Dialog
		/// </summary>
		void OpenDialog();

		/// <summary>
		/// Permet de fermer le Dialog
		/// </summary>
		void OnCloseData();

		/// <summary>
		/// Enregistre une nouvelle valeur et ferme le dialog
		/// </summary>
		void SaveAndClose();

		/// <summary>
		/// Enregistre une nouvelle valeur et propose une autre.
		/// </summary>
		Task SaveAndNewData();

		/// <summary>
		/// Event dans le cas d'un format date.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="idColonne"></param>
		void OnChangeDate(ChangeEventArgs args, int idColonne);

		/// <summary>
		/// Exporte les valeurs vers Excels
		/// </summary>
		/// <returns></returns>
		Task ExportToExcel();

		/// <summary>
		/// Supprime toutes les données du tableau
		/// </summary>
		/// <returns></returns>
		Task DeleteTableau();
	}
}
