﻿using AccessData.Models;
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
		/// Récupère la liste des valeurs pour un tableau et un utilisateurs
		/// </summary>
		/// <param name="idTableau"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<List<List<ValueColonne>>> GetValeurs(string idTableau, string userId);

		/// <summary>
		/// Récupère la liste des valeurs pour un tableau, et pour tous les utilisateurs.
		/// </summary>
		/// <param name="idTableau"></param>
		/// <returns></returns>
		Task<List<List<ValueColonne>>> GetValeurs(string idTableau);

		
		Task SaveData(List<List<ValueColonne>> valeurs);

		/// <summary>
		/// Permet la suppression du tableau et des valeurs associées.
		/// </summary>
		/// <param name="idTableau"></param>
		/// <returns></returns>
		Task DeleteTableau(Guid idTableau);

		/// <summary>
		/// Supprime une ligne de valeur pour un utilisateur et dans le tableau donné.
		/// </summary>
		/// <param name="idTableau"></param>
		/// <param name="idUser"></param>
		/// <param name="numeroLigne"></param>
		/// <returns></returns>
		Task DeleteRow(string idTableau, string idUser, int numeroLigne);

		/// <summary>
		/// Met à jour une ligne de valeur.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		Task UpdateRow(List<ValueColonne> value);

		
	}
}
