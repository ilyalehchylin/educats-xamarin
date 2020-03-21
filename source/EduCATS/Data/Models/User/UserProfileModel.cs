using Newtonsoft.Json;

namespace EduCATS.Data.Models.User
{
	public class UserProfileModel
	{
		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("UserType")]
		public string UserType { get; set; }

		[JsonProperty("Skill")]
		public string Skill { get; set; }

		[JsonProperty("SkypeContact")]
		public string SkypeContact { get; set; }

		[JsonProperty("Email")]
		public string Email { get; set; }

		[JsonProperty("Phone")]
		public string Phone { get; set; }

		[JsonProperty("About")]
		public string About { get; set; }

		[JsonProperty("Avatar")]
		public string Avatar { get; set; }

		[JsonProperty("Group")]
		public string GroupName { get; set; }

		[JsonProperty("GroupId")]
		public int GroupId { get; set; }
	}
}
