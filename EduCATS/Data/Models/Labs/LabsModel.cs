using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// Laboratory works model.
	/// </summary>
	public class LabsModel
	{
		/// <summary>
		/// Labs details list.
		/// </summary>
		[JsonProperty("Labs")]
		public IList<LabDetailsModel> Labs { get; set; }

		/// <summary>
		/// Schedult proptection labs list.
		/// </summary>
		[JsonProperty("ScheduleProtectionLabs")]
		public IList<LabProtectionModel> ProtectionLabs { get; set; }
	}
}
