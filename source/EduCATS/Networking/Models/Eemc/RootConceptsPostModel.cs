using Newtonsoft.Json;

namespace EduCATS.Networking.Models.Eemc
{
	/// <summary>
	/// EEMC root concepts <c>POST</c> model.
	/// </summary>
	public class RootConceptsPostModel
	{
		/// <summary>
		/// Identity key.
		/// </summary>
		const string _identityKey = "7e13f363-2f00-497e-828e-49e82d8b4223";

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="userId">User ID.</param>
		/// <param name="subjectId">Subject ID.</param>
		public RootConceptsPostModel(string userId, string subjectId)
		{
			UserId = userId;
			SubjectId = subjectId;
			IdentityKey = _identityKey;
		}

		/// <summary>
		/// Subject ID.
		/// </summary>
		[JsonProperty("subjectId")]
		public string SubjectId { get; set; }

		/// <summary>
		/// User ID.
		/// </summary>
		[JsonProperty("userId")]
		public string UserId { get; set; }

		/// <summary>
		/// Identity key.
		/// </summary>
		[JsonProperty("identityKey")]
		public string IdentityKey { get; }
	}
}
