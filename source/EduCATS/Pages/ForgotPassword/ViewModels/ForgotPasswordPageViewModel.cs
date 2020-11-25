using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Json;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
using EduCATS.Networking.AppServices;
using EduCATS.Networking.Models;
using Newtonsoft.Json;
using Nyxbull.Plugins.CrossLocalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EduCATS.Pages.ForgotPassword.ViewModels
{
	public class ForgotPasswordPageViewModel : ViewModel
	{
		public readonly IPlatformServices _services;
		public ForgotPasswordPageViewModel(IPlatformServices services)
		{
			_services = services;
			IsPasswordHidden = true;
		}

		Command _resetPasswordCommand;
		/// <summary>
		/// Login command.
		/// </summary>
		public Command ResetPasswordCommand
		{
			get
			{
				return _resetPasswordCommand ??= new Command(async () => await startResetPassword());
			}
		}

		bool checkCredentials()
		{
			if (string.IsNullOrEmpty(UserName) ||
				string.IsNullOrEmpty(NewPassword) ||
				string.IsNullOrEmpty(ConfirmPassword) ||
				string.IsNullOrEmpty(AnswerToSecretQuestion) || 
				string.IsNullOrEmpty(QuestionId))
			{
				return false;
			}

			return true;
		}

		protected async Task<object> startResetPassword()
		{
			var uppercase = 0;
			bool latin_password = true;
			try
			{
				if (checkCredentials())
				{

					foreach (char symbol in NewPassword.Where(char.IsUpper))
					{
						uppercase++;
					};
					for (int i = 0; i < NewPassword.Length; i++)
					{
						if (!(((NewPassword[i] >= 'a') && (NewPassword[i] <= 'z')) || ((NewPassword[i] >= 'A') && (NewPassword[i] <= 'Z')) ||
							(NewPassword[i] > '0' && NewPassword[i] < '9')))
						{
							latin_password = false;
							break;
						}
					}

					if (!(Servers.Current == Servers.EduCatsAddress))
					{
						_services.Dialogs.ShowMessage(CrossLocalization.Translate("invaild_server"),
							CrossLocalization.Translate("change_server"));
						return Task.FromResult<object>(null);
					}

					if (!(NewPassword.Length > 6 && NewPassword.Length < 30))
					{
						_services.Dialogs.ShowError("password_length_error");
						return Task.FromResult<object>(null);
					}

					if (!(uppercase != 0 && latin_password == true))
					{
						_services.Dialogs.ShowMessage(CrossLocalization.Translate("password_not_correct"),
									CrossLocalization.Translate("latin_password"));
						return Task.FromResult<object>(null);
					}

					if (QuestionId == CrossLocalization.Translate("mother_last_name"))
					{
						SelectedQuestionId = 1;
					}

					else if (QuestionId == CrossLocalization.Translate("pets_name"))
					{
						SelectedQuestionId = 2;
					}
					else if (QuestionId == CrossLocalization.Translate("hobby"))
					{
						SelectedQuestionId = 3;
					}

					if (!(NewPassword == ConfirmPassword))
					{
						_services.Dialogs.ShowError(CrossLocalization.Translate("password_mismatch"));
						return Task.FromResult<object>(null);
					}

					var result = await VerifyPostAsync(UserName, SelectedQuestionId, AnswerToSecretQuestion);
					if (JsonConvert.DeserializeObject<string>(result.Key) == "Пользователь не найден!")
					{
						_services.Dialogs.ShowError(CrossLocalization.Translate("no_user"));
						return Task.FromResult<object>(null);
					}

					if (JsonConvert.DeserializeObject<string>(result.Key) == "Введен неверный секретный ответ")
					{
						_services.Dialogs.ShowError(CrossLocalization.Translate("invaild_answer"));
						return Task.FromResult<object>(null);
					}

					if (JsonConvert.DeserializeObject<string>(result.Key) == "OK")
					{
						setLoading(true, CrossLocalization.Translate("change_password"));
						await ResetPassword(NewPassword, UserName);
						setLoading(false);
						_services.Dialogs.ShowMessage(CrossLocalization.Translate("password_changed"),
							CrossLocalization.Translate("successful_password_change"));
						await _services.Navigation.ClosePage(false);
					}
				}
				else
				{
					_services.Dialogs.ShowError(CrossLocalization.Translate("empty_fields"));
					return Task.FromResult<object>(null);
				}
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
			return Task.FromResult<object>(null);
		}

		public static async Task<KeyValuePair<string, HttpStatusCode>> ResetPassword(string newPassword, string userName)
		{
			var newpassword = new ForgotPasswordModel
			{
				Password = newPassword,
				UserName = userName,
			};
			var body = JsonController.ConvertObjectToJson(newpassword);
			return await AppServicesController.Request(Links.ResetPassword, body);
		}

		public async Task<KeyValuePair<string, HttpStatusCode>> VerifyPostAsync(string username,
			int questionId, string answerToSecretQuestion)
		{
			object user = default;
			var body = JsonController.ConvertObjectToJson(user);
			return await AppServicesController.Request(Links.VerifySecretQuestion + "userName=" +
				username + "&questionId=" + questionId + "&answer=" + answerToSecretQuestion,body);
		}

		/// <summary>
		/// Username property.
		/// </summary>
		string _username;
		public string UserName
		{
			get { return _username; }
			set { SetProperty(ref _username, value); }
		}

		String _selectedQuestion;

		/// <summary>
		/// Secret question property.
		/// </summary>
		public String QuestionId
		{
			get { return _selectedQuestion; }
			set { SetProperty(ref _selectedQuestion, value); }
		}

		int _selectedQuestionId;

		/// <summary>
		/// Secret question property.
		/// </summary>
		public int SelectedQuestionId
		{
			get { return _selectedQuestionId; }
			set { SetProperty(ref _selectedQuestionId, value); }
		}

		string _answerToSecretQuestion;

		/// <summary>
		/// Secret question property.
		/// </summary>
		public string AnswerToSecretQuestion
		{
			get { return _answerToSecretQuestion; }
			set { SetProperty(ref _answerToSecretQuestion, value); }
		}

		string _newpassword;
		/// <summary>
		/// Confirm password property.
		/// </summary>
		public string NewPassword
		{
			get { return _newpassword; }
			set { SetProperty(ref _newpassword, value); }
		}

		string _confirmpassword;
		/// <summary>
		/// Password property.
		/// </summary>
		public string ConfirmPassword
		{
			get { return _confirmpassword; }
			set { SetProperty(ref _confirmpassword, value); }
		}

		bool _isPasswordHidden;

		/// <summary>
		/// Property for checking if password is hidden.
		/// </summary>
		public bool IsPasswordHidden
		{
			get { return _isPasswordHidden; }
			set { SetProperty(ref _isPasswordHidden, value); }
		}

		/// <summary>
		/// Sets loading status.
		/// </summary>
		/// <param name="isLoading">Is loading status.</param>
		/// <param name="message">Message to show.</param>
		void setLoading(bool isLoading, string message = null)
		{
			if (isLoading)
			{
				_services.Dialogs.ShowLoading(message);
			}
			else
			{
				_services.Dialogs.HideLoading();
			}
		}
	}
}
