using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Extensions;
using EduCATS.Helpers.Date;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Networking.Models.Testing;
using EduCATS.Pages.Testing.Passing.Models;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;
using EduCATS.Networking;

namespace EduCATS.Pages.Testing.Passing.ViewModels
{
	public partial class TestPassingPageViewModel : ViewModel
	{
		readonly IPlatformServices _services;
		readonly DateTime _startedEntireTest;
		readonly int _testId;

		bool _timerCancellation;
		int _timeForCompletion;
		bool _isBusySpeech;

		/// <summary>
		/// Time for question/entire test completion.
		/// </summary>
		/// <remarks>
		/// If <c>_isTimeForEntireTest</c> is set to <c>true</c>,
		/// the time will be in minutes, in seconds otherwise.
		/// </remarks>
		bool _isTimeForEntireTest;

		int _questionCount;
		int _questionsLeft;
		string _testIdString;
		int _questionNumber;
		int _questionType;
		DateTime _started;

		public TestPassingPageViewModel(IPlatformServices services, int testId, bool forSelfStudy)
		{
			_startedEntireTest = DateTime.Now;
			_services = services;
			_testId = testId;
			_testIdString = testId.ToString();
			IsTestForSelfStudy = forSelfStudy;
			HeadphonesIcon = Theme.Current.BaseHeadphonesIcon;
			getData(1);
		}

		bool _isNotLoading;
		public bool IsNotLoading {
			get { return _isNotLoading; }
			set { SetProperty(ref _isNotLoading, value); }
		}

		bool _isTestForSelfStudy;
		public bool IsTestForSelfStudy {
			get { return _isTestForSelfStudy; }
			set { SetProperty(ref _isTestForSelfStudy, value); }
		}

		object _selectedItem;
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);

				if (_selectedItem != null) {
					selectItem(_selectedItem);
				}
			}
		}

		string _title;
		public string Title {
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		string _question;
		public string Question {
			get { return _question; }
			set { SetProperty(ref _question, value); }
		}

		string _description;
		public string Description {
			get { return _description; }
			set { SetProperty(ref _description, value); }
		}

		string _headphonesIcon;
		public string HeadphonesIcon {
			get { return _headphonesIcon; }
			set { SetProperty(ref _headphonesIcon, value); }
		}

		List<TestPassingAnswerModel> _answers;
		public List<TestPassingAnswerModel> Answers {
			get { return _answers; }
			set { SetProperty(ref _answers, value); }
		}

		Command _answerCommand;
		public Command AnswerCommand {
			get {
				return _answerCommand ?? (_answerCommand = new Command(
					async () => await ExecuteAnswerCommand()));
			}
		}

		Command _skipCommand;
		public Command SkipCommand {
			get {
				return _skipCommand ?? (_skipCommand = new Command(
					async () => await ExecuteSkipCommand()));
			}
		}

		Command _speechCommand;
		public Command SpeechCommand {
			get {
				return _speechCommand ?? (_speechCommand = new Command(
					async () => await speechToText()));
			}
		}

		Command _closeCommand;
		public Command CloseCommand {
			get {
				return _closeCommand ?? (_closeCommand = new Command(
					async () => await closePage()));
			}
		}

		void getData(int number)
		{
			_services.Device.MainThread(async () => {
				try {
					setLoading(true);
					await getAndSetTest();
					await getAndSetQuestion(number);
					setTimer();
					_questionsLeft = _questionCount - _questionNumber + 1;
					setLoading(false);
				} catch (Exception ex) {
					AppLogs.Log(ex);
				}
			});
		}

		async Task getAndSetTest()
		{
			var test = await getTest();
			setTestData(test);
		}

		async Task<TestDetailsModel> getTest()
		{
			var test = await DataAccess.GetTest(_testId);

			if (DataAccess.IsError) {
				_services.Dialogs.ShowError(DataAccess.ErrorMessage);
				return new TestDetailsModel();
			}

			return test;
		}

		async Task getAndSetQuestion(int number)
		{
			if (number == -1)
			{
				if (Servers.EduCatsBntuAddress == _services.Preferences.Server)
				{
					await getQuestion(number);
				}
				completeTest();
			}
			else
			{
				var question = await getQuestion(number);

				if (question.Question == null)
				{
					completeTest();
				}
				else
				{
					setQuestionData(question);
				}
			}
		}

		async Task<TestQuestionModel> getQuestion(int number)
		{
			var question = await DataAccess.GetNextQuestion(
				_testId, number, AppUserData.UserId);

			if (DataAccess.IsError && !DataAccess.IsConnectionError) {
				_services.Dialogs.ShowError(DataAccess.ErrorMessage);
				return new TestQuestionModel();
			}

			return question;
		}

		async Task answerQuestion(TestAnswerPostModel answerModel, bool isAuto = false)
		{
			try {
				if (answerModel == null || answerModel.Answers == null || answerModel.Answers.Count == 0) {
					_services.Dialogs.ShowError(CrossLocalization.Translate("answer_question_not_selected_error"));
					return;
				}

				await DataAccess.AnswerQuestionAndGetNext(answerModel);

				if (DataAccess.IsError) {
					_services.Dialogs.ShowError(DataAccess.ErrorMessage);
					return;
				}

				if (!isAuto) {
					_questionsLeft--;
				}

				await getAndSetQuestion(getNextQuestion());
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		void setQuestionData(TestQuestionModel testQuestionCommonModel)
		{
			var testQuestionModel = testQuestionCommonModel.Question;

			if (testQuestionModel != null) {
				Question = testQuestionModel.Title;
				Description = $"<head><meta charset=\"utf-8\">" +
					$"<font size=\"5\" " +
					$"color=\"{Theme.Current.TestPassingQuestionColor}\">" +
					$"{testQuestionModel.Description}" +
					$"</font>";
				_questionNumber = testQuestionCommonModel.Number;
				_questionType = testQuestionModel.QuestionType;
				setAnswers(testQuestionModel.Answers);
			}
		}

		void setAnswers(List<TestAnswerModel> testQuestionAnswers)
		{
			var answersList = testQuestionAnswers?.Select(
				a => new TestPassingAnswerModel(a, _questionType) {
					DownMovableAnswerCommand = new Command(ExecuteDownMovableAnswerCommand),
					UpMovableAnswerCommand = new Command(ExecuteUpMovableAnswerCommand)
				}).ToList();

			if (_questionType < 2 && answersList.Count > 0) {
				answersList[0].IsSelected = true;
			}

			Answers = new List<TestPassingAnswerModel>(answersList);
		}

		void setTitle(TimeSpan timeLeft)
		{
			var questionDetails = $"{_questionNumber}/{_questionCount}";
			var timeLeftFormatted = timeLeft.ToString(@"hh\:mm\:ss");
			Title = $"{CrossLocalization.Translate("question_title")} {questionDetails} ({timeLeftFormatted})";
		}

		void completeTest()
		{
			var testTimePassed = DateHelper.CheckDatesDifference(_startedEntireTest, DateTime.Now);
			_timerCancellation = true;
			_services.Navigation.OpenTestResults(_testId, false, testTimePassed.ToString(@"hh\:mm\:ss"));
		}

		void autoAnswerAllQuestions()
		{
			_services.Device.MainThread(async () => {
				for (int i = 0; i < _questionsLeft; i++) {
					await answerQuestion(true);
				}
			});
		}

		void completeQuestion()
		{
			_services.Device.MainThread(async () => {
				await answerQuestion(true);
				_started = DateTime.Now;
			});
		}

		void selectItem(object item)
		{
			try {
				if (item == null || item.GetType() != typeof(TestPassingAnswerModel)) {
					return;
				}

				var answer = item as TestPassingAnswerModel;
				var answers = new List<TestPassingAnswerModel>(Answers);

				switch (_questionType) {
					case 0:
						answers.ForEach(a => a.IsSelected = a == answer ? true : false);
						break;
					case 1:
						var index = answers.IndexOf(answer);
						answers[index].IsSelected = !answers[index].IsSelected;
						break;
				}

				Answers = new List<TestPassingAnswerModel>(answers);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		protected async Task speechToText()
		{
			try {
				if (Answers == null || string.IsNullOrEmpty(Question)) {
					return;
				}

				if (_isBusySpeech) {
					_isBusySpeech = false;
					_services.Device.CancelSpeech();
					HeadphonesIcon = Theme.Current.BaseHeadphonesIcon;
					return;
				}

				HeadphonesIcon = Theme.Current.BaseHeadphonesCancelIcon;
				_isBusySpeech = true;
				await _services.Device.Speak(Question);

				if (!_isBusySpeech) {
					return;
				}

				var formattedDescription = Description.RemoveHTMLTags();

				if (!string.IsNullOrEmpty(formattedDescription)) {
					await _services.Device.Speak(formattedDescription);
				}

				if (!_isBusySpeech) {
					return;
				}

				await _services.Device.Speak(CrossLocalization.Translate("test_passing_options"));

				if (!_isBusySpeech) {
					return;
				}

				for (var i = 0; i < Answers.Count; i++) {
					var answer = Answers[i];

					if (!_isBusySpeech) {
						return;
					}

					await _services.Device.Speak((i + 1).ToString());

					if (!_isBusySpeech) {
						return;
					}

					if (!string.IsNullOrEmpty(answer.Content)) {
						await _services.Device.Speak(answer.Content);
					}
				}

				_isBusySpeech = false;
				HeadphonesIcon = Theme.Current.BaseHeadphonesIcon;
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		protected async Task ExecuteAnswerCommand()
		{
			await answerQuestion();

			if (!_isTimeForEntireTest) {
				_started = DateTime.Now;
			}
		}

		protected void ExecuteUpMovableAnswerCommand(object obj)
		{
			moveAnswer(obj, false);
		}

		protected void ExecuteDownMovableAnswerCommand(object obj)
		{
			moveAnswer(obj, true);
		}

		protected async Task ExecuteSkipCommand()
		{
			try {
				setLoading(true);
				await getAndSetQuestion(getNextQuestion());

				if (!_isTimeForEntireTest) {
					_started = DateTime.Now;
				}

				setLoading(false);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		protected async Task closePage()
		{
			try {
				var result = await _services.Dialogs.ShowConfirmationMessage(
					CrossLocalization.Translate("base_warning"),
					CrossLocalization.Translate("test_passing_cancel_message"));

				if (!result) {
					return;
				}

				_timerCancellation = true;

				if (_isBusySpeech) {
					_isBusySpeech = false;
					_services.Device.CancelSpeech();
				}

				await _services.Navigation.ClosePage(true, false);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		void setLoading(bool loading)
		{
			_services.Device.MainThread(() => {
				if (loading) {
					_services.Dialogs.ShowLoading();
				} else {
					_services.Dialogs.HideLoading();
				}

				IsNotLoading = !loading;
			});
		}
	}
}
