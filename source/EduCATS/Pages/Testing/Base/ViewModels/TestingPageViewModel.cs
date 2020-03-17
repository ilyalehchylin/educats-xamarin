using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.Testing.Base;
using EduCATS.Data.User;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Pages.Testing.Base.Models;
using EduCATS.Pages.Utils.ViewModels;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Base.ViewModels
{
	public class TestingPageViewModel : SubjectsViewModel
	{
		readonly IDialogs _dialogService;
		readonly IPages _navigation;
		readonly IAppDevice _device;

		public TestingPageViewModel(IDialogs dialogService, IPages navigation, IAppDevice device)
			: base(dialogService, device)
		{
			_dialogService = dialogService;
			_navigation = navigation;
			_device = device;

			Task.Run(async () => await update());
			SubjectChanged += async (id, name) => await update();
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
					openTest(_selectedItem);
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

		async Task update()
		{
			await SetupSubjects();
			await getAndSetTests();
		}

		async Task getAndSetTests()
		{
			var tests = await getTests();
			TestList = new List<TestingGroupModel>(tests);
		}

		async Task<List<TestingGroupModel>> getTests()
		{
			var tests = await DataAccess.GetAvailableTests(CurrentSubject.Id, AppUserData.UserId);

			if (DataAccess.IsError && !DataAccess.IsConnectionError) {
				_device.MainThread(
					() => _dialogService.ShowError(DataAccess.ErrorMessage));
			}

			var testsForSelfStudy = getGroup(tests, "testing_self_study", true);
			var testsForControl = getGroup(tests, "testing_knowledge_control", false);
			var groups = new List<TestingGroupModel>();
			groups = addNonEmptyGroup(groups, testsForSelfStudy);
			groups = addNonEmptyGroup(groups, testsForControl);
			return groups;
		}

		List<TestingGroupModel> addNonEmptyGroup(List<TestingGroupModel> groups, TestingGroupModel group)
		{
			if (group.Count > 0) {
				groups.Add(group);
			}

			return groups;
		}

		TestingGroupModel getGroup(IList<TestingItemModel> tests, string localizedTag, bool isSelfStudy)
		{
			return new TestingGroupModel(
				CrossLocalization.Translate(localizedTag),
				getSeparateTests(tests, isSelfStudy));
		}

		List<TestingItemModel> getSeparateTests(IList<TestingItemModel> tests, bool isSelfStudy)
		{
			return tests.Where(
				t => t.ForSelfStudy.Equals(isSelfStudy)).ToList();
		}

		void openTest(object testObject)
		{
			if (testObject == null || testObject.GetType() != typeof(TestingItemModel)) {
				return;
			}

			var test = testObject as TestingItemModel;
			_device.MainThread((
				async () => await showStartTestDialog(test.Id, test.ForSelfStudy)));
		}

		async Task showStartTestDialog(int testId, bool forSelfStudy)
		{
			var result = await _dialogService.ShowConfirmationMessage(
				CrossLocalization.Translate("testing_start_test_title"),
				CrossLocalization.Translate("testing_start_test_description"));

			if (result) {
				await _navigation.OpenTestPassing(testId, forSelfStudy);
			}
		}

		protected async Task executeRefreshCommand()
		{
			IsRefreshing = true;
			await getAndSetTests();
			IsRefreshing = false;
		}
	}
}
