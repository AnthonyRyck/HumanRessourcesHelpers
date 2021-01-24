using AccessData;
using AccessData.Models;
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

		public Task<List<List<ValueColonne>>> GetAllDatas()
		{
			throw new NotImplementedException();
		}

		public Task<List<ValueColonne>> GetDatas()
		{
			throw new NotImplementedException();
		}

		public Task<Tableau> GetTable()
		{
			throw new NotImplementedException();
		}

		public Task SaveData(List<ValueColonne> valeurs)
		{
			throw new NotImplementedException();
		}

		public Task SaveData(List<List<ValueColonne>> valeurs)
		{
			throw new NotImplementedException();
		}

		public async Task SaveNewTable(Tableau nouveauTableau)
		{
			await _sqlContext.AddTableau(nouveauTableau);
		}

		#endregion

	}
}
