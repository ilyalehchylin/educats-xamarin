using Newtonsoft.Json;

namespace EduCATS.Data.Models.Testing
{
	public class TestingItemModel
	{
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("ForSelfStudy")]
        public bool ForSelfStudy { get; set; }
    }
}
