using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.SaveMarks.Practicals
{
	public class GroupAndSubjModel
	{
		[JsonProperty("subjectId")]
		public int SubjectId { get; set; }
		[JsonProperty("groupId")]
		public int GroupId { get; set; }
	}
}
