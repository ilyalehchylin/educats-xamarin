using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Data.Models.Registration
{
	public class RegistrationModel
	{
		/// <summary>
		/// Login.
		/// </summary>
		public string UserName { get; set; }
		/// <summary>
		/// Password.
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		/// Confirm password.
		/// </summary>
		public string ConfirmPassword { get; set; }
		/// <summary>
		/// Name of Student.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Surname of Student.
		/// </summary>
		public string Surname { get; set; }
		/// <summary>
		/// Patronymic of Student.
		/// </summary>
		public string Patronymic { get; set; }
		/// <summary>
		/// Student group.
		/// </summary>
		public int Group { get; set; }
		/// <summary>
		/// Secret question.
		/// </summary>
		public int QuestionId { get; set; }
		/// <summary>
		/// Answer to secret question.
		/// </summary>
		public string AnswerToSecretQuestion { get; set; }
	}
}
