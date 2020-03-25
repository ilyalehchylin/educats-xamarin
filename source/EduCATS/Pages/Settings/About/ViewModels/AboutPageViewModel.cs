using System.Threading.Tasks;
using EduCATS.Constants;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Networking;
using Xamarin.Forms;

namespace EduCATS.Pages.Settings.About.ViewModels
{
	public class AboutPageViewModel : ViewModel
	{
		readonly IDevice _device;

		public AboutPageViewModel(IDevice device)
		{
			_device = device;
			Version = device.GetVersion();
			Build = device.GetBuild();
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

		protected async Task openSourceSite() => await _device.OpenUri(GlobalConsts.GitHubLink);

		protected async Task openSite() => await _device.OpenUri(Servers.Current);
	}
}
