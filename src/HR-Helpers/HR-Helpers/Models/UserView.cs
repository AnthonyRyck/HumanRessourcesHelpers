using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.Models
{
	public class UserView
	{
		public IdentityUser User { get; set; }

		public string Role { get; set; }
	}
}
