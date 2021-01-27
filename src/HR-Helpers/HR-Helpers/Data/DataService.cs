using AccessData;
using AccessData.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.Data
{
	public class DataService : IDataAccess
	{
		private SqlContextAccess _sqlContext;



		public DataService(SqlContextAccess sqlContext)
		{
			_sqlContext = sqlContext;
		}

		#region IDataAccess implementation

		///<inheritdoc cref="IDataAccess.GetUserTableaux(string)"/>
		public async Task<IEnumerable<Tableau>> GetUserTableaux(string idUser)
		{
			IEnumerable<Tableau> tableaux = null;

			try
			{
				tableaux = await _sqlContext.GetTableauByUser(idUser);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erreur sur la récupération des tableaux de l'utilisateur : " + idUser);
			}

			return tableaux;
		}

		///<inheritdoc cref="IDataAccess.GetAllDatas"/>
		public Task<List<List<ValueColonne>>> GetAllDatas()
		{
			throw new NotImplementedException();
		}

		///<inheritdoc cref="IDataAccess.GetDatas"/>
		public Task<List<ValueColonne>> GetDatas()
		{
			throw new NotImplementedException();
		}

		///<inheritdoc cref="IDataAccess.GetTable(string)"/>
		public async Task<Tableau> GetTable(string idTableau)
		{
			try
			{
				return await _sqlContext.GetTableau(idTableau);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erreur récupération du tableau : " + idTableau);
				throw;
			}
		}

		///<inheritdoc cref="IDataAccess.SaveData(List{ValueColonne})"/>
		public async Task SaveData(List<ValueColonne> valeurs)
		{
			try
			{
				await _sqlContext.AddValeurs(valeurs);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erreur savegarde d'une ligne de valeur.");
				throw;
			}
		}

		///<inheritdoc cref="IDataAccess.SaveData(List{List{ValueColonne}})"/>
		public Task SaveData(List<List<ValueColonne>> valeurs)
		{
			throw new NotImplementedException();
		}

		///<inheritdoc cref="IDataAccess.SaveNewTable(Tableau)"/>
		public async Task SaveNewTable(Tableau nouveauTableau)
		{
			await _sqlContext.AddTableau(nouveauTableau);
		}

		/// <see cref="IDataAccess.GetValeurs(string, string)"/>
		public async Task<List<List<ValueColonne>>> GetValeurs(string idTableau, string userId)
		{
			try
			{
				return await _sqlContext.GetValeurs(idTableau, userId);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erreur récupération des valeurs - IdTableau:" + idTableau + " - userId:"+ userId);
				throw;
			}
		}

		#endregion

	}
}
