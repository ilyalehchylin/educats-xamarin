using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// Statistics model.
	/// </summary>
	public class StatsModel
	{
		/// <summary>
		/// Students statistics list.
		/// </summary>
		[JsonProperty("Students")]
		public IList<StatsStudentModel> Students { get; set; }
	}
}
