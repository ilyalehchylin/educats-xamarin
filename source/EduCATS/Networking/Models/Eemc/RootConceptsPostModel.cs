using EduCATS.Constants;
using Newtonsoft.Json;

namespace EduCATS.Networking.Models.Eemc
{
	public class RootConceptsPostModel
	{
		public RootConceptsPostModel(string userId, string subjectId)
		{
			UserId = userId;
			SubjectId = subjectId;
			IdentityKey = GlobalConsts.RootConceptsIdentityKey;
		}

		[JsonProperty("subjectId")]
		public string SubjectId { get; set; }

		[JsonProperty("userId")]
		public string UserId { get; set; }

		[JsonProperty("identityKey")]
		public string IdentityKey { get; }
	}
}
