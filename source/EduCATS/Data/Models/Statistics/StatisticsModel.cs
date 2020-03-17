using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Statistics
{
	public class StatisticsModel
	{
		[JsonProperty("Students")]
		public IList<StatisticsStudentModel> Students { get; set; }
	}
}