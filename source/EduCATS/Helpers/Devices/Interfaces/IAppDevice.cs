using System;
using System.Threading.Tasks;

namespace EduCATS.Helpers.Devices.Interfaces
{
	public interface IAppDevice
	{
		void MainThread(Action action);
		void SetTimer(TimeSpan interval, Func<bool> callback);
		void OpenUri(string uri);
		string GetAppDataDirectory();
		Task ShareFile(string title, string filePath);
	}
}
