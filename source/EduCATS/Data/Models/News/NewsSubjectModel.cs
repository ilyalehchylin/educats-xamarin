using Newtonsoft.Json;

namespace EduCATS.Data.Models.News
{
	public class NewsSubjectModel
	{
		[JsonProperty("Name")]
		public string Name { get; set; }
	}
}
