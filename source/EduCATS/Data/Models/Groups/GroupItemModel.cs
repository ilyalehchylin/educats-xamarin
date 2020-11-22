using Newtonsoft.Json;

namespace EduCATS.Data.Models
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

		/// <summary>
		/// Group name.
		/// </summary>
		[JsonProperty("Name")]
		public string Name { get; set; }

		/// <summary>
		/// Group ID.
		/// </summary>
		[JsonProperty("Id")]
		public int Id { get; set; }
	}
}
