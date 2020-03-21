using System;
using System.Threading.Tasks;

namespace EduCATS.Helpers.Devices.Interfaces
{
	public interface IDevice
	{
		void OpenUri(string uri);
		string GetAppDataDirectory();
		void MainThread(Action action);
		Task ShareFile(string title, string filePath);
		void SetTimer(TimeSpan interval, Func<bool> callback);
	}
}
