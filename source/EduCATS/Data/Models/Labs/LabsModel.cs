using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Labs
{
	public class LabsModel
	{
		[JsonProperty("Labs")]
		public IList<LabDetailsModel> Labs { get; set; }

		[JsonProperty("ScheduleProtectionLabs")]
		public IList<LabProtectionModel> ProtectionLabs { get; set; }
	}
}
