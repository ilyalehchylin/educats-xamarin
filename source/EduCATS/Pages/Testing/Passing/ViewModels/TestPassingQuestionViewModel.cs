using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Date;
using EduCATS.Helpers.Extensions;
using EduCATS.Helpers.Logs;
using EduCATS.Networking.Models.Testing;
using EduCATS.Pages.Testing.Passing.Models;

namespace EduCATS.Pages.Testing.Passing.ViewModels
{
	public partial class TestPassingPageViewModel
	{
		async Task answerQuestion(bool isAuto = false)
		{
			try {
				setLoading(true);

				TestAnswerPostModel postModel = null;

				switch (_questionType) {
					case 0:
					case 1:
						postModel = getSelectedAnswer();
						break;
					case 2:
						postModel = getEditableAnswer();
						break;
					case 3:
						postModel = getMovableAnswer();
						break;
				}

				await answerQuestion(postModel, isAuto);

				setLoading(false);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		TestAnswerPostModel getSelectedAnswer()
		{
			var answersList = Answers?.Select(
				a => new TestAnswerDetailsPostModel(a.Id, Convert.ToInt32(a.IsSelected)));

			return composeAnswer(answersList.ToList());
		}

		TestAnswerPostModel getMovableAnswer()
		{
			var answersList = Answers?.Select(
				(a, index) => new TestAnswerDetailsPostModel(Answers[index].Id, index));
			return composeAnswer(answersList.ToList());
		}

		TestAnswerPostModel getEditableAnswer()
		{
			var answersList = Answers?.Select(
				a => new TestAnswerDetailsPostModel(Answers[0].Id, Answers[0].ContentToAnswer)).ToList();

			if (_questionType == 2 && answersList.Count > 0 && string.IsNullOrEmpty(answersList[0].Content)) {
				answersList[0].Content = "-";
			}

			return composeAnswer(answersList);
		}

		TestAnswerPostModel composeAnswer(List<TestAnswerDetailsPostModel> answersList)
		{
			return new TestAnswerPostModel {
				Answers = answersList,
				QuestionNumber = _questionNumber,
				TestId = _testIdString,
				UserId = AppUserData.UserId
			};
		}

		void setTestData(TestDetailsModel test)
		{
			try {
				_isTimeForEntireTest = test.SetTimeForAllTest;
				_timeForCompletion = test.TimeForCompleting;
				_questionCount = test.CountOfQuestions;
				_testIdString = test.Id.ToString();
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		void setTimer()
		{
			try {
				if (_timeForCompletion == 0)
					return;

				setTimer(_isTimeForEntireTest);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		void setTimer(bool forTest)
		{
			_started = DateTime.Now;

			_services.Device.SetTimer(TimeSpan.FromSeconds(1), () => {
				if (checkTimerCancellation()) {
					return false;
				}

				var timePassed = DateHelper.CheckDatesDifference(_started, DateTime.Now);
				var timeLeft = _isTimeForEntireTest ?
					new TimeSpan(0, _timeForCompletion, 0).Subtract(timePassed) :
					new TimeSpan(0, 0, _timeForCompletion).Subtract(timePassed);

				_services.Device.MainThread(() => setTitle(timeLeft));

				if (forTest && timePassed.TotalMinutes >= _timeForCompletion) {
					autoAnswerAllQuestions();
					return false;
				} else if (!forTest && timePassed.TotalSeconds >= _timeForCompletion) {
					completeQuestion();
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

		void moveAnswer(object obj, bool down)
		{
			try {
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
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		int getNextQuestion()
		{
			return _questionNumber + 1 <= _questionCount ? _questionNumber + 1 : 1;
		}
	}
}
