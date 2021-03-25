using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Pages.Parental.FindGroup.Models
{
	public class Subject
	{
		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("ShortName")]
		public string ShortName { get; set; }

		[JsonProperty("IsNeededCopyToBts")]
		public bool IsNeededCopyToBts { get; set; }
	}
}

