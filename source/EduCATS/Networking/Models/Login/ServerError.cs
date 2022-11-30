using Newtonsoft.Json;

namespace EduCATS.Networking.Models.Login
{
	public class ServerError
	{
		[JsonProperty("error")]
		public int Error { get; set; }

		[JsonProperty("description")]
		public string Descriprion { get; set; }

	}
}
