using Newtonsoft.Json;

namespace EduCATS.Networking.Models.Testing
{
	public class TestAnswerDetailsPostModel
	{
		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("IsCorrect")]
		public int IsCorrect { get; set; }

		[JsonProperty("Content")]
		public string Content { get; set; }

		public TestAnswerDetailsPostModel(int id, int isCorrect)
		{
			Id = id;
			IsCorrect = isCorrect;
		}

		public TestAnswerDetailsPostModel(int id, string content)
		{
			Id = id;
			Content = content;
		}
	}
}
