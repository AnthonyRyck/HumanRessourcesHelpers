using AccessData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.ViewModels
{
	public interface IMesTableauxViewModel
	{
		/// <summary>
		/// Liste de tous les tableaux de l'utilisateur
		/// </summary>
		IEnumerable<Tableau> TousMesTableau { get; set; }

		/// <summary>
		/// Permet la création d'un nouveau tableau.
		/// </summary>
		void AddTableau();

		/// <summary>
		/// Récupère les tableaux de l'utilisateur
		/// </summary>
		/// <returns></returns>
		Task GetMesTableaux();

		/// <summary>
		/// Permet d'ouvrir la page de ce tableau.
		/// </summary>
		/// <param name="idTableau"></param>
		/// <returns></returns>
		Task OpenThisTableau(string idTableau);
	}
}
