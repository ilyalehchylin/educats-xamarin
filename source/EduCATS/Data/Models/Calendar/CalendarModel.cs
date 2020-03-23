using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Calendar
{
	/// <summary>
	/// Calendar model.
	/// </summary>
	public class CalendarModel
	{
		/// <summary>
		/// Laboratory works list in Calendar.
		/// </summary>
		[JsonProperty("Labs")]
		public IList<CalendarSubjectModel> Labs { get; set; }

		/// <summary>
		/// Lectures list in Calendar.
		/// </summary>
		[JsonProperty("Lect")]
		public IList<CalendarSubjectModel> Lectures { get; set; }
	}
}
