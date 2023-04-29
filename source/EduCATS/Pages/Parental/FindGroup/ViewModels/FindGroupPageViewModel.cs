using System;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Pages;
using EduCATS.Helpers.Logs;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Parental.FindGroup.ViewModels
{  
	/// <summary>
	/// Login page ViewModel.
	/// </summary>
	public class FindGroupPageViewModel : ViewModel
	{
		/// <summary>
		/// Platform services.
		/// </summary>
		readonly IPlatformServices _service;

		/// <summary>
		/// FindGroup page ViewModel constructor.
		/// </summary>
		public FindGroupPageViewModel(IPlatformServices services)
		{
			_service = services;
			IsLoadingCompleted = true;
		}

		bool _isLoadingCompleted;

		/// <summary>
		/// Property for checking loading status completion.
		/// </summary>
		public bool IsLoadingCompleted
		{
			get { return _isLoadingCompleted; }
			set { SetProperty(ref _isLoadingCompleted, value); }
		}

		string _groupNumber;

		/// <summary>
		/// Group Number property.
		/// </summary>
		public string GroupNumber
		{
			get { return _groupNumber; }
			set { SetProperty(ref _groupNumber, value); }
		}

		string _studentName;

		/// <summary>
		/// FIO property.
		/// </summary>
		public string StudentName
		{
			get { return _studentName; }
			set { SetProperty(ref _studentName, value); }
		}

		Command _parentalCommand;
		/// <summary>
		/// Open Main Statistic page
		/// </summary>
		public Command ParentalCommand
		{
			get
			{
				return _parentalCommand ?? (_parentalCommand = new Command(
					async () => await openParental()));
			}
		}
		protected async Task openParental()
		{
			if (string.IsNullOrEmpty(GroupNumber))
			{
				_service.Dialogs.ShowError(CrossLocalization.Translate("parental_error_empty_group_number"));
				return;
			}
			try
			{
				_service.Device.MainThread(() => _service.Dialogs.ShowLoading());

				var group = await DataAccess.GetGroupInfo(GroupNumber);

				if (group.Code.Equals("200"))
				{
					_service.Preferences.GroupId = group.GroupId;
					_service.Preferences.GroupName = GroupNumber;
					_service.Preferences.ChosenGroupId = group.GroupId;
					await _service.Navigation.OpenParentalStats(group, CrossLocalization.Translate("main_statistics"));
				}
				else
				{
					_service.Dialogs.ShowError(CrossLocalization.Translate("parental_group_not_found"));
				}

			}
			catch
			{
				_service.Dialogs.ShowError(CrossLocalization.Translate("parental_connection_error"));
			}
			finally
			{
				_service.Device.MainThread(() => _service.Dialogs.HideLoading());
			}
		}

		Command _settingsCommand;

		/// <summary>
		/// Opent settigns page command.
		/// </summary>
		public Command SettingsCommand
		{
			get
			{
				return _settingsCommand ?? (_settingsCommand = new Command(
					async () => await openSettings()));
			}
		}

		protected async Task openSettings()
		{
			try
			{
				await _service.Navigation.OpenSettings(
					CrossLocalization.Translate("main_settings"));
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}
	}
}
