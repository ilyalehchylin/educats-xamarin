using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
		/// <summary>
		/// Username property.
		/// </summary>
		string _username;
		public string Username
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
		public string SName
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
		public GroupItemModel GroupNumber
		{
			get { return _groupNumber; }
			set { SetProperty(ref _groupNumber, value); }
		}

		string _selectedQuestion;

		/// <summary>
		/// Secret question property.
		/// </summary>
		public string SelectedQuestion
		{
			get { return _selectedQuestion; }
			set { SetProperty(ref _selectedQuestion, value); }
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
	}
}
