using System;
using System.Threading.Tasks;
using EduCATS.Constants;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
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
	}
}
