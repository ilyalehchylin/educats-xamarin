using Newtonsoft.Json;

namespace EduCATS.Data.Models.Groups
{
	/// <summary>
	/// Group details model.
	/// </summary>
	public class GroupItemModel
	{
		/// <summary>
		/// Group ID.
		/// </summary>
		[JsonProperty("GroupId")]
		public int GroupId { get; set; }

		/// <summary>
		/// Group name.
		/// </summary>
		[JsonProperty("GroupName")]
		public string GroupName { get; set; }
	}
}
