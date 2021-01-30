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

		/// <see cref="IDataAccess.GetValeurs(string)"/>
		public async Task<List<List<ValueColonne>>> GetValeurs(string idTableau)
		{
			try
			{
				return await _sqlContext.GetValeurs(idTableau);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erreur récupération des valeurs - IdTableau:" + idTableau);
				throw;
			}
		}

		/// <see cref="IDataAccess.DeleteTableau(Guid)"/>
		public async Task DeleteTableau(Guid idTableau)
		{
			try
			{
				await _sqlContext.DeleteTableau(idTableau);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erreur sur la suppression du tableau - IdTableau:" + idTableau.ToString());
				throw;
			}
		}

		/// <see cref="IDataAccess.DeleteRow(string, string, int)"/>
		public async Task DeleteRow(string idTableau, string idUser, int numeroLigne)
		{
			try
			{
				await _sqlContext.DeleteRow(idTableau, idUser, numeroLigne);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erreur sur la suppression d'une linge - IdTableau:" + idTableau + " - userId:" + idUser + " -linge:" + numeroLigne);
				throw;
			}
		}

		/// <see cref="IDataAccess.UpdateRow(List{ValueColonne})"/>
		public async Task UpdateRow(List<ValueColonne> value)
		{
			try
			{
				await _sqlContext.UpdateValeurs(value);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Erreur sur la mise à jour d'une linge");
				throw;
			}
		}

		#endregion

	}
}
