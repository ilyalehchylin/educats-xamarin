using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Results.ViewModels
{
	public class TestingResultsPageViewModel : ViewModel
	{
		const int _maximumRating = 10;

		readonly IPlatformServices _services;
		readonly int _testId;
		readonly bool _fromComplexLearning;

		public TestingResultsPageViewModel(int testId, bool fromComplexLearning, IPlatformServices services)
		{
			_services = services;
			_testId = testId;
			_fromComplexLearning = fromComplexLearning;

			Task.Run(async () => {
				try {
					await getResults();
					estimateRating();
				} catch (Exception ex) {
					AppLogs.Log(ex);
				}
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
				_services.Dialogs.ShowError(DataAccess.ErrorMessage);
				return;
			}

			Results = new List<TestResultsModel>(resultList);
		}

		protected void closeCommand()
		{
			try {
				if (!_fromComplexLearning) {
					_services.Navigation.ClosePage(true);
					_services.Navigation.ClosePage(true);
				}
			} catch (Exception ex) {
				AppLogs.Log(ex);
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
