using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using EduCATS.Helpers.Files;

namespace EduCATS.Demo
{
	/// <summary>
	/// Helper class for handling demo account state.
	/// </summary>
	public class AppDemo
	{
		/// <summary>
		/// Shared instance of <c>AppDemo</c>.
		/// </summary>
		public static AppDemo Instance;

		const string demoUsername = "demo";
		const string demoPassword = "demo";

		FileManager fileManager = new FileManager();

		static AppDemo()
		{
			Instance = new AppDemo();
		}

		bool _isDemoAccount;

		/// <summary>
		/// Property for checking whether is account demo or not.
		/// </summary>
		public bool IsDemoAccount {
			get {
				return _isDemoAccount;
			}
			set {
				_isDemoAccount = value;
			}
		}

		/// <summary>
		/// Check if account is demo or not.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <returns>Demo account or not.</returns>
		public bool CheckDemoAccount(string username, string password)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) {
				return false;
			}

			var isDemoAccount = username.ToLower().Equals(demoUsername.ToLower()) && password.Equals(demoPassword);
			IsDemoAccount = isDemoAccount;
			return isDemoAccount;
		}

		/// <summary>
		/// Get demo response string and status code.
		/// </summary>
		/// <param name="type">Demo resource type to retrieve.</param>
		/// <returns>Demo response string and status code.</returns>
		public KeyValuePair<string, HttpStatusCode> GetDemoResponse(AppDemoType type)
		{
			var resource = $"demo{type.ToString()}";
			var extension = "json";

			switch (type) {
				case AppDemoType.TestAnswerAndGetNext:
					extension = "txt";
					break;
				default:
					extension = "json";
					break;
			}

			var contents = fileManager.ReadDemoEmbeddedResource(resource, extension);

			if (type == AppDemoType.ProfileInfoCalendar) {
				var today = DateTime.Today;
				var format = "yyyy-MM-dd";
				var todayString = today.ToString(format);
				var tomorrowString = today.AddDays(1).ToString(format);
				var builder = new StringBuilder(contents);
				builder.Replace("$dateToday", todayString);
				builder.Replace("$dateTomorrow", tomorrowString);
				contents = builder.ToString();
			}

			return new KeyValuePair<string, HttpStatusCode>(contents, HttpStatusCode.OK);
		}

		/// <summary>
		/// Generate invalid response with <c>BadRequest</c> status code.
		/// </summary>
		/// <returns>Invalid response with empty string and <c>BadRequest</c> status code.</returns>
		public KeyValuePair<string, HttpStatusCode> GetInvalidResponse()
		{
			return new KeyValuePair<string, HttpStatusCode>("", HttpStatusCode.BadRequest);
		}
	}
}
