using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Files
{
	public class FilesModel
	{
		[JsonProperty("Lectures")]
		public List<FileDetailsModel> Lectures { get; set; }

		[JsonProperty("Labs")]
		public List<FileDetailsModel> Labs { get; set; }

		[JsonProperty("Practicals")]
		public List<FileDetailsModel> Practicals { get; set; }
	}
}
