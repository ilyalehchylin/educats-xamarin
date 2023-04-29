using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Data.Models
{
	public class ConceptModelTest
	{
		[JsonProperty("Concept")]
		public object Concept { get; set; }
	}
}
