using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// Lectures visiting student model.
	/// </summary>
	public class LecturesStudentModel
	{
		/// <summary>
		/// Visiting details list.
		/// </summary>
		[JsonProperty("Marks")]
		public IList<LecturesDetailsModel> VisitingList { get; set; }

		/// <summary>
		/// Student ID.
		/// </summary>
		[JsonProperty("StudentId")]
		public int StudentId { get; set; }

		/// <summary>
		/// Student username.
		/// </summary>
		[JsonProperty("Login")]
		public string Login { get; set; }

		/// <summary>
		/// Student full name.
		/// </summary>
		[JsonProperty("StudentName")]
		public string StudentName { get; set; }
	}
}
