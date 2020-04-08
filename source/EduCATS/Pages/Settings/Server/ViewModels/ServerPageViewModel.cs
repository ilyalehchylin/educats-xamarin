using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.User;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
using EduCATS.Pages.Settings.Server.Models;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Pages.Settings.Server.ViewModels
{
	public class ServerPageViewModel : ViewModel
	{
		readonly IPlatformServices _services;

		public ServerPageViewModel(IPlatformServices services)
		{
			_services = services;
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
				_services.Device.MainThread(async () => await selectServer(_selectedItem));
			}
		}

		void setServers()
		{
			try {
				ServerList = new List<ServerPageModel> {
					getServerDetails(Servers.EduCatsBntuAddress, "server_stable"),
					getServerDetails(Servers.EduCatsAddress, "server_test"),
					getServerDetails(Servers.LocalAddress, "server_local")
				};
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		async Task selectServer(object selectedObject)
		{
			try {
				if (selectedObject == null || !(selectedObject is ServerPageModel)) {
					return;
				}

				var server = (ServerPageModel)selectedObject;

				if (!_services.Preferences.IsLoggedIn) {
					changeServer(server);
					return;
				}

				var result = await _services.Dialogs.ShowConfirmationMessage(
					CrossLocalization.Translate("base_warning"),
					CrossLocalization.Translate("settings_server_change_message"));

				if (!result) {
					SelectedItem = null;
					return;
				}

				changeServer(server);
				_services.Device.MainThread(() => _services.Navigation.OpenLogin());
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		void changeServer(ServerPageModel server)
		{
			Servers.SetCurrent(server.Address);
			_services.Preferences.IsLoggedIn = false;
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
				IsChecked = _services.Preferences.Server == serverAddress
			};
		}
	}
}
