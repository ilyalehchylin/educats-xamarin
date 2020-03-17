using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Labs
{
	public class LabsModel
	{
		[JsonProperty("Labs")]
		public IList<LabsDetailsModel> Labs { get; set; }

		[JsonProperty("ScheduleProtectionLabs")]
		public IList<LabsScheduleProtectionModel> ScheduleProtectionLabs { get; set; }
	}
}
