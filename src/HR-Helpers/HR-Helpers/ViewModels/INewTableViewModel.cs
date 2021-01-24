using AccessData.Models;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.ViewModels
{
	public interface INewTableViewModel
	{
		/// <summary>
		/// Objet nouveau Tableau.
		/// </summary>
		Tableau NouveauTableau { get; set; }

		/// <summary>
		/// Pour l'ajout d'une nouvelle colonne.
		/// </summary>
		ColonneModel NouvelleColonne { get; set; }

		/// <summary>
		/// Pour afficher le Dialog pour nouvelle colonne.
		/// </summary>
		bool ShowNewColonne { get; set; }

		/// <summary>
		/// 
		/// </summary>
		RadzenGrid<ColonneModel> ColonneModelGrid { get; set; }

		/// <summary>
		/// Crée un nouveau tableau en base de donnée.
		/// </summary>
		/// <returns></returns>
		Task CreateTable();

		/// <summary>
		/// Permet d'ajouter une colonne
		/// </summary>
		/// <returns></returns>
		Task OnClickAddColonne();

		/// <summary>
		/// Changement de la valeur du DataType
		/// </summary>
		/// <param name="e"></param>
		void OnChangeDataType(ChangeEventArgs e);

		/// <summary>
		/// Sauvegarde la colonne et ajoute une nouvelle colonne
		/// </summary>
		/// <returns></returns>
		Task SaveAndNewColonne();

		/// <summary>
		/// Permet de fermer la fenêtre pour nouvelle colonne.
		/// </summary>
		void CloseNewColonne();

		/// <summary>
		/// Termine et sauvegarde la colonne
		/// </summary>
		void CloseAndSaveNewColonne();

	}
}
