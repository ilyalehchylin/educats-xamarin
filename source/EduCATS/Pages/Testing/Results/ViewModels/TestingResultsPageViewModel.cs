using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.Testing.Results;
using EduCATS.Data.User;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Results.ViewModels
{
	public class TestingResultsPageViewModel : ViewModel
	{
		const int maximumRating = 10;

		readonly IDialogs _dialogs;
		readonly IPages _navigation;
		readonly int _testId;
		readonly bool _fromComplexLearning;

		public TestingResultsPageViewModel(
			int testId, bool fromComplexLearning, IDialogs dialogs, IPages navigation)
		{
			_dialogs = dialogs;
			_navigation = navigation;
			_testId = testId;
			_fromComplexLearning = fromComplexLearning;

			Task.Run(async () => {
				await getResults();
				estimateRating();
			});
		}

		List<TestingResultsModel> results;
		public List<TestingResultsModel> Results {
			get { return results; }
			set { SetProperty(ref results, value); }
		}

		string mark;
		public string Mark {
			get { return mark; }
			set { SetProperty(ref mark, value); }
		}

		Command closeCommand;
		public Command CloseCommand {
			get {
				return closeCommand ?? (closeCommand = new Command(executeCloseCommand));
			}
		}

		async Task getResults()
		{
			var resultList = await DataAccess.GetUserAnswers(AppUserData.UserId, _testId);

			if (DataAccess.IsError) {
				_dialogs.ShowError(DataAccess.ErrorMessage);
				return;
			}

			Results = new List<TestingResultsModel>(resultList);
		}

		protected void executeCloseCommand()
		{
			if (_fromComplexLearning) {

			} else {
				_navigation.ClosePage(true);
				_navigation.ClosePage(true);
			}
		}

		void estimateRating()
		{
			if (Results == null || Results.Count == 0) {
				return;
			}

			int correctAnswers = 0;
			Results.ForEach(r => {
				if (r.Points != 0) {
					correctAnswers++;
				}
			});

			Mark = Convert.ToInt32(
				correctAnswers * maximumRating / Results.Count).ToString();
		}
	}
}
