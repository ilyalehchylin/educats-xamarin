using Newtonsoft.Json;

namespace EduCATS.Networking.Models.Login
{
	/// <summary>
	/// Token <c>POST</c> model.
	/// </summary>
	public class TokenCredentials
	{
		/// <summary>
		/// User login.
		/// </summary>
		[JsonProperty("userName")]
		public string Username { get; set; }

		/// <summary>
		/// Password.
		/// </summary>
		[JsonProperty("password")]
		public string Password { get; set; }
	}
}

