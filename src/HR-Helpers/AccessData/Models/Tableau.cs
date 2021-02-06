using System;
using System.Collections.Generic;
using System.Text;

namespace AccessData.Models
{
	public class Tableau
	{
		public Guid IdTableau { get; set; }

		public string IdUser { get; set; }

		
		public string NomDuTableau
		{
			get { return _nomDuTableau; }
			set { _nomDuTableau = value.Replace("'", "’"); }
		}
		private string _nomDuTableau;



		public string Description
		{
			get { return _description; }
			set { _description = value.Replace("'", "’"); }
		}
		private string _description;



		public DateTime DateFinInscription { get; set; }

		public List<ColonneModel> Colonnes { get; set; }

		public Tableau()
		{
			Colonnes = new List<ColonneModel>();
			DateFinInscription = DateTime.Now;
		}
	}
}
