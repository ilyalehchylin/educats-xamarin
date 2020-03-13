using Newtonsoft.Json;

namespace EduCATS.Data.Models.Lectures
{
	public class LecturesDetailsModel
	{
		[JsonProperty("Date")]
		public string Date { get; set; }

		[JsonProperty("Mark")]
		public string Mark { get; set; }
	}
}
