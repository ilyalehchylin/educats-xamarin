using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Pages.Testing.Base.Models;
using EduCATS.Pages.Utils.ViewModels;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Base.ViewModels
{
	public class TestingPageViewModel : SubjectsViewModel
	{
		IDialogs _dialogService;

		public TestingPageViewModel(IDialogs dialogService) : base(dialogService)
		{
			_dialogService = dialogService;

			Task.Run(async () => {
				await SetupSubjects();
			});

			SubjectChanged += (id, name) => {

			};
		}

		bool isRefreshing;
		public bool IsRefreshing {
			get { return isRefreshing; }
			set { SetProperty(ref isRefreshing, value); }
		}

		object selectedItem;
		public object SelectedItem {
			get { return selectedItem; }
			set {
				SetProperty(ref selectedItem, value);

				if (selectedItem != null) {
					//startTestConfirmation((TestModel)selectedItem);
				}
			}
		}

		List<TestingGroupModel> testList;
		public List<TestingGroupModel> TestList {
			get { return testList; }
			set { SetProperty(ref testList, value); }
		}

		Command refreshCommand;
		public Command RefreshCommand {
			get {
				return refreshCommand ?? (
					refreshCommand = new Command(async () => await executeRefreshCommand());
			}
		}

		protected async Task executeRefreshCommand()
		{
				IsRefreshing = true;
				//await getTests();
				IsRefreshing = false;
		}
	}
}
