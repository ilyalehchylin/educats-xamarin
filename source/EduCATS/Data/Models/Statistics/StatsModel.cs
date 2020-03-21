using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Statistics
{
	public class StatsModel
	{
		[JsonProperty("Students")]
		public IList<StatsStudentModel> Students { get; set; }
	}
}
