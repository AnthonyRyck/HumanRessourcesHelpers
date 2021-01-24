using System;
using System.Collections.Generic;
using System.Text;

namespace AccessData.Models
{
	public class ColonneModel
	{
		public Guid TableId { get; set; }

		public int IdColonne { get; set; }

		public string NomColonne { get; set; }

		public string Description { get; set; }

		public string TypeData { get; set; }


		public ColonneModel()
		{
			IdColonne = 1;
		}
	}
}
