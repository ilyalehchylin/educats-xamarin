using Newtonsoft.Json;

namespace EduCATS.Data.Models.News
{
	public class NewsModel
	{
		[JsonProperty("Title")]
		public string Title { get; set; }

		[JsonProperty("Body")]
		public string Body { get; set; }

		[JsonProperty("EditDate")]
		public string EditDate { get; set; }

		[JsonProperty("SubjectId")]
		public int SubjectId { get; set; }

		[JsonProperty("Subject")]
		public NewsSubjectModel Subject { get; set; }

		[JsonProperty("Id")]
		public int Id { get; set; }
	}
}
