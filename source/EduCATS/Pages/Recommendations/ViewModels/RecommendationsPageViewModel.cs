using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.User;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Pages.Pickers;
using EduCATS.Pages.Recommendations.Models;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Recommendations.ViewModels
{
	public class RecommendationsPageViewModel : SubjectsViewModel
	{
		readonly IPages _navigation;

		public RecommendationsPageViewModel(
			IDialogs dialogs, IDevice device, IPages navigation) : base(dialogs, device)
		{
			_navigation = navigation;
			update();
		}

		List<RecommendationsPageModel> _recommendations;
		public List<RecommendationsPageModel> Recommendations {
			get { return _recommendations; }
			set { SetProperty(ref _recommendations, value); }
		}

		bool _isLoading;
		public bool IsLoading {
			get { return _isLoading; }
			set { SetProperty(ref _isLoading, value); }
		}

		object _selectedItem;
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);

				if (_selectedItem != null) {
					openRecommendation(_selectedItem);
				}
			}
		}

		Command _refreshCommand;
		public Command RefreshCommand {
			get {
				return _refreshCommand ?? (
					_refreshCommand = new Command(update));
			}
		}

		void update()
		{
			DeviceService.MainThread(async () => {
				IsLoading = true;
				await SetupSubjects();
				await getRecList();
				IsLoading = false;
			});
		}

		async Task getRecList()
		{
			var recsList = await DataAccess.GetRecommendations(CurrentSubject.Id, AppUserData.UserId);

			if (DataAccess.IsError && !DataAccess.IsConnectionError) {
				DialogService.ShowError(DataAccess.ErrorMessage);
			}

			var recommendations = recsList?.Select(r => new RecommendationsPageModel(r));

			if (recommendations != null) {
				Recommendations = new List<RecommendationsPageModel>(recommendations);
			}
		}

		void openRecommendation(object selectedObject)
		{
			if (selectedObject == null || !(selectedObject is RecommendationsPageModel)) {
				return;
			}

			var recommedation = selectedObject as RecommendationsPageModel;

			if (recommedation.IsTest) {
				_navigation.OpenTestPassing(recommedation.Id, true);
			} else {
				_navigation.OpenEemc(
					CrossLocalization.Translate("learning_card_eemc"),
					recommedation.Id);
			}
		}
	}
}
