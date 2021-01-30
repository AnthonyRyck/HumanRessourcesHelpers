using AccessData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR_Helpers.Data.ModelValidation
{
	public class TableauModelValidation
	{
		[Required(ErrorMessage = "Doit avoir un nom.")]
		[StringLength(32, ErrorMessage = "Le nom est trop long, 32 caractères max")]
		public string NomDuTableau { get; set; }

		[Required(ErrorMessage = "Manque une description.")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Choisir une date de fin")]
		public DateTime? DateFinInscription { get; set; }

		[Required(ErrorMessage = "Il faut au moins une colonne")]
		public List<ColonneModel> Colonnes { get; set; }



		public Guid IdTable { get; set; }
		public int IdColonne { get; internal set; }

		public Tableau ToTableau(string idUser)
		{
			return new Tableau()
			{
				NomDuTableau = NomDuTableau,
				Description = Description,
				DateFinInscription = DateFinInscription.Value,
				IdTableau = IdTable,
				IdUser = idUser,
				Colonnes = Colonnes
			};
		}

	}
}
