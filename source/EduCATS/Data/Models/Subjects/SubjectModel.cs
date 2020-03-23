using Newtonsoft.Json;

namespace EduCATS.Data.Models.Subjects
{
	/// <summary>
	/// Subject ID.
	/// </summary>
	public class SubjectModel
	{
		/// <summary>
		/// Subject name.
		/// </summary>
		[JsonProperty("Name")]
		public string Name { get; set; }

		/// <summary>
		/// Subject ID.
		/// </summary>
		[JsonProperty("Id")]
		public int Id { get; set; }

		/// <summary>
		/// Subject short name.
		/// </summary>
		[JsonProperty("ShortName")]
		public string ShortName { get; set; }

		/// <summary>
		/// Subject color.
		/// </summary>
		[JsonProperty("Color")]
		public string Color { get; set; }

		/// <summary>
		/// Subject completing percent.
		/// </summary>
		[JsonProperty("Completing")]
		public int Completing { get; set; }
	}
}
