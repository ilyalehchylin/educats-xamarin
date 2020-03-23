using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Groups
{
	/// <summary>
	/// Group model.
	/// </summary>
	public class GroupModel
	{
		/// <summary>
		/// Group list.
		/// </summary>
		[JsonProperty("Groups")]
		public IList<GroupItemModel> GroupsList { get; set; }
	}
}
