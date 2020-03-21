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
		const int _maximumRating = 10;

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

		List<TestResultsModel> _results;
		public List<TestResultsModel> Results {
			get { return _results; }
			set { SetProperty(ref _results, value); }
		}

		string _mark;
		public string Mark {
			get { return _mark; }
			set { SetProperty(ref _mark, value); }
		}

		Command _closeCommand;
		public Command CloseCommand {
			get {
				return _closeCommand ?? (_closeCommand = new Command(closeCommand));
			}
		}

		async Task getResults()
		{
			var resultList = await DataAccess.GetUserAnswers(AppUserData.UserId, _testId);

			if (DataAccess.IsError) {
				_dialogs.ShowError(DataAccess.ErrorMessage);
				return;
			}

			Results = new List<TestResultsModel>(resultList);
		}

		protected void closeCommand()
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
				correctAnswers * _maximumRating / Results.Count).ToString();
		}
	}
}
