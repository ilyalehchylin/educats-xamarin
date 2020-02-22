using System.Net;
using System.Threading.Tasks;
using EduCATS.Data.Caching;
using EduCATS.Helpers.Networking.AppServices;
using EduCATS.Pages.Login.Models;

namespace EduCATS.Data
{
	public static class DataAccess
	{
		public static void ResetData()
		{
			DataCaching<object>.RemoveCache();
		}

		public async static Task<UserModel> Login(string username, string password)
		{
			var response = await AppServices.Login(username, password);

			if (response.Value == HttpStatusCode.OK) {
				return response.Key;
			}

			return null;
		}
	}
}