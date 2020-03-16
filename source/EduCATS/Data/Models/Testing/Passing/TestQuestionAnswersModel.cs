using Newtonsoft.Json;

namespace EduCATS.Data.Models.Testing.Passing
{
	public class TestQuestionAnswersModel
	{
		[JsonProperty("Content")]
		public string Content { get; set; }

		[JsonProperty("СorrectnessIndicator")]
		public int СorrectnessIndicator { get; set; }

		[JsonProperty("Id")]
		public int Id { get; set; }
	}
}
