using EduCATS.Helpers.Forms.Devices;
using EduCATS.Helpers.Forms.Dialogs;
using EduCATS.Helpers.Forms.Pages;
using EduCATS.Helpers.Forms.Settings;

namespace EduCATS.Helpers.Forms
{
	public class PlatformServices : IPlatformServices
	{
		public IDevice Device { get; set; }
		public IDialogs Dialogs { get; set; }
		public IPages Navigation { get; set; }
		public IPreferences Preferences { get; set; }

		public PlatformServices() {
			Device = new AppDevice();
			Dialogs = new AppDialogs();
			Navigation = new AppPages();
			Preferences = new AppPrefs();
		}
	}
}
