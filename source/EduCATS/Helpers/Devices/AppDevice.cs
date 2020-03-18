using System;
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
	}
}
