using AccessData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.Data
{
	public interface IDataAccess
	{
		/// <summary>
		/// Récupère les tableaux pour un utilisateurs.
		/// </summary>
		/// <param name="idUser"></param>
		/// <returns></returns>
		Task<IEnumerable<Tableau>> GetUserTableaux(string idUser);


		/// <summary>
		/// Sauvegarde un nouveau tableau.
		/// </summary>
		/// <param name="nouveauTableau"></param>
		/// <returns></returns>
		Task SaveNewTable(Tableau nouveauTableau);

		/// <summary>
		/// Récupère un Tableau par rapport à son ID
		/// </summary>
		/// <returns></returns>
		Task<Tableau> GetTable(string idTableau);

		/// <summary>
		/// Sauvegarde les valeurs pour un tableau donné.
		/// </summary>
		/// <param name="valeurs"></param>
		/// <returns></returns>
		Task SaveData(List<ValueColonne> valeurs);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		Task<List<ValueColonne>> GetDatas();

		Task<List<List<ValueColonne>>> GetAllDatas();
		
		Task SaveData(List<List<ValueColonne>> valeurs);
	}
}
