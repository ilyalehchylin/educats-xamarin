using Newtonsoft.Json;

namespace EduCATS.Data.Models.User
{
	public class UserLoginModel
	{
		[JsonProperty("UserLogin")]
		public string UserLogin { get; set; }
	}
}