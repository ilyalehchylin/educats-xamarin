using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	public class UserModel
	{
		[JsonProperty("UserName")]
		public string Username { get; set; }

		[JsonProperty("UserId")]
		public int UserId { get; set; }
	}
}
