using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.Services
{
	public class CurrentUserService
	{
		#region Properties

		/// <summary>
		/// ID de l'utilisateur
		/// </summary>
		public string UserId
		{
			get
			{
				if (string.IsNullOrEmpty(_userId))
				{
					_userId = GetId().Result;
				}

				return _userId;
			}
		}
		private string _userId;

		private AuthenticationStateProvider AuthenticationStateProvider;

		#endregion


		public CurrentUserService(AuthenticationStateProvider stateProvider)
		{
			AuthenticationStateProvider = stateProvider;
		}

		#region Public Methods

		/// <summary>
		/// Récupère l'ID de l'utilisateur (IdentityUser)
		/// </summary>
		/// <returns></returns>
		public async Task<string> GetId()
		{
			var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
			var user = authState.User;

			string userId;

			if (user.Identity.IsAuthenticated)
			{
				var claims = user.Claims;
				userId = claims.Where(x => x.Type.Contains("nameidentifier")).FirstOrDefault().Value;
			}
			else
			{
				userId = null;
			}

			return userId;
		}

		#endregion
	}
}
