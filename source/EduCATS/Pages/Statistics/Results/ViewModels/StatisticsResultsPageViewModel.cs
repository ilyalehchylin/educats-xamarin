using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.User;
using EduCATS.Pages.Statistics.Enums;
using EduCATS.Pages.Statistics.Results.Models;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Results.ViewModels
{
	public class StatisticsResultsPageViewModel : ViewModel
	{
		int currentSubjectId { get; set; }
		int currentGroupId { get; set; }
		string currentUserLogin { get; set; }
		StatisticsPageEnum statisticsPage { get; set; }

		List<StatisticsResultsPageLabsVisitingModel> currentLabsVisitingList { get; set; }
		List<StatisticsResultsPageLabsMarkModel> currentLabsMarksList { get; set; }

		public StatisticsResultsPageViewModel(
			string userLogin, int subjectId, int groupId, StatisticsPageEnum statisticsPage)
		{
			currentUserLogin = userLogin;
			currentSubjectId = subjectId;
			currentGroupId = groupId;
			this.statisticsPage = statisticsPage;

			Task.Run(async () => await getData());
		}

		List<StatisticsResultsPageModel> marks;
		public List<StatisticsResultsPageModel> Marks {
			get { return marks; }
			set { SetProperty(ref marks, value); }
		}

		bool isLoading;
		public bool IsLoading {
			get { return isLoading; }
			set { SetProperty(ref isLoading, value); }
		}

		Command refreshCommand;
		public Command RefreshCommand {
			get {
				return refreshCommand ?? (
					refreshCommand = new Command(async () => await executeRefreshCommand()));
			}
		}

		private async Task executeRefreshCommand()
		{
			await getData();
		}

		async Task getData()
		{
			IsLoading = true;

			switch (statisticsPage) {
				case StatisticsPageEnum.LabsRating:
					await getLabs(false);
					await getLabsMarksAndVisiting();
					break;
				case StatisticsPageEnum.LabsVisiting:
					await getLabs(true);
					await getLabsMarksAndVisiting();
					break;
				case StatisticsPageEnum.LecturesVisiting:
					await getLecturesVisiting();
					break;
			}

			IsLoading = false;
		}

		async Task getLabs(bool isVisiting)
		{
			var labsModel = await DataAccess.GetLabs(currentSubjectId, currentGroupId);

			if (isVisiting) {
				var visitingLabsList = labsModel.ScheduleProtectionLabs?.Select(
					labs => new StatisticsResultsPageLabsVisitingModel(
						labs.ScheduleProtectionLabId, labs.Date));

				if (visitingLabsList == null) {
					return;
				}

				currentLabsVisitingList = new List<StatisticsResultsPageLabsVisitingModel>(
					visitingLabsList);
			} else {
				var marksLabsList = labsModel.Labs?.Select(
					labs => new StatisticsResultsPageLabsMarkModel(
						labs.LabId, labs.ShortName, labs.Theme));

				if (marksLabsList == null) {
					return;
				}

				currentLabsMarksList = new List<StatisticsResultsPageLabsMarkModel>(
					marksLabsList);
			}
		}

		async Task getLecturesVisiting()
		{
			var lecturesVisitingModel = await DataAccess.GetLectures(currentSubjectId, currentGroupId);

			if (lecturesVisitingModel.GroupsVisiting != null && lecturesVisitingModel.GroupsVisiting.Count > 0) {
				var groupVisiting = lecturesVisitingModel.GroupsVisiting[0];
				if (groupVisiting.LecturesVisiting != null && groupVisiting.LecturesVisiting.Count > 0) {

					var userLecturesVisiting = groupVisiting.LecturesVisiting.SingleOrDefault(
						s => {
							if (!string.IsNullOrEmpty(s.Login)) {
								return s.Login.ToLower() == currentUserLogin.ToLower();
							}
							return false;
						});

					if (userLecturesVisiting != null && userLecturesVisiting.VisitingList != null) {
						var lecturesVisitingResultsList = userLecturesVisiting.VisitingList.Select(s => new StatisticsResultsPageModel {
							Date = s.Date,
							Result = string.IsNullOrEmpty(s.Mark) ? "-" : s.Mark,
						}).ToList();

						if (AppUserData.UserType == UserTypeEnum.Student) {
							var objectsToRemove = lecturesVisitingResultsList.Where(x => x.Result == "-").ToList();
							foreach (var x in objectsToRemove) {
								lecturesVisitingResultsList.Remove(x);
							}
						}

						Device.BeginInvokeOnMainThread(() => Marks = new List<StatisticsResultsPageModel>(lecturesVisitingResultsList));
					}
				}
			}
		}

		async Task getLabsMarksAndVisiting()
		{
			var labsMarkAndVisitingModel = await DataAccess.GetStatistics(currentSubjectId, currentGroupId);

			if (labsMarkAndVisitingModel != null &&
				labsMarkAndVisitingModel.Students != null &&
				labsMarkAndVisitingModel.Students.Count > 0) {

				var userMarksAndVisiting = labsMarkAndVisitingModel.Students.SingleOrDefault(
					s => {
						if (!string.IsNullOrEmpty(s.Login)) {
							return s.Login.ToLower() == currentUserLogin.ToLower();
						}
						return false;
					});

				if (userMarksAndVisiting != null) {
					switch (statisticsPage) {
						case StatisticsPageEnum.LabsRating:

							if (userMarksAndVisiting.MarkList != null &&
								userMarksAndVisiting.MarkList.Count > 0 &&
								currentLabsMarksList != null &&
								currentLabsMarksList.Count > 0) {

								var marksResultsList = new List<StatisticsResultsPageModel>();

								int index = 0;

								foreach (var mark in userMarksAndVisiting.MarkList) {
									var markResult = new StatisticsResultsPageModel();

									var labData = currentLabsMarksList.FirstOrDefault(
										lab => lab.LabId == mark.LabId);

									if (labData != null) {
										markResult.Title = string.Format(
											"{0}. {1}", labData.ShortName, labData.Theme);
									}

									markResult.Comment = mark.Comment;
									markResult.Date = mark.Date;
									markResult.Result = string.IsNullOrEmpty(mark.Mark) ? "-" : mark.Mark;

									if (AppUserData.UserType == UserTypeEnum.Student && markResult.Result == "-") {

									} else {
										marksResultsList.Add(markResult);
									}
								}

								Device.BeginInvokeOnMainThread(() => Marks = new List<StatisticsResultsPageModel>(marksResultsList));

								int count = 0;
								int sum = 0;

								foreach (var mark in Marks) {
									if (mark.Result != "-") {
										count++;
										sum += Convert.ToInt32(mark.Result);
									}
								}
							}

							break;
						case StatisticsPageEnum.LabsVisiting:
							if (userMarksAndVisiting.MarkList != null &&
								userMarksAndVisiting.MarkList.Count > 0 &&
								currentLabsVisitingList != null &&
								currentLabsVisitingList.Count > 0) {

								var marksVisitingResultsList = new List<StatisticsResultsPageModel>();

								int index = 0;

								foreach (var visiting in userMarksAndVisiting.VisitingList) {
									var markResult = new StatisticsResultsPageModel();

									var labData = currentLabsVisitingList.FirstOrDefault(
										lab => lab.ProtectionLabId == visiting.ScheduleProtectionLabId);

									markResult.Comment = visiting.Comment;

									if (labData != null) {
										markResult.Date = labData.Date;
									}

									markResult.Result = string.IsNullOrEmpty(visiting.Mark) ? "-" : visiting.Mark;

									if (AppUserData.UserType == UserTypeEnum.Student && markResult.Result == "-") {

									} else {
										marksVisitingResultsList.Add(markResult);
									}
								}

								Device.BeginInvokeOnMainThread(() => Marks = new List<StatisticsResultsPageModel>(marksVisitingResultsList));
							}

							break;
					}
				}
			}
		}


	}
}
