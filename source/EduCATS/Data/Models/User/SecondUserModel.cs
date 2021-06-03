using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Data.Models.User
{
	public class SecondUserModel
	{
		[JsonProperty("userName")]
		public string Username { get; set; }

		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("role")]
		public string role { get; set; }
	}
}
