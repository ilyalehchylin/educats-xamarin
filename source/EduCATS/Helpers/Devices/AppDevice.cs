using System;
using System.Threading.Tasks;
using EduCATS.Helpers.Devices.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EduCATS.Helpers.Devices
{
	public class AppDevice : IAppDevice
	{
		public void MainThread(Action action)
		{
			Device.BeginInvokeOnMainThread(action);
		}

		public void SetTimer(TimeSpan interval, Func<bool> callback)
		{
			Device.StartTimer(interval, callback);
		}

		public void OpenUri(string uri)
		{
			Launcher.OpenAsync(uri);
		}

		public string GetAppDataDirectory()
		{
			return FileSystem.AppDataDirectory;
		}

		public async Task ShareFile(string title, string filePath)
		{
			await Share.RequestAsync(new ShareFileRequest {
				Title = title,
				File = new ShareFile(filePath)
			});
		}
	}
}
