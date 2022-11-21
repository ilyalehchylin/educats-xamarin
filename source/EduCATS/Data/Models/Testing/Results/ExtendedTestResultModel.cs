using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Data.Models
{
	public class ExtendedTestResultModel
	{

		/// <summary>
		/// Extended data
		/// </summary>
		[JsonProperty("Data")]
		public List<KeyValuePair<string, object>> Data { get; set; }
	}
}
