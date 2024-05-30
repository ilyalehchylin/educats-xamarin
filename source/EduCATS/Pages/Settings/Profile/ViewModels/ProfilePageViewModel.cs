using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using EduCATS.Constants;
using EduCATS.Data;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
using EduCATS.Pages.Statistics.Students.Models;
using Newtonsoft.Json.Linq;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;


namespace EduCATS.Pages.Settings.Profile.ViewModels
{
	public class ProfilePageViewModel : ViewModel
	{
		readonly IPlatformServices _services;

		public ProfilePageViewModel(IPlatformServices services)
		{
			try
			{
				_services = services;
				Build = _services.Device.GetBuild();
				Version = _services.Device.GetVersion();
				setInitData();
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex, nameof(ProfilePageViewModel));
			}
		}
		string _version;
		public string Version
		{
			get { return _version; }
			set { SetProperty(ref _version, value); }
		}

		string _build;
		public string Build
		{
			get { return _build; }
			set { SetProperty(ref _build, value); }
		}

		bool _isLoggedIn;
		public bool IsLoggedIn
		{
			get { return _isLoggedIn; }
			set { SetProperty(ref _isLoggedIn, value); }
		}

		string _username;
		public string Username
		{
			get { return _username; }
			set { SetProperty(ref _username, value); }
		}

		string _group;
		public string Group
		{
			get { return _group; }
			set { SetProperty(ref _group, value); }
		}

		string _groupLabel;
		public string GroupLabel
		{
			get { return _groupLabel; }
			set { SetProperty(ref _groupLabel, value); }
		}

		string _role;
		public string Role
		{
			get { return _role; }
			set { SetProperty(ref _role, value); }
		}

		string _name;
		public string Name
		{
			get { return _name; }
			set { SetProperty(ref _name, value); }
		}

		string _surname;
		public string SecondName
		{
			get { return _surname; }
			set { SetProperty(ref _surname, value); }
		}

		string _patronymic;
		public string Patronymic
		{
			get { return _patronymic; }
			set { SetProperty(ref _patronymic, value); }
		}

		string _avatar;
		public string Avatar
		{
			get { return _avatar; }
			set { SetProperty(ref _avatar, value); }
		}

		string _email;
		public string Email
		{
			get { return _email; }
			set { SetProperty(ref _email, value); }
		}

		string _phone;
		public string Phone
		{
			get { return _phone; }
			set { SetProperty(ref _phone, value); }
		}

		string _accountinfo;
		public string AccountInfo
		{
			get { return _accountinfo; }
			set { SetProperty(ref _accountinfo, value); }
		}

		string _about;
		public string About
		{
			get { return _about; }
			set { SetProperty(ref _about, value); }
		}

		async void setInitData()
		{
			Username = _services.Preferences.UserLogin;
			Avatar = _services.Preferences.Avatar;
			var isProfessor = string.IsNullOrEmpty(_services.Preferences.GroupName);
			Group = isProfessor ? null : _services.Preferences.GroupName;
			GroupLabel = isProfessor ? null : CrossLocalization.Translate("choose_group");
			Role = CrossLocalization.Translate(isProfessor ? "role_professor" : "role_student");
			var profile = await DataAccess.GetProfileInfo(_username);
			if (profile.Name != null)
			{
				var fio = profile.Name.Split(' ');
				SecondName = fio[0];
				Name = fio[1];
				Patronymic = fio[2];
			}
			else
			{
				_services.Dialogs.ShowError(CrossLocalization.Translate("today_account_error"));
			}
			Email = profile.Email;
			Phone = profile.Phone;
			AccountInfo = profile.SkypeContact;
			About = profile.About;
		}
	}
}
