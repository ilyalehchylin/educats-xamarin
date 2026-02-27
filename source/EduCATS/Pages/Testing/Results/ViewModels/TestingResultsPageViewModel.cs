using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Date;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using Newtonsoft.Json;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Results.ViewModels
{
	public class TestingResultsPageViewModel : ViewModel
	{
		const int _maximumRating = 10;

		readonly IPlatformServices _services;
		readonly int _testId;
		readonly bool _fromComplexLearning;

		public TestingResultsPageViewModel(
			int testId,
			bool fromComplexLearning,
			string timePassed,
			IPlatformServices services)
		{
			_services = services;
			_testId = testId;
			_fromComplexLearning = fromComplexLearning;
			TimePassed = timePassed;

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

		string _timePassed;
		public string TimePassed {
			get { return _timePassed; }
			set {
				if (SetProperty(ref _timePassed, value)) {
					IsTimePassedVisible = !string.IsNullOrWhiteSpace(value);
				}
			}
		}

		bool _isTimePassedVisible;
		public bool IsTimePassedVisible {
			get { return _isTimePassedVisible; }
			set { SetProperty(ref _isTimePassedVisible, value); }
		}

		Command _closeCommand;
		public Command CloseCommand {
			get {
				return _closeCommand ?? (_closeCommand = new Command(closeCommand));
			}
		}

		async Task getResults()
		{
			List<TestResultsModel> resultList = null;
			ExtendedTestResultModel extendedResultList = await DataAccess.GetUserAnswers(_testId);

			if (DataAccess.IsError || extendedResultList?.Data == null) {
				var errorMessage = string.IsNullOrWhiteSpace(DataAccess.ErrorMessage) ?
					CrossLocalization.Translate("test_results_error") :
					DataAccess.ErrorMessage;
				_services.Dialogs.ShowError(errorMessage);
				Results = new List<TestResultsModel>();
				return;
			}

			var markData = getDataByKey(extendedResultList, "Mark");
			if (markData.Value != null) {
				Mark = markData.Value.ToString();
			}

			if (string.IsNullOrWhiteSpace(TimePassed)) {
				TimePassed = getTimePassed(extendedResultList);
			}

			KeyValuePair<string, object> answer = getDataByKey(extendedResultList, "Answers");
			if (answer.Value != null) {
				try {
					resultList = JsonConvert.DeserializeObject<List<TestResultsModel>>(answer.Value.ToString());
				} catch {
					resultList = new List<TestResultsModel>();
				}
			}

			if (resultList == null) {
				resultList = new List<TestResultsModel>();
			}

			Results = new List<TestResultsModel>(resultList);
		}

		protected void closeCommand()
		{
			try {
				if (!_fromComplexLearning) {
					_services.Navigation.ClosePage(true, false);
					_services.Navigation.ClosePage(true, false);
				}
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		void estimateRating()
		{
			if (!string.IsNullOrWhiteSpace(Mark)) {
				return;
			}

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

		static KeyValuePair<string, object> getDataByKey(ExtendedTestResultModel extendedResult, string key)
		{
			if (extendedResult?.Data == null || string.IsNullOrWhiteSpace(key)) {
				return default;
			}

			return extendedResult.Data.SingleOrDefault(x =>
				x.Key != null &&
				x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
		}

		static string getTimePassed(ExtendedTestResultModel extendedResult)
		{
			var start = getDataByKey(extendedResult, "StartTime").Value?.ToString();
			var end = getDataByKey(extendedResult, "EndTime").Value?.ToString();

			if (string.IsNullOrWhiteSpace(start) || string.IsNullOrWhiteSpace(end)) {
				return null;
			}

			try {
				var startUnix = DateHelper.GetUnixFromString(start);
				var endUnix = DateHelper.GetUnixFromString(end);
				var startDate = DateHelper.Convert13DigitsUnixToDateTime(startUnix);
				var endDate = DateHelper.Convert13DigitsUnixToDateTime(endUnix);

				if (endDate <= startDate) {
					return null;
				}

				return DateHelper.CheckDatesDifference(startDate, endDate).ToString(@"hh\:mm\:ss");
			} catch {
				return null;
			}
		}
	}
}
