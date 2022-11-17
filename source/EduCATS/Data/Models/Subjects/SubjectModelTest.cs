using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Data.Models
{
	class SubjectModelTest
	{
		[JsonProperty("Subjects")]
		public List<SubjectModel> Subjects { get; set; }
	}
}
