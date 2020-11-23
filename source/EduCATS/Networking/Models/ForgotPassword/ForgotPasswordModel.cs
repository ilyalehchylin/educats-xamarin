using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Networking.Models
{
	public class ForgotPasswordModel
	{
		/// <summary>
		/// Login.
		/// </summary>
		public string UserName { get; set; }
		/// <summary>
		/// Password.
		/// </summary>
		public string Password { get; set; }
	}
}
