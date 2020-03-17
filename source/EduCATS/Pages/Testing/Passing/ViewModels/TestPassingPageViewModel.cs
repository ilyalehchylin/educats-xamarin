using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.Testing.Passing;
using EduCATS.Data.User;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Networking.Models.Testing;
using EduCATS.Pages.Testing.Passing.Models;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Passing.ViewModels
{
	public partial class TestPassingPageViewModel : ViewModel
	{
		readonly bool _fromComplexLearning;
		readonly IDialogs _dialogs;
		readonly IPages _navigation;
		readonly IAppDevice _device;
		readonly int _testId;

		bool _timerCancellation;
		int _timeForCompletion;

		/// <summary>
		/// Time for question/entire test completion.
		/// If _isTimeForEntireTest is set to TRUE,
		/// the time will be in minutes, otherwise - in seconds.
		/// </summary>
		bool _isTimeForEntireTest;
		int _questionCount;
		string _testIdString;
		int _questionNumber;
		int _questionType;
		DateTime _testStarted;
		DateTime _questionStarted;

		public TestPassingPageViewModel(
			IDialogs dialogs, IPages navigation, IAppDevice device,
			int testId, bool forSelfStudy, bool fromComplexLearning)
		{
			_fromComplexLearning = fromComplexLearning;
			_dialogs = dialogs;
			_navigation = navigation;
			_device = device;
			_testId = testId;
			_testIdString = testId.ToString();
			IsTestForSelfStudy = forSelfStudy;
			Task.Run(async () => await getData(1));
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

		List<TestPassingAnswerModel> _answers;
		public List<TestPassingAnswerModel> Answers {
			get { return _answers; }
			set { SetProperty(ref _answers, value); }
		}

		Command answerCommand;
		public Command AnswerCommand {
			get {
				return answerCommand ?? (answerCommand = new Command(
					async () => await ExecuteAnswerCommand()));
			}
		}

		Command skipCommand;
		public Command SkipCommand {
			get {
				return skipCommand ?? (skipCommand = new Command(
					async () => await ExecuteSkipCommand()));
			}
		}

		Command closeCommand;
		public Command CloseCommand {
			get {
				return closeCommand ?? (closeCommand = new Command(
					async () => await ExecuteCloseCommand()));
			}
		}

		async Task getData(int number)
		{
			setLoading(true);
			await getAndSetTest();
			await getAndSetQuestion(number);
			setTimer();
			setLoading(false);
		}

		async Task getAndSetTest()
		{
			var test = await getTest();
			setTestData(test);
		}

		async Task<TestDetailsModel> getTest()
		{
			var test = await DataAccess.GetTest(_testId);

			if (test.IsError) {
				await _dialogs.ShowError(test.ErrorMessage);
				await _navigation.ClosePage(true);
				return new TestDetailsModel();
			}

			return test;
		}

		async Task getAndSetQuestion(int number)
		{
			var question = await getQuestion(number);

			if (question.Question == null) {
				completeTest();
			} else {
				setQuestionData(question);
			}
		}

		async Task<TestQuestionCommonModel> getQuestion(int number)
		{
			var question = await DataAccess.GetNextQuestion(
				_testId, number, AppUserData.UserId);

			if (question.IsError) {
				await _dialogs.ShowError(question.ErrorMessage);
				await _navigation.ClosePage(true);
				return new TestQuestionCommonModel();
			}

			return question;
		}

		async Task answerCommonQuestion(TestingCommonAnswerPostModel answerModel)
		{
			if (answerModel == null || answerModel.Answers == null || answerModel.Answers.Count == 0) {
				await _dialogs.ShowError(CrossLocalization.Translate("answer_question_not_selected_error"));
				return;
			}

			var data = await DataAccess.AnswerQuestionAndGetNext(answerModel);

			if (data.IsError) {
				await _dialogs.ShowError(data.ErrorMessage);
				return;
			}

			if (!_isTimeForEntireTest) {
				_timerCancellation = true;
			}

			await getAndSetQuestion(getNextQuestion());
		}

		void setQuestionData(TestQuestionCommonModel testQuestionCommonModel)
		{
			var testQuestionModel = testQuestionCommonModel.Question;

			if (testQuestionModel != null) {
				Question = testQuestionModel.Title;
				Description = $"<head><meta charset=\"utf-8\"><font size=\"5\">{testQuestionModel.Description}</font>";
				_questionNumber = testQuestionCommonModel.Number;
				_questionType = testQuestionModel.QuestionType;
				setAnswers(testQuestionModel.Answers);
			}
		}

		void setAnswers(List<TestQuestionAnswersModel> testQuestionAnswers)
		{
			var answersList = testQuestionAnswers?.Select(
				a => new TestPassingAnswerModel(a, _questionType) {
					DownMovableAnswerCommand = new Command(ExecuteDownMovableAnswerCommand),
					UpMovableAnswerCommand = new Command(ExecuteUpMovableAnswerCommand)
				});

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
			_timerCancellation = true;
			// open results page
		}

		void completeQuestion()
		{
			_timerCancellation = true;
		}

		void selectItem(object item)
		{
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
		}

		protected async Task ExecuteAnswerCommand()
		{
			await answerQuestion();
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
			setLoading(true);
			await getAndSetQuestion(getNextQuestion());
			setLoading(false);
		}

		protected async Task ExecuteCloseCommand()
		{
			var result = await _dialogs.ShowConfirmationMessage(
				CrossLocalization.Translate("common_warning"),
				CrossLocalization.Translate("test_passing_cancel_message"));

			if (result) {
				await _navigation.ClosePage(true);
			}
		}

		void setLoading(bool loading)
		{
			if (loading) {
				_dialogs.ShowLoading();
			} else {
				_dialogs.HideLoading();
			}

			IsNotLoading = !loading;
		}
	}
}
