using System;
using System.Collections.Generic;
using System.Text;

namespace AccessData.Models
{
	public class ColonneModel
	{
		public Guid TableId { get; set; }

		public int IdColonne { get; set; }


		public string NomColonne
		{
			get { return _nomColonne; }
			set { _nomColonne = value.Replace("'", "’"); }
		}
		private string _nomColonne;



		public string Description
		{
			get { return _description; }
			set { _description = value.Replace("'", "’"); }
		}
		private string _description;

		public string TypeData { get; set; }


		public ColonneModel()
		{
			IdColonne = 1;
		}
	}
}
