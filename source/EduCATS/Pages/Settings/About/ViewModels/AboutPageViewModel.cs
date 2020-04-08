using System;
using System.Threading.Tasks;
using EduCATS.Constants;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.About.ViewModels
{
	public class AboutPageViewModel : ViewModel
	{
		readonly IPlatformServices _services;

		public AboutPageViewModel(IPlatformServices services)
		{
			try {
				_services = services;
				Build = _services.Device.GetBuild();
				Version = _services.Device.GetVersion();
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		string _version;
		public string Version {
			get { return _version; }
			set { SetProperty(ref _version, value); }
		}

		string _build;
		public string Build {
			get { return _build; }
			set { SetProperty(ref _build, value); }
		}

		Command _sendLogsCommand;
		public Command SendLogsCommand {
			get {
				return _sendLogsCommand ?? (
					_sendLogsCommand = new Command(async () => await sendLogs()));
			}
		}

		Command _openSourceCommand;
		public Command OpenSourceCommand {
			get {
				return _openSourceCommand ?? (
					_openSourceCommand = new Command(async () => await openSourceSite()));
			}
		}

		Command _openSiteCommand;
		public Command OpenSiteCommand {
			get {
				return _openSiteCommand ?? (
					_openSiteCommand = new Command(async () => await openSite()));
			}
		}

		protected async Task openSourceSite()
		{
			try {
				await _services.Device.OpenUri(GlobalConsts.GitHubLink);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		protected async Task openSite()
		{
			try {
				await _services.Device.OpenUri(Servers.Current);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		protected async Task sendLogs()
		{
			try {
				var platform = _services.Device.GetRuntimePlatform();
				var logsFilePath = AppLogs.LogsFilePath;

				var result = await _services.Device.SendEmail(
					GlobalConsts.SupportAddress,
					$"[{platform}] Logs",
					$"Sent from {platform}, app version: {Version} ({Build})",
					logsFilePath);

				if (!result) {
					_services.Dialogs.ShowError(
						CrossLocalization.Translate("settings_about_send_logs_not_supported"));
				}
			} catch (Exception ex) {
				AppLogs.Log(ex);
				_services.Dialogs.ShowError(
						CrossLocalization.Translate("settings_about_send_logs_error"));
			}
		}
	}
}
