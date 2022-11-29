using EduCATS.Data.Models;
using EduCATS.Data.Models.Registration;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Json;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
using EduCATS.Networking.AppServices;
using Newtonsoft.Json;
using Nyxbull.Plugins.CrossLocalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EduCATS.Pages.Registration.ViewModels
{
	public class RegistrationPageViewModel : ViewModel
	{
		public readonly IPlatformServices _services;
		public RegistrationPageViewModel(IPlatformServices services)
		{
			_services = services;
			IsConfirmPasswordHidden = IsPasswordHidden = true;
		}

		Command _registerCommand;
		/// <summary>
		/// Login command.
		/// </summary>
		public Command RegisterCommand
		{
			get
			{
				return _registerCommand ??= new Command(async () => await startRegister());
			}
		}

		Command _hidePasswordCommand;
		/// <summary>
		/// Hide password command.
		/// </summary>
		public Command HidePasswordCommand
		{
			get
			{
				return _hidePasswordCommand ?? (_hidePasswordCommand = new Command(hidePassword));
			}
		}

		Command _hideConfirmPasswordCommand;
		/// <summary>
		/// Hide password command.
		/// </summary>
		public Command HideConfirmPasswordCommand
		{
			get
			{
				return _hideConfirmPasswordCommand ?? (_hideConfirmPasswordCommand = new Command(hideConfirmPassword));
			}
		}

		public bool checkCredentials()
		{
			if (string.IsNullOrEmpty(UserName) ||
				string.IsNullOrEmpty(Password) ||
				string.IsNullOrEmpty(ConfirmPassword) ||
				string.IsNullOrEmpty(Name) ||
				string.IsNullOrEmpty(Surname) ||
				ReferenceEquals(Group, null) ||
				ReferenceEquals(SelectedQuestionId, 0) ||
				string.IsNullOrEmpty(AnswerToSecretQuestion))
			{
				return false;
			}

			return true;
		}

		public int UpperCaseLettersInPassword()
		{
			int uppercase = 0;
			foreach (char symbol in Password.Where(char.IsUpper))
			{
				uppercase++;
			};
			return uppercase;
		}

		public bool LatinPassword()
		{
			bool latin_password = true;
			try
			{
				
				for (int i = 0; i < Password.Length; i++)
				{
					if (!(((Password[i] >= 'a') && (Password[i] <= 'z')) || ((Password[i] >= 'A') && (Password[i] <= 'Z')) || (Password[i] == '_') ||
						((Password[i] >= '0') && Password[i] <= '9')))
					{
						latin_password = false;
						break;
					}
				}
			}
			catch
			{
				latin_password = false;
			}
			return latin_password;
		}

		public bool CheckNameOfUser(string nameOfUser)
		{
			bool name = true;
			try
			{
				for (int i = 0; i < nameOfUser.Length; i++)
				{
					if (!(
						((nameOfUser[i] >= 'a') && (nameOfUser[i] <= 'z')) || ((nameOfUser[i] >= 'A') && (nameOfUser[i] <= 'Z')) ||
						((nameOfUser[i] >= 'а') && (nameOfUser[i] <= 'я')) || ((nameOfUser[i] >= 'А') && (nameOfUser[i] <= 'Я')) ||
						((nameOfUser[i] == '_') || (nameOfUser[i] == ' ') || (nameOfUser[i] == '-')) ||
						(nameOfUser[i] >= '0' && nameOfUser[i] <= '9')))
					{
						name = false;
						break;
					}
				}
			}
			catch
			{
				name = false;
			}
			return name;
		}
		 
		public bool LatinUserName()
		{
			bool latin_username = true;
			try
			{
				for (int i = 0; i < UserName.Length; i++)
				{
					if (!(((UserName[i] >= 'a') && (UserName[i] <= 'z')) || ((UserName[i] >= 'A') && (UserName[i] <= 'Z'))
						|| ((UserName[i] >= '0') && (UserName[i] <= '9'))
						|| (UserName[i] == '_')
						|| (UserName[i] == '.')
						|| (UserName[i] == '-')
						|| (UserName[i] == '@')))
					{
						latin_username = false;
						break;
					}
				}
			}
			catch
			{
				latin_username = false;
			}
			return latin_username;
		}

		public async Task<Task<object>> startRegister()
		{
			try
			{
				if (checkCredentials())
				{
					bool correctPatronymic = true;
					bool correctName = CheckNameOfUser(Name);
					bool correctSurname = CheckNameOfUser(Surname);
					bool secretAnswer = CheckNameOfUser(AnswerToSecretQuestion);
					if (Patronymic != null)
					{
						correctPatronymic = CheckNameOfUser(Patronymic);
					}
					SelectedQuestionId = 0;
					int uppercase = UpperCaseLettersInPassword();
					bool latinPassword = LatinPassword();
					bool latinUsername = LatinUserName();

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

					var userExists = await VerifyUserNameAsync(UserName);
					if (JsonConvert.DeserializeObject<string>(userExists.Key) == "true")
					{
						_services.Dialogs.ShowError(CrossLocalization.Translate("user_exists"));
						return Task.FromResult<object>(null);
					}

					if (!(UserName.Length >= 3 && UserName.Length <= 30))
					{
						_services.Dialogs.ShowMessage(CrossLocalization.Translate("username_error"),
										CrossLocalization.Translate("less_than_three_characters"));
						return Task.FromResult<object>(null);
					}

					if (!(latinUsername == true))
					{
						_services.Dialogs.ShowMessage(CrossLocalization.Translate("username_error"),
											CrossLocalization.Translate("latin_letters"));
						return Task.FromResult<object>(null);
					}

					if (!(Password.Length >= 6 && Password.Length <= 30))
					{
						_services.Dialogs.ShowError(CrossLocalization.Translate("password_length_error"));
						return Task.FromResult<object>(null);
					}

					if (!(uppercase != 0 && latinPassword == true))
					{
						_services.Dialogs.ShowMessage(CrossLocalization.Translate("password_not_correct"),
									CrossLocalization.Translate("latin_password"));
						return Task.FromResult<object>(null);
					}

					if (!(Password == ConfirmPassword))
					{
						_services.Dialogs.ShowError(CrossLocalization.Translate("password_mismatch"));
						return Task.FromResult<object>(null);
					}

					if (Name[0] == ' ' || Name[Name.Length - 1] == ' ' || Surname[0] == ' ' || Surname[Surname.Length - 1] == ' ')
					{
						_services.Dialogs.ShowMessage(CrossLocalization.Translate("not_corrected_data"),
							CrossLocalization.Translate("delete_spaces"));
						return Task.FromResult<object>(null);
					}

					if (Patronymic != null && (Patronymic[0] == ' ' || Patronymic[Patronymic.Length - 1] == ' '))
					{
						_services.Dialogs.ShowMessage(CrossLocalization.Translate("not_corrected_data"),
							CrossLocalization.Translate("delete_spaces"));
						return Task.FromResult<object>(null);
					}

					if (Name.Length >= 30 || Surname.Length >= 30 || (Patronymic!= null && Patronymic.Length >= 30))
					{
						_services.Dialogs.ShowMessage(CrossLocalization.Translate("not_carrected_lenth"),
							CrossLocalization.Translate("lenth_name_surname_patronymic"));
						return Task.FromResult<object>(null);
					}

					if (!(correctName && correctSurname && correctPatronymic))
					{
						_services.Dialogs.ShowMessage(CrossLocalization.Translate("not_corrected_data"),
							CrossLocalization.Translate("name_surmane_patronymic_format"));
						return Task.FromResult<object>(null);
					}

					setLoading(true, CrossLocalization.Translate("chek_In"));
					await RegistrationPostAsync(UserName, Name, Surname, Patronymic, Password, 
						ConfirmPassword, Group.Id, SelectedQuestionId, AnswerToSecretQuestion);
					setLoading(false);
					_services.Dialogs.ShowMessage(CrossLocalization.Translate("registration"),
						CrossLocalization.Translate("succses_reg"));
					await _services.Navigation.ClosePage(false);
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

		public async Task<KeyValuePair<string, HttpStatusCode>> VerifyUserNameAsync(string username)
		{
			return await AppServicesController.Request(Links.VerifyUserExists + "userName=" + username);
		}

		public async Task<KeyValuePair<string, HttpStatusCode>> RegistrationPostAsync(string username, string name, string surname, string patronymic, string password, string confirmPassword,
						int group, int questionId, string answerToSecretQuestion)
		{
			var registerUser = new RegistrationModel
			{
				Name = name,
				Surname = surname,
				Patronymic = patronymic,
				UserName = username,
				Password = password,
				ConfirmPassword = confirmPassword,
				Group = group,
				QuestionId = questionId,
				AnswerToSecretQuestion = answerToSecretQuestion,
			};

			var body = JsonController.ConvertObjectToJson(registerUser);
			return await AppServicesController.Request(Links.Registration, body);
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
		/// <summary>
		/// Name property.
		/// </summary>
		string _name;
		public string Name
		{
			get { return _name; }
			set { SetProperty(ref _name, value); }
		}
		/// <summary>
		/// SurName property.
		/// </summary>
		string _sname;
		public string Surname
		{
			get { return _sname; }
			set { SetProperty(ref _sname, value); }
		}
		/// <summary>
		/// Patronymic property.
		/// </summary>
		string _patronymic;
		public string Patronymic
		{
			get { return _patronymic; }
			set { SetProperty(ref _patronymic, value); }
		}

		string _confirmpassword;
		/// <summary>
		/// Confirm password property.
		/// </summary>
		public string ConfirmPassword
		{
			get { return _confirmpassword; }
			set { SetProperty(ref _confirmpassword, value); }
		}
		string _password;
		/// <summary>
		/// Password property.
		/// </summary>
		public string Password
		{
			get { return _password; }
			set { SetProperty(ref _password, value); }
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

		bool _isConfirmPasswordHidden;

		/// <summary>
		/// Property for checking if confirm password is hidden.
		/// </summary>
		public bool IsConfirmPasswordHidden
		{
			get { return _isConfirmPasswordHidden; }
			set { SetProperty(ref _isConfirmPasswordHidden, value); }
		}

		GroupItemModel _groupNumber;

		/// <summary>
		/// Group Number property.
		/// </summary>
		public GroupItemModel Group
		{
			get { return _groupNumber; }
			set { SetProperty(ref _groupNumber, value); }
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

		protected void hidePassword()
		{
			IsPasswordHidden = !IsPasswordHidden;
		}

		/// <summary>
		/// Hides or shows a confirm password.
		/// </summary>
		protected void hideConfirmPassword()
		{
			IsConfirmPasswordHidden = !IsConfirmPasswordHidden;
		}
	}
}
