using Newtonsoft.Json;

namespace EduCATS.Networking.Models.Testing
{
	/// <summary>
	/// Test answer details <c>POST</c> model.
	/// </summary>
	public class TestAnswerDetailsPostModel
	{
		/// <summary>
		/// Constructor with answer ID and correctness.
		/// </summary>
		/// <param name="id">Answer ID.</param>
		/// <param name="isCorrect">Is answer correct.</param>
		public TestAnswerDetailsPostModel(int id, int isCorrect)
		{
			Id = id;
			IsCorrect = isCorrect;
		}

		/// <summary>
		/// Constructor with answer ID and content.
		/// </summary>
		/// <param name="id">Answer ID.</param>
		/// <param name="content">Answer content.</param>
		public TestAnswerDetailsPostModel(int id, string content)
		{
			Id = id;
			Content = content;
		}

		/// <summary>
		/// Answer ID.
		/// </summary>
		[JsonProperty("Id")]
		public int Id { get; set; }

		/// <summary>
		/// Is answer correct.
		/// </summary>
		[JsonProperty("IsCorrect")]
		public int IsCorrect { get; set; }

		/// <summary>
		/// Answer content.
		/// </summary>
		[JsonProperty("Content")]
		public string Content { get; set; }
	}
}
