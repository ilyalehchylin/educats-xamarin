using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.Testing.Base;
using EduCATS.Data.Models.Testing.Passing;
using EduCATS.Data.User;
using EduCATS.Helpers.Date;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Lists;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Networking.Models.Testing;
using EduCATS.Pages.Testing.Passing.Models;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Passing.ViewModels
{
	public class TestPassingPageViewModel : ViewModel
	{
		/// <summary>
		/// Time for question/entire test completion.
		/// If isTimeForEntireTest is set to TRUE,
		/// the time will be in minutes, otherwise - in seconds.
		/// </summary>
		readonly bool _fromComplexLearning;
		readonly TestingItemModel _testModel;
		readonly IDialogs _dialogs;
		readonly IPages _navigation;

		bool _timerCancellation;
		int _timeForCompletion;
		bool _isTimeForEntireTest;
		int _questionCount;
		string _testId;
		int _questionNumber;
		int _questionType;
		DateTime _testStarted;
		DateTime _questionStarted;

		public TestPassingPageViewModel(
			IDialogs dialogs, IPages navigation, TestingItemModel testModel, bool fromComplexLearning)
		{
			_fromComplexLearning = fromComplexLearning;
			_testModel = testModel;
			_dialogs = dialogs;
			_navigation = navigation;
			IsTestForSelfStudy = testModel.ForSelfStudy;
			Task.Run(async () => await getData(1));
		}

		bool _isLoading;
		public bool IsLoading {
			get { return _isLoading; }
			set { SetProperty(ref _isLoading, value); }
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
			var test = await DataAccess.GetTest(_testModel.Id);

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
				_testModel.Id, number, AppUserData.UserId);

			if (question.IsError) {
				await _dialogs.ShowError(question.ErrorMessage);
				await _navigation.ClosePage(true);
				return new TestQuestionCommonModel();
			}

			return question;
		}

		async Task answerQuestion()
		{
			setLoading(true);

			TestingCommonAnswerPostModel commonPostModel = null;

			switch (_questionType) {
				case 0:
				case 1:
					commonPostModel = getSelectedAnswer();
					break;
				case 2:
					commonPostModel = getEditableAnswer();
					break;
				case 3:
					commonPostModel = getMovableAnswer();
					break;
			}

			await answerCommonQuestion(commonPostModel);

			setLoading(false);
		}

		TestingCommonAnswerPostModel getSelectedAnswer()
		{
			var answersList = Answers?.Select(
				a => a.IsSelected ? new TestingAnswerPostModel(a.Id, 1) : null);
			return composeAnswer(answersList.ToList());
		}

		TestingCommonAnswerPostModel getMovableAnswer()
		{
			var answersList = Answers?.Select(
				(a, index) => new TestingAnswerPostModel(Answers[index].Id, index));
			return composeAnswer(answersList.ToList());
		}

		TestingCommonAnswerPostModel getEditableAnswer()
		{
			var answersList = Answers?.Select(
				a => new TestingAnswerPostModel(Answers[0].Id, Answers[0].ContentToAnswer));
			return composeAnswer(answersList.ToList());
		}

		TestingCommonAnswerPostModel composeAnswer(List<TestingAnswerPostModel> answersList)
		{
			return new TestingCommonAnswerPostModel {
				Answers = answersList,
				QuestionNumber = _questionNumber,
				TestId = _testId,
				UserId = AppUserData.UserId
			};
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

			await getQuestion(getNextQuestion());
		}

		void setTestData(TestDetailsModel test)
		{
			_isTimeForEntireTest = test.SetTimeForAllTest;
			_timeForCompletion = test.TimeForCompleting;
			_questionCount = test.CountOfQuestions;
			_testId = test.Id.ToString();
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

		void setTimer()
		{
			if (_timeForCompletion == 0)
				return;

			if (_isTimeForEntireTest) {
				setTimerForEntireTest();
			} else {
				setTimerForQuestion();
			}
		}

		void setTimerForEntireTest()
		{
			_testStarted = DateTime.Now;

			Device.StartTimer(TimeSpan.FromSeconds(1), () => {
				if (checkTimerCancellation()) {
					return false;
				}

				var timePassed = DateHelper.CheckDatesDifference(_testStarted, DateTime.Now);
				var timeLeft = new TimeSpan(0, _timeForCompletion, 0).Subtract(timePassed);
				setTitle(timeLeft);

				if (timePassed.TotalMinutes >= _timeForCompletion) {
					completeTest();
					return false;
				}

				return true;
			});
		}

		void setTimerForQuestion()
		{
			_questionStarted = DateTime.Now;

			Device.StartTimer(TimeSpan.FromSeconds(1), () => {
				if (checkTimerCancellation()) {
					return false;
				}

				var timePassed = DateHelper.CheckDatesDifference(_questionStarted, DateTime.Now);
				var timeLeft = new TimeSpan(0, _timeForCompletion, 0).Subtract(timePassed);
				setTitle(timeLeft);

				if (timePassed.TotalSeconds >= _timeForCompletion) {
					completeQuestion();
					return false;
				}

				return true;
			});
		}

		bool checkTimerCancellation()
		{
			if (_timerCancellation) {
				_timerCancellation = false;
				return true;
			}

			return false;
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

		void moveAnswer(object obj, bool down)
		{
			if (obj == null || obj.GetType() != typeof(int)) {
				return;
			}

			var id = (int)obj;
			var answers = Answers;
			var answer = answers.SingleOrDefault(a => a.Id == id);

			if (answer == null) {
				return;
			}

			var index = answers.IndexOf(answer);

			if (down) {
				answers.Swap(
					index,
					index == answers.Count - 1 ? 0 : index + 1);
			} else {
				answers.Swap(
					index == 0 ? 0 : index,
					index == 0 ? answers.Count - 1 : index - 1);
			}

			Answers = new List<TestPassingAnswerModel>(answers);
		}

		protected async Task ExecuteSkipCommand()
		{
			setLoading(true);
			await getQuestion(getNextQuestion());
			setLoading(false);
		}

		int getNextQuestion()
		{
			return _questionNumber + 1 <= _questionCount ? _questionNumber + 1 : 1;
		}

		void setLoading(bool loading)
		{
			IsLoading = loading;
			IsNotLoading = !loading;
		}
	}
}
