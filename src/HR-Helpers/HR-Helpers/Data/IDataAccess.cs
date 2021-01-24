using AccessData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.Data
{
	public interface IDataAccess
	{
		Task SaveNewTable(Tableau nouveauTableau);

		Task<Tableau> GetTable();

		Task SaveData(List<ValueColonne> valeurs);
		Task<List<ValueColonne>> GetDatas();



		Task<List<List<ValueColonne>>> GetAllDatas();
		Task SaveData(List<List<ValueColonne>> valeurs);
	}
}
