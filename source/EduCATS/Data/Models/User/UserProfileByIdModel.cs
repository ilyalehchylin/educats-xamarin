using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	public class UserProfileByIdModel
	{
		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("Name")]
		public string Name { get; set; }
	}
}
