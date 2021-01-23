using HR_Helpers.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.ViewModels
{
	public interface IUsersViewModel
	{
		List<UserView> AllUsers { get; set; }

		NavigationManager Navigation { get; set; }

		UserManager<IdentityUser> UserManager { get; set; }

		bool ShowResetMdp { get; set; }

		void OnChangeRole(ChangeEventArgs e, string idUser);

		void DeleteUser(string idUser);

		void OpenChangeMdp(string idUser);

		void CancelChangeMdp();

		Task SetNewPassword(string newPassword);
	}
}
