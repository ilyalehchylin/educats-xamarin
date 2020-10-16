using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// Lectures model.
	/// </summary>
	public class LecturesModel
	{
		/// <summary>
		/// Lectures group visiting list.
		/// </summary>
		[JsonProperty("GroupsVisiting")]
		public IList<LecturesGroupsModel> GroupsVisiting { get; set; }
	}
}
