using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Data.Models.User
{
	public class DeleteAccountModel
	{
		[JsonProperty("IsTest")]
		public bool IsTest { get; set; }
		
		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("Text")]
		public string Text { get; set; }


	}
}
