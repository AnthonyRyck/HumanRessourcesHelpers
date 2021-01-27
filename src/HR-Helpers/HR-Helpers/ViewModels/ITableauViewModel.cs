using AccessData.Models;
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


		RadzenGrid<ColonneModel> ColonneModelGrid { get; set; }

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
	}
}
