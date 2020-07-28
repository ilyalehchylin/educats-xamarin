using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Pages.Parental.FindGroup.Models
{
	public class GroupInfo
	{
		[JsonProperty("Code")]
		public string Code { get; set; }

		[JsonProperty("DataD")]
		public List<object> DataD { get; set; }

		[JsonProperty("Message")]
		public string Message { get; set; }

		[JsonProperty("GroupId")]
		public int GroupId { get; set; }

		[JsonProperty("Subjects")]
		public List<Subject> Subjects { get; set; }




	}
}
