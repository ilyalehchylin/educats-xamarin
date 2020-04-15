using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.User;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Pickers;
using EduCATS.Pages.Recommendations.Models;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Recommendations.ViewModels
{
	public class RecommendationsPageViewModel : SubjectsViewModel
	{
		public RecommendationsPageViewModel(IPlatformServices services) : base(services)
		{
			SubjectChanged += async (id, name) => await update(true);
			Task.Run(async () => await update(true));
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
					_refreshCommand = new Command(async () => await update(false)));
			}
		}

		/// <summary>
		/// Update with dialog or pull-to-refresh.
		/// </summary>
		/// <param name="dialog">Is dialog.</param>
		/// <returns>Task.</returns>
		async Task update(bool dialog)
		{
			if (dialog) {
				PlatformServices.Dialogs.ShowLoading();
			} else {
				IsLoading = true;
			}

			await update();

			if (dialog) {
				PlatformServices.Dialogs.HideLoading();
			} else {
				IsLoading = false;
			}
		}

		async Task update()
		{
			try {
				await SetupSubjects();
				await getRecList();
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		async Task getRecList()
		{
			var recsList = await DataAccess.GetRecommendations(CurrentSubject.Id, AppUserData.UserId);

			if (DataAccess.IsError && !DataAccess.IsConnectionError) {
				PlatformServices.Device.MainThread(
					() => PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage));
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
				PlatformServices.Navigation.OpenTestPassing(recommedation.Id, true);
			} else {
				PlatformServices.Navigation.OpenEemc(
					CrossLocalization.Translate("learning_card_eemc"),
					recommedation.Id);
			}
		}
	}
}
