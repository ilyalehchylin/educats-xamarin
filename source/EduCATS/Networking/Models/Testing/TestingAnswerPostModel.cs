using Newtonsoft.Json;

namespace EduCATS.Networking.Models.Testing
{
	public class TestingAnswerPostModel
	{
		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("IsCorrect")]
		public int IsCorrect { get; set; }

		[JsonProperty("Content")]
		public string Content { get; set; }

		public TestingAnswerPostModel(int id, int isCorrect)
		{
			Id = id;
			IsCorrect = isCorrect;
		}

		public TestingAnswerPostModel(int id, string content)
		{
			Id = id;
			Content = content;
		}
	}
}
