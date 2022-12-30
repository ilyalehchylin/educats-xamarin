using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	public class FileDetailsModelTest
	{
		/// <summary>
		/// File ID.
		/// </summary>
		[JsonProperty("IdFile")]
		public int Id { get; set; }

		/// <summary>
		/// File size.
		/// </summary>
		[JsonProperty("Size")]
		public string Size { get; set; }
	}
}
