using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// News model.
	/// </summary>
	public class NewsModel
	{
		/// <summary>
		/// News title.
		/// </summary>
		[JsonProperty("Title")]
		public string Title { get; set; }

		/// <summary>
		/// News body.
		/// </summary>
		/// <remarks>
		/// Retrieved in <c>HTML</c> format.
		/// </remarks>
		[JsonProperty("Body")]
		public string Body { get; set; }

		/// <summary>
		/// News publish/update date.
		/// </summary>
		[JsonProperty("EditDate")]
		public string Date { get; set; }

		/// <summary>
		/// Subject ID.
		/// </summary>
		[JsonProperty("SubjectId")]
		public int SubjectId { get; set; }

		/// <summary>
		/// Subject details.
		/// </summary>
		[JsonProperty("Subject")]
		public NewsSubjectModel Subject { get; set; }

		/// <summary>
		/// News ID.
		/// </summary>
		[JsonProperty("Id")]
		public int Id { get; set; }
	}
}
