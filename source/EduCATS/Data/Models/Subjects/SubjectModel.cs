using Newtonsoft.Json;
using System;

namespace EduCATS.Data.Models
{
	/// <summary>
	/// Subject ID.
	/// </summary>
	public class SubjectModel: IEquatable<SubjectModel>
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

		public bool Equals(SubjectModel other)
		{
			if (Object.ReferenceEquals(other, null)) return false;

			if (Object.ReferenceEquals(this, other)) return true;

			return Id.Equals(other.Id);
		}

		// If Equals() returns true for a pair of objects
		// then GetHashCode() must return the same value for these objects.

		public override int GetHashCode()
		{
			int hashId = Id == null ? 0 : Id.GetHashCode();

			return hashId;
		}
	}
}
