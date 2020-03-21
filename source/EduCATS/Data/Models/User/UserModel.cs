using Newtonsoft.Json;

namespace EduCATS.Data.Models.User
{
	public class UserModel
	{
		[JsonProperty("UserName")]
		public string Username { get; set; }

		[JsonProperty("UserId")]
		public int UserId { get; set; }
	}
}
