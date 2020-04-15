using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Pickers;
using EduCATS.Pages.Testing.Base.Models;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Base.ViewModels
{
	public class TestingPageViewModel : SubjectsViewModel
	{
		public TestingPageViewModel(IPlatformServices services) : base(services)
		{
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
					_refreshCommand = new Command(async () => await refresh()));
			}
		}

		async Task update()
		{
			try {
				PlatformServices.Dialogs.ShowLoading();
				await SetupSubjects();
				await getAndSetTests();
				PlatformServices.Dialogs.HideLoading();
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
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
				PlatformServices.Device.MainThread(
					() => PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage));
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

		TestingGroupModel getGroup(IList<TestModel> tests, string localizedTag, bool isSelfStudy)
		{
			return new TestingGroupModel(
				CrossLocalization.Translate(localizedTag),
				getSeparateTests(tests, isSelfStudy));
		}

		List<TestModel> getSeparateTests(IList<TestModel> tests, bool isSelfStudy)
		{
			return tests.Where(
				t => t.ForSelfStudy.Equals(isSelfStudy)).ToList();
		}

		void openTest(object testObject)
		{
			try {
				if (testObject == null || testObject.GetType() != typeof(TestModel)) {
					return;
				}

				var test = testObject as TestModel;
				PlatformServices.Device.MainThread(
					async () => await showStartTestDialog(test.Id, test.ForSelfStudy));
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		async Task showStartTestDialog(int testId, bool forSelfStudy)
		{
			var result = await PlatformServices.Dialogs.ShowConfirmationMessage(
				CrossLocalization.Translate("testing_start_test_title"),
				CrossLocalization.Translate("testing_start_test_description"));

			if (result) {
				await PlatformServices.Navigation.OpenTestPassing(testId, forSelfStudy);
			}
		}

		protected async Task refresh()
		{
			try {
				IsRefreshing = true;
				await getAndSetTests();
				IsRefreshing = false;
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}
	}
}
