using EduCATS.Helpers.Forms.Devices;
using EduCATS.Helpers.Forms.Dialogs;
using EduCATS.Helpers.Forms.Pages;
using EduCATS.Helpers.Forms.Settings;

namespace EduCATS.Helpers.Forms
{
	public interface IPlatformServices
	{
		IDevice Device { get; set; }
		IDialogs Dialogs { get; set; }
		IPages Navigation { get; set; }
		IPreferences Preferences { get; set; }
	}
}
