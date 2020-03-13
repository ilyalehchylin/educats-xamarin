using Newtonsoft.Json;

namespace EduCATS.Data.Models.Groups
{
	public class GroupItemModel
	{
		[JsonProperty("GroupId")]
		public int GroupId { get; set; }

		[JsonProperty("GroupName")]
		public string GroupName { get; set; }
	}
}
