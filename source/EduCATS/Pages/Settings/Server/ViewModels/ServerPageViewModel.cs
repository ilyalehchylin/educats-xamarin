using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.User;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Helpers.Settings;
using EduCATS.Networking;
using EduCATS.Pages.Settings.Server.Models;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Pages.Settings.Server.ViewModels
{
	public class ServerPageViewModel : ViewModel
	{
		readonly IDialogs _dialogs;
		readonly IDevice _device;
		readonly IPages _pages;

		public ServerPageViewModel(IDialogs dialogs, IDevice device, IPages pages)
		{
			_pages = pages;
			_device = device;
			_dialogs = dialogs;

			setServers();
		}

		List<ServerPageModel> _serverList;
		public List<ServerPageModel> ServerList {
			get { return _serverList; }
			set { SetProperty(ref _serverList, value); }
		}

		object _selectedItem;
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);
				_device.MainThread(async () => await selectServer(_selectedItem));
			}
		}

		void setServers()
		{
			ServerList = new List<ServerPageModel> {
				getServerDetails(Servers.EduCatsBntuAddress, "server_stable"),
				getServerDetails(Servers.EduCatsAddress, "server_test"),
				getServerDetails(Servers.LocalAddress, "server_local")
			};
		}

		async Task selectServer(object selectedObject)
		{
			if (selectedObject == null || !(selectedObject is ServerPageModel)) {
				return;
			}

			var server = (ServerPageModel)selectedObject;

			if (!AppPrefs.IsLoggedIn) {
				changeServer(server);
				return;
			}

			var result = await _dialogs.ShowConfirmationMessage(
				CrossLocalization.Translate("base_warning"),
				CrossLocalization.Translate("settings_server_change_message"));

			if (!result) {
				SelectedItem = null;
				return;
			}

			changeServer(server);
			_device.MainThread(() => _pages.OpenLogin());
		}

		void changeServer(ServerPageModel server)
		{
			Servers.SetCurrent(server.Address);
			AppPrefs.IsLoggedIn = false;
			AppUserData.Clear();
			DataAccess.ResetData();
			toggleServer(server);
		}

		void toggleServer(ServerPageModel server)
		{
			var servers = ServerList.Select(s => {
				s.IsChecked = server.Address == s.Address;
				return s;
			});

			ServerList = new List<ServerPageModel>(servers);
		}

		ServerPageModel getServerDetails(string serverAddress, string descriptionLocalizedKey)
		{
			return new ServerPageModel {
				Address = serverAddress,
				Title = Servers.GetServerType(serverAddress),
				Description = CrossLocalization.Translate(descriptionLocalizedKey),
				IsChecked = AppPrefs.Server == serverAddress
			};
		}
	}
}
