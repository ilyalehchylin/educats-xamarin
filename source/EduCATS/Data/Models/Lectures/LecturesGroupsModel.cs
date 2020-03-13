using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Lectures
{
	public class LecturesGroupsModel
	{
		[JsonProperty("GroupId")]
		public int GroupId { get; set; }

		[JsonProperty("LecturesMarksVisiting")]
		public IList<LecturesStudentModel> LecturesVisiting { get; set; }
	}
}
