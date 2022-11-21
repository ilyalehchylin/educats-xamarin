using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Data.Models
{
	public class FilesModelTest
	{
		[JsonProperty("Attachment")]
		public List<FileDetailsModel> Files { get; set; }
	}
}
