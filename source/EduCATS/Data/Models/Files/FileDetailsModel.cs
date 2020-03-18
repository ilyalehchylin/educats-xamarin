using Newtonsoft.Json;

namespace EduCATS.Data.Models.Files
{
	public class FileDetailsModel
	{
		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("FileName")]
		public string FileName { get; set; }

		[JsonProperty("PathName")]
		public string PathName { get; set; }

		[JsonProperty("AttachmentType")]
		public int AttachmentType { get; set; }

		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("IsNew")]
		public bool IsNew { get; set; }
	}
}
