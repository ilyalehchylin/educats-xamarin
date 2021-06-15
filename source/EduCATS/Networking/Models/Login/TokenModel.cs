using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models.Login
{
	public class TokenModel
	{
		[JsonProperty("token")]
		public string Token { get; set; }
	}
}
