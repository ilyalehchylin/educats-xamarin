using EduCATS.Data.Models;
using EduCATS.Data.Models.Registration;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Json;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
using EduCATS.Networking.AppServices;
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
			IsPasswordHidden = true;
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

		bool checkCredentials()
		{
			if (string.IsNullOrEmpty(UserName) ||
				string.IsNullOrEmpty(Password) ||
				string.IsNullOrEmpty(ConfirmPassword) ||
				string.IsNullOrEmpty(Name) ||
				string.IsNullOrEmpty(Surname) ||
				string.IsNullOrEmpty(AnswerToSecretQuestion) ||
				ReferenceEquals(Group.Id, null))
			{
				return false;
			}

			return true;
		}

		protected async Task startRegister()
		{
			var uppercase = 0;
			bool latin_password = true;
			bool latin_username = true;
			try
			{
				if (checkCredentials())
				{
					foreach (char symbol in Password.Where(char.IsUpper))
					{
						uppercase++;
					};
					for (int i = 0; i < Password.Length; i++)
					{
						if (!(((Password[i] >= 'a') && (Password[i] <= 'z')) || ((Password[i] >= 'A') && (Password[i] <= 'Z')) ||
							(Password[i] > '0' && Password[i] < '9')))
						{
							latin_password = false;
							break;
						}
					}
					for (int i = 0; i < UserName.Length; i++)
					{
						if (!(((UserName[i] >= 'a') && (UserName[i] <= 'z')) || ((UserName[i] >= 'A') && (UserName[i] <= 'Z'))
							|| (Password[i] > '0' && Password[i] < '9')))
						{
							latin_username = false;
							break;
						}
					}
					switch (QuestionId)
					{
						case ("Девичья фамилия матери?"):
							SelectedQuestionId = 1;
							break;
						case ("Кличка любимого животного?"):
							SelectedQuestionId = 2;
							break;
						case ("Ваше хобби?"):
							SelectedQuestionId = 3;
							break;
					}
					if (Servers.Current == "https://host27072020.of.by")
					{
						if (Password.Length > 6 && Password.Length < 30)
						{
							if (uppercase != 0 && latin_password == true)
							{
								if (UserName.Length > 3 && UserName.Length < 30)
								{
									if (latin_username == true)
									{
										if (Password == ConfirmPassword)
										{
											setLoading(true, "Registration goes"); //CrossLocalization.Translate("login_loading"));
											await RegistrationPostAsync(UserName, Name, Surname, Patronymic, Password, ConfirmPassword, Group.Id, SelectedQuestionId, AnswerToSecretQuestion);
											setLoading(false);
											await _services.Navigation.ClosePage(false);
										}
										else
										{
											_services.Dialogs.ShowMessage("Password mismatch", "Passwords do not match");
										}
									}
									else
									{
										_services.Dialogs.ShowMessage("Username error", "Use only Latin letters, numbers, period or underscore.");
									}
								}
								else
								{
									_services.Dialogs.ShowMessage("Username error", "You have entered less than 3 characters.");
								}
							}
							else
							{
								_services.Dialogs.ShowMessage("Password not correct", "Use only Latin letters (at least one uppercase and lowercase), an underscore and at least one number.");
							}
						}
						else
						{
							_services.Dialogs.ShowError("Password length must be more than 3 symbols and less than 30");
						}
					}
					else
					{
						_services.Dialogs.ShowMessage("Invaild server", "Please change current server on 'educats.by'");
					}
				}
				else
				{
					_services.Dialogs.ShowError("Some fields empty...");//CrossLocalization.Translate());
				}
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		public static async Task<KeyValuePair<string, HttpStatusCode>> RegistrationPostAsync(string username, string name, string surname, string patronymic, string password, string confirmPassword,
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
				QuestionId = questionId + 1,
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
	}
}
