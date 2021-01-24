using System;
using System.Collections.Generic;
using System.Text;

namespace AccessData.Models
{
	public class Tableau
	{
		public Guid IdTableau { get; set; }

		public string IdUser { get; set; }

		public string NomDuTableau { get; set; }

		public string Description { get; set; } = string.Empty;

		public DateTime DateFinInscription { get; set; }

		public List<ColonneModel> Colonnes { get; set; }

		public Tableau()
		{
			Colonnes = new List<ColonneModel>();
			DateFinInscription = DateTime.Now;
		}
	}
}
