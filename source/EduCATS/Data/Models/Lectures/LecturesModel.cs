using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Lectures
{
	public class LecturesModel : DataModel
	{
		[JsonProperty("GroupsVisiting")]
		public IList<LecturesGroupsModel> GroupsVisiting { get; set; }
	}
}
