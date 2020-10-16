using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// Student statistics model.
	/// </summary>
	public class StatsStudentModel
	{
		/// <summary>
		/// Student ID.
		/// </summary>
		[JsonProperty("StudentId")]
		public int StudentId { get; set; }

		/// <summary>
		/// Student name.
		/// </summary>
		[JsonProperty("FullName")]
		public string Name { get; set; }

		/// <summary>
		/// Student username.
		/// </summary>
		[JsonProperty("Login")]
		public string Login { get; set; }

		/// <summary>
		/// Student laboratory works visiting list.
		/// </summary>
		[JsonProperty("LabVisitingMark")]
		public IList<StatsVisitingModel> VisitingList { get; set; }

		/// <summary>
		/// Student laboratory works rating marks list.
		/// </summary>
		[JsonProperty("Marks")]
		public IList<StatsMarkModel> MarkList { get; set;  }

		/// <summary>
		/// Average labs rating mark.
		/// </summary>
		[JsonProperty("LabsMarkTotal")]
		public string AverageLabsMark { get; set; }

		/// <summary>
		/// Average tests rating mark.
		/// </summary>
		[JsonProperty("TestMark")]
		public string AverageTestMark { get; set; }
	}
}
