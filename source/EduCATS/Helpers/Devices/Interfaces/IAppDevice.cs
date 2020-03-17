using System;

namespace EduCATS.Helpers.Devices.Interfaces
{
	public interface IAppDevice
	{
		void MainThread(Action action);
		void SetTimer(TimeSpan interval, Func<bool> callback);
	}
}
