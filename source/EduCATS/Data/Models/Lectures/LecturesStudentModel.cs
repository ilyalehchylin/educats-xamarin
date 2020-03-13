using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Lectures
{
	public class LecturesStudentModel
	{
		[JsonProperty("Marks")]
		public IList<LecturesDetailsModel> VisitingList { get; set; }

		[JsonProperty("StudentId")]
		public int StudentId { get; set; }

		[JsonProperty("Login")]
		public string Login { get; set; }

		[JsonProperty("StudentName")]
		public string StudentName { get; set; }
	}
}
