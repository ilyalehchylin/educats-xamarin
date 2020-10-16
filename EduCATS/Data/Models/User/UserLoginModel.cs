using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	public class UserLoginModel
	{
		[JsonProperty("UserLogin")]
		public string UserLogin { get; set; }
	}
}
