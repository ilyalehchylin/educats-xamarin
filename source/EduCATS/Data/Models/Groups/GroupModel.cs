using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Groups
{
	public class GroupModel : DataModel
	{
		[JsonProperty("Groups")]
		public IList<GroupItemModel> GroupsList { get; set; }
	}
}
