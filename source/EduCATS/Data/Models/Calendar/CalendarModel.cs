using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Calendar
{
	public class CalendarModel
	{
		[JsonProperty("Labs")]
		public IList<CalendarSubjectModel> Labs { get; set; }

		[JsonProperty("Lect")]
		public IList<CalendarSubjectModel> Lectures { get; set; }
	}
}