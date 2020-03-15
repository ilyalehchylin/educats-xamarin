using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.User;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Pages.Testing.Base.Models;
using EduCATS.Pages.Utils.ViewModels;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Base.ViewModels
{
	public class TestingPageViewModel : SubjectsViewModel
	{
		readonly IDialogs _dialogService;

		public TestingPageViewModel(IDialogs dialogService) : base(dialogService)
		{
			_dialogService = dialogService;

			Task.Run(async () => {
				await SetupSubjects();
				await getAndSetTests();
			});

			SubjectChanged += async (id, name) => {
				await getAndSetTests();
			};
		}

		bool _isRefreshing;
		public bool IsRefreshing {
			get { return _isRefreshing; }
			set { SetProperty(ref _isRefreshing, value); }
		}

		object _selectedItem;
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);

				if (_selectedItem != null) {
					openTest();
				}
			}
		}

		List<TestingGroupModel> _testList;
		public List<TestingGroupModel> TestList {
			get { return _testList; }
			set { SetProperty(ref _testList, value); }
		}

		Command _refreshCommand;
		public Command RefreshCommand {
			get {
				return _refreshCommand ?? (
					_refreshCommand = new Command(async () => await executeRefreshCommand()));
			}
		}

		async Task getAndSetTests()
		{
			var tests = await getTests();
			TestList = new List<TestingGroupModel>(tests);
		}

		async Task<List<TestingGroupModel>> getTests()
		{
			var item = await DataAccess.GetAvailableTests(CurrentSubject.Id, AppUserData.UserId);

			if (item.IsError) {
				await _dialogService.ShowError(item.ErrorMessage);
				return null;
			}

			var testsForSelfStudy = new TestingGroupModel(
				CrossLocalization.Translate("testing_self_study"));

			var testsForControl = new TestingGroupModel(
				CrossLocalization.Translate("testing_knowledge_control"));

			foreach (var test in item?.Tests) {
				if (test.ForSelfStudy) {
					testsForSelfStudy.Add(test);
				} else {
					testsForControl.Add(test);
				}
			}

			return new List<TestingGroupModel> {
				testsForSelfStudy,
				testsForControl
			};
		}

		void openTest()
		{
			var result = _dialogService.ShowConfirmationMessage(
				CrossLocalization.Translate("testing_start_test_title"),
				CrossLocalization.Translate("testing_start_test_description"))
				.ContinueWith(response => {
					if (response.Result) {
						// open page
					}
				});
		}

		protected async Task executeRefreshCommand()
		{
				IsRefreshing = true;
				await getAndSetTests();
				IsRefreshing = false;
		}
	}
}
