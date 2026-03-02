using System.Collections.Generic;
using EduCATS.Data.Models;
using Newtonsoft.Json;

namespace EduCATS.Data.Models.Calendar
{
	public class DiplomProjectConsultationModel
	{
		[JsonProperty("DiplomProjectConsultationDates")]
		public List<DiplomProjectConsultationDateModel> DiplomProjectConsultationDates { get; set; }
			= new List<DiplomProjectConsultationDateModel>();
	}

	public class DiplomProjectConsultationDateModel
	{
		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("LecturerId")]
		public int LecturerId { get; set; }

		[JsonProperty("Day")]
		public string Day { get; set; }

		[JsonProperty("StartTime")]
		public string StartTime { get; set; }

		[JsonProperty("EndTime")]
		public string EndTime { get; set; }

		[JsonProperty("Building")]
		public string Building { get; set; }

		[JsonProperty("Audience")]
		public string Audience { get; set; }
	}

	public class CourseProjectConsultationModel
	{
		[JsonProperty("Consultations")]
		public List<CourseProjectConsultationDetailsModel> Consultations { get; set; }
			= new List<CourseProjectConsultationDetailsModel>();
	}

	public class CourseProjectConsultationDetailsModel
	{
		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("Teacher")]
		public Teacher Teacher { get; set; }

		[JsonProperty("Day")]
		public string Day { get; set; }

		[JsonProperty("Subject")]
		public SubjectModel Subject { get; set; }

		[JsonProperty("StartTime")]
		public string StartTime { get; set; }

		[JsonProperty("EndTime")]
		public string EndTime { get; set; }

		[JsonProperty("Building")]
		public string Building { get; set; }

		[JsonProperty("Audience")]
		public string Audience { get; set; }
	}
}
