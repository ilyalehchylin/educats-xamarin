using System.Net;
using System.Threading.Tasks;
using EduCATS.Data.Caching;
using EduCATS.Data.Models;
using EduCATS.Data.Models.User;
using EduCATS.Helpers.Json;
using EduCATS.Networking.AppServices;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Data
{
	public static partial class DataAccess
	{
		const string profileInfoKey = "PROFILE_INFO_KEY";

		public static void ResetData()
		{
			DataCaching<object>.RemoveCache();
		}

		public async static Task<DataModel> Login(string username, string password)
		{
			if (checkConnection()) {
				var response = await AppServices.Login(username, password);

				switch (response.Value) {
					case HttpStatusCode.OK:
						var data = parseResponse(response, isCaching: false);
						return JsonController<UserModel>.ConvertJsonToObject(data);
					default:
						return getError(CrossLocalization.Translate("login_error_text"));
				}
				
			}

			return getError();
		}

		public async static Task<DataModel> GetProfileInfo(string username)
		{
			if (checkConnection()) {
				var response = await AppServices.GetProfileInfo(username);

				switch (response.Value) {
					case HttpStatusCode.OK:
						var data = parseResponse(response);
						return data != null ?
							JsonController<UserProfileModel>.ConvertJsonToObject(data) :
							getError();
					default:
						return getError(CrossLocalization.Translate("login_user_profile_error_text"));
				}
			} else {
				var data = getDataFromCache(profileInfoKey);
				return JsonController<UserProfileModel>.ConvertJsonToObject(data);
			}
		}

		static DataModel getError(string message = null)
		{
			return new DataModel {
				IsError = true,
				ErrorMessage = message ?? CrossLocalization.Translate("common_unexpected_error_text")
			};
		}
	}
}