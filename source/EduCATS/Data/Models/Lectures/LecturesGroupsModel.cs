using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Lectures
{
	/// <summary>
	/// Lectures visiting group model.
	/// </summary>
	public class LecturesGroupsModel
	{
		/// <summary>
		/// Group ID.
		/// </summary>
		[JsonProperty("GroupId")]
		public int GroupId { get; set; }

		/// <summary>
		/// Lectures visiting list.
		/// </summary>
		[JsonProperty("LecturesMarksVisiting")]
		public IList<LecturesStudentModel> LecturesVisiting { get; set; }
	}
}
