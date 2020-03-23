using Newtonsoft.Json;

namespace EduCATS.Data.Models.Testing.Base
{
	/// <summary>
	/// Test model.
	/// </summary>
	public class TestModel
	{
		/// <summary>
		/// Test ID.
		/// </summary>
        [JsonProperty("Id")]
        public int Id { get; set; }

		/// <summary>
		/// Test title.
		/// </summary>
        [JsonProperty("Title")]
        public string Title { get; set; }

		/// <summary>
		/// Test description.
		/// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

		/// <summary>
		/// Is test for self-study.
		/// </summary>
        [JsonProperty("ForSelfStudy")]
        public bool ForSelfStudy { get; set; }
    }
}
