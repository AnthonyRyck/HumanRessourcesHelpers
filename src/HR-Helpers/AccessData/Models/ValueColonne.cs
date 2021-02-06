using System;
using System.Collections.Generic;
using System.Text;

namespace AccessData.Models
{
	public class ValueColonne
	{
		public Guid IdTableau { get; set; }

		public int NumeroLigne { get; set; }

		public int IdColonne { get; set; }

		public string IdUser { get; set; }

		public string Value
		{
			get { return _value; }
			set { _value = value.Replace("'", "’"); }
		}
		private string _value;

	}
}
