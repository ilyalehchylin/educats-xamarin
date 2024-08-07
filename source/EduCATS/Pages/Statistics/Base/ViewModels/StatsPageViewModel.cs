﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Extensions;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.LabSchedule;
using EduCATS.Networking.Models.SaveMarks.Practicals;
using EduCATS.Pages.Pickers;
using EduCATS.Pages.SaveMarks;
using EduCATS.Pages.Statistics.Base.Models;
using EduCATS.Pages.Statistics.Enums;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Base.ViewModels
{
	public class StatsPageViewModel : SubjectsViewModel
	{
		const string _doubleStringFormat = "0.0";
		private List<StatsStudentModel> _students;
		private LabsVisitingList _studentsTest;
		private IPlatformServices _service;

		public StatsPageViewModel(IPlatformServices services) : base(services)
		{
			_service = services;
		}

		public void Init()
		{
			setCollapsedDetails();

			_service.Device.MainThread(async () => {
				_service.Device.MainThread(() => _service.Dialogs.ShowLoading());
				checkStudent();
				await SetupSubjects();
				await getAndSetStatistics();
				await setButtonsList();
				_service.Device.MainThread(() => _service.Dialogs.HideLoading());
				IsLoading = false;
			});

			SubjectChanged += async (id, name) => {
				_service.Dialogs.ShowLoading();
				checkStudent();
				await setButtonsList();
				await getAndSetStatistics();
				_service.Dialogs.HideLoading();
			};

		}

		bool _isLoading;
		public bool IsLoading {
			get { return _isLoading; }
			set { SetProperty(ref _isLoading, value); }
		}

		bool _isExpandedStatistics;
		public bool IsExpandedStatistics {
			get { return _isExpandedStatistics; }
			set { SetProperty(ref _isExpandedStatistics, value); }
		}

		bool _isCollapsedStatistics;
		public bool IsCollapsedStatistics {
			get { return _isCollapsedStatistics; }
			set { SetProperty(ref _isCollapsedStatistics, value); }
		}

		bool _isNotEnoughDetails;
		public bool IsNotEnoughDetails {
			get { return _isNotEnoughDetails; }
			set { SetProperty(ref _isNotEnoughDetails, value); }
		}

		bool _isEnoughDetails;
		public bool IsEnoughDetails {
			get { return _isEnoughDetails; }
			set { SetProperty(ref _isEnoughDetails, value); }
		}

		bool _isStudent;
		public bool IsStudent {
			get { return _isStudent; }
			set { SetProperty(ref _isStudent, value); }
		}

		string _averageLabs;
		public string AverageLabs {
			get { return _averageLabs; }
			set { SetProperty(ref _averageLabs, value); }
		}

		string _averagePract;
		public string AveragePract
		{
			get { return _averagePract; }
			set { SetProperty(ref _averagePract, value); }
		}

		string _averageTests;
		public string AverageTests {
			get { return _averageTests; }
			set { SetProperty(ref _averageTests, value); }
		}

		string _rating;
		public string Rating {
			get { return _rating; }
			set { SetProperty(ref _rating, value); }
		}

		List<double> _chartEntries;
		public List<double> ChartEntries {
			get { return _chartEntries; }
			set { SetProperty(ref _chartEntries, value); }
		}

		List<StatsPageModel> _pagesList;
		public List<StatsPageModel> PagesList {
			get { return _pagesList; }
			set { SetProperty(ref _pagesList, value); }
		}

		object _selectedItem;
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);

				if (_selectedItem != null) {
					openPage(_selectedItem);
				}
			}
		}

		bool _isLabs;
		public bool IsLabs
		{
			get { return _isLabs; }
			set
			{
				SetProperty(ref _isLabs, value);
			}
		}

		bool _isPract;
		public bool IsPract
		{
			get { return _isPract; }
			set
			{
				SetProperty(ref _isPract, value);
			}
		}

		Command _refreshCommand;
		public Command RefreshCommand {
			get {
				return _refreshCommand ?? (_refreshCommand = new Command(
					async () => await executeRefreshCommand()));
			}
		}

		Command _expandCommand;
		public Command ExpandCommand {
			get {
				return _expandCommand ?? (_expandCommand = new Command(executeExpandCommand));
			}
		}

		public async Task setButtonsList()
		{
			IsPract = IsLabs = false;

			if (CurrentSubject == null) {
				setPagesList();
				return;
			}

			var dataPract = await DataAccess.GetPracticals(CurrentSubject.Id);
			if (dataPract.Practicals.Count != 0) {
				IsPract = true;
			}

			var dataLabs = await DataAccess.GetLabs(CurrentSubject.Id);
			if (dataLabs.Labs.Count != 0) {
				IsLabs = true;
			}

			setPagesList();
		}

		public void setPagesList()
		{
			if (_isPract && _isLabs)
			{
				PagesList = new List<StatsPageModel> {
				getPage("stats_page_lectures_visiting"),
				getPage("practiсe_visiting"),
				getPage("practice_mark"),
				getPage("stats_page_labs_visiting"),
				getPage("stats_page_labs_rating"),
				};
			}
			else if (_isPract)
			{
				PagesList = new List<StatsPageModel> {
				getPage("stats_page_lectures_visiting"),
				getPage("practiсe_visiting"),
				getPage("practice_mark"),
				};
			}
			else if (_isLabs)
			{
				PagesList = new List<StatsPageModel> {
				getPage("stats_page_lectures_visiting"),
				getPage("stats_page_labs_visiting"),
				getPage("stats_page_labs_rating"),
				};
			}
			else
			{
				PagesList = new List<StatsPageModel> {
				getPage("stats_page_lectures_visiting") };
			}
		}

		public void setCollapsedDetails(bool isCollapsed = true)
		{
			IsCollapsedStatistics = isCollapsed;
			IsExpandedStatistics = !isCollapsed;
		}

		protected StatsPageEnum getPageToOpen(string pageString)
		{
			var labsRatingString = CrossLocalization.Translate("stats_page_labs_rating");
			var labsVisitingString = CrossLocalization.Translate("stats_page_labs_visiting");
			var practiseVisitingString = CrossLocalization.Translate("practiсe_visiting");
			var practiseRatingString = CrossLocalization.Translate("practice_mark");

			if (pageString.Equals(labsRatingString))
			{
				return StatsPageEnum.LabsRating;
			}
			else if (pageString.Equals(labsVisitingString))
			{
				return StatsPageEnum.LabsVisiting;
			}
			else if (pageString.Equals(practiseVisitingString))
			{
				return StatsPageEnum.PractiseVisiting;
			}
			else if (pageString.Equals(practiseRatingString))
			{
				return StatsPageEnum.PractiseMarks;
			}
			else
			{
				return StatsPageEnum.LecturesVisiting;
			}
		}


		protected virtual async Task executeRefreshCommand()
		{
			try {
				PlatformServices.Device.MainThread(() => PlatformServices.Dialogs.ShowLoading());
				checkStudent();
				await SetupSubjects();
				await getAndSetStatistics();
				PlatformServices.Device.MainThread(() => PlatformServices.Dialogs.HideLoading());
				PlatformServices.Device.MainThread(() => IsLoading = false);
			}
			catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		protected virtual void openPage(object selectedObject)
		{
			try
			{
				if (selectedObject == null || selectedObject.GetType() != typeof(StatsPageModel))
				{
					return;
				}

				var page = selectedObject as StatsPageModel;
				var pageType = getPageToOpen(page.Title);

				if (AppUserData.UserType == UserTypeEnum.Professor)
				{
					PlatformServices.Navigation.OpenStudentsListStats(
						(int)pageType, CurrentSubject.Id, _students, page.Title);
					return;
				}
				else
				{
					var user = _students.SingleOrDefault(s => s.StudentId == PlatformServices.Preferences.UserId);
					if (user == null)
					{
						return;
					}
					PlatformServices.Navigation.OpenDetailedStatistics(
					user.Login, CurrentSubject.Id, PlatformServices.Preferences.GroupId, (int)pageType, page.Title, user.Name);
				}
				
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		protected virtual async Task<List<StatsStudentModel>> getStatistics()
		{
			try
			{
				if (!AppUserData.IsProfileLoaded)
				{
					await getProfile();
				}

				var groupId = PlatformServices.Preferences.GroupId;

				if (CurrentSubject == null || groupId == -1)
				{
					return null;
				}

				var statisticsModel = await DataAccess.GetStatistics(
					CurrentSubject.Id, PlatformServices.Preferences.GroupId);

				if (DataAccess.IsError && !DataAccess.IsConnectionError)
				{
					PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage);
				}

				return statisticsModel.Students?.ToList();
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
				return null;
			}
		}

		protected virtual async Task getAndSetStatistics()
		{
			try
			{
				if (CurrentSubject != null)
				{
					if (_isStudent)
					{
						var studentsStatistics = await getStatistics();
						_students = studentsStatistics;
						var currentPractStudentStatistics = new LaboratoryWorksModel();
						var studentsPractStatistics = await DataAccess.GetTestPracticialStatistics(CurrentSubject.Id, PlatformServices.Preferences.GroupId);
						if (Servers.Current == Servers.EduCatsAddress)
						{
							var currentStudentStatistics = new StatsStudentModel();
							var studentTestStatistics = await DataAccess.GetTestStatistics(CurrentSubject.Id, PlatformServices.Preferences.GroupId);
							var currentTestStudentStatistics = studentTestStatistics.Students.SingleOrDefault(
							s => s.StudentId == PlatformServices.Preferences.UserId);
							currentPractStudentStatistics = studentsPractStatistics.Students.SingleOrDefault(
								s => s.StudentId == PlatformServices.Preferences.UserId);
							setChartData(currentStudentStatistics, studentTestStatistics, studentsPractStatistics);
							_studentsTest = studentTestStatistics;
						}
						else
						{
							var currentStudentStatistics = new StatsStudentModel();
							studentsStatistics = await getStatistics();
							currentStudentStatistics = studentsStatistics.SingleOrDefault(
							s => s.StudentId == PlatformServices.Preferences.UserId);
							setChartData(currentStudentStatistics, new LabsVisitingList(), studentsPractStatistics);
							_students = studentsStatistics;
						}
					}
					else
					{
						var studentsStatistics = await getStatistics();

						var groupsModel = new GroupsViewModel(new PlatformServices(), CurrentSubject.Id);

						await groupsModel.SetupGroups();

						var currentStudentStatistics = new StatsStudentModel();

						List<double> averages = new();
						averages.Add(0);
						averages.Add(0);
						averages.Add(0);

						List<double> avForGroup;

						int countOfGroups = 0;

						foreach (var group in (groupsModel.CurrentGroups))
						{
							countOfGroups++;
							var studentsPractStatistics = await DataAccess.GetTestPracticialStatistics(CurrentSubject.Id, group.GroupId);

							if (Servers.Current == Servers.EduCatsAddress)
							{
								var studentTestStatistics = await DataAccess.GetTestStatistics(CurrentSubject.Id, group.GroupId);
								avForGroup = calculateChartData(currentStudentStatistics, studentTestStatistics, studentsPractStatistics, new(), new());
								averages[0] += avForGroup[0];
								averages[1] += avForGroup[1];
								averages[2] += avForGroup[2];
							}
							else
							{								
								studentsStatistics = await getStatistics();
								avForGroup = calculateChartData(currentStudentStatistics, new LabsVisitingList(), studentsPractStatistics, new(), new());
								averages[0] += avForGroup[0];
								averages[1] += avForGroup[1];
								averages[2] += avForGroup[2];
							}
						}

						averages[0] /= countOfGroups == 0? 1 : countOfGroups;
						averages[1] /= countOfGroups == 0? 1 : countOfGroups;
						averages[2] /= countOfGroups == 0 ? 1 : countOfGroups;

						setChartData(currentStudentStatistics, new LabsVisitingList(), null, averages);
					}
				}
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}


		void executeExpandCommand()
		{
			try {
				if (IsCollapsedStatistics) {
					setCollapsedDetails(false);
				} else {
					setCollapsedDetails();
				}
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		void setChartData(StatsStudentModel stats, LabsVisitingList studentsPractStatistics, LabsVisitingList studentTestStatistics, List<double> averages = null)
		{
			try {
				double rating = 0;

				if (stats == null) {
					stats = new StatsStudentModel();
				}


				LaboratoryWorksModel currentPractStudentStatistics = null, worksModel = null;

				if (_isStudent)
				{
					averages = calculateChartData(stats, studentsPractStatistics, studentTestStatistics, currentPractStudentStatistics, worksModel);
				}

				AveragePract = averages[2].ToString(_doubleStringFormat, CultureInfo.InvariantCulture);
				AverageLabs = averages[0].ToString(_doubleStringFormat, CultureInfo.InvariantCulture);
				AverageTests = averages[1].ToString(_doubleStringFormat, CultureInfo.InvariantCulture);

				if (Servers.Current == Servers.EduCatsAddress)
				{

					if (worksModel is not null && worksModel.PracticalsMarks.Count == 0 || worksModel is null && averages[2] == 0)
					{
						rating = (averages[0] + averages[1]) / 2;
						Rating = rating.ToString(_doubleStringFormat, CultureInfo.InvariantCulture);
						ChartEntries = new List<double> {
							averages[0], averages[1], rating, averages[2]
						};
						setNotEnoughDetails(averages[0] == 0 && averages[1] == 0 && rating == 0);
					}
					else
					{
						rating = (averages[0] + averages[1] + averages[2]) / 3;
						Rating = rating.ToString(_doubleStringFormat, CultureInfo.InvariantCulture);
						ChartEntries = new List<double> {
							averages[0], averages[1], rating, averages[2]
						};
						setNotEnoughDetails(averages[0] == 0 && averages[1] == 0 && averages[2] == 0 && rating == 0);
					}
				}
				else
				{
					rating = (averages[0] + averages[1]) / 2;
					Rating = rating.ToString(_doubleStringFormat, CultureInfo.InvariantCulture);
					ChartEntries = new List<double> {
							averages[0], averages[1], rating
						};
					setNotEnoughDetails(averages[0] == 0 && averages[1] == 0 && rating == 0);
				}

			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		List<double> calculateChartData(StatsStudentModel stats, LabsVisitingList studentsPractStatistics, LabsVisitingList studentTestStatistics, LaboratoryWorksModel currentPractStudentStatistics, LaboratoryWorksModel worksModel)
		{
			double avgLabs = 0;
			double avgTests = 0;
			double avgPract = 0;

			List<double> averages = new();

			if (_isStudent)
			{
				currentPractStudentStatistics = studentsPractStatistics.Students.SingleOrDefault(
								s => s.StudentId == PlatformServices.Preferences.UserId);
				worksModel = studentTestStatistics.Students.SingleOrDefault(
					s => s.StudentId == PlatformServices.Preferences.UserId);

				if (Servers.Current == Servers.EduCatsAddress)
				{
					avgPract = calculateAvgPractMarks(worksModel.PracticalsMarks);

					avgLabs = calculateAvgLabsMarkTest(currentPractStudentStatistics.LabsMarks);

					if (currentPractStudentStatistics.TestMark != null)
						avgTests = double.Parse(currentPractStudentStatistics.TestMark, CultureInfo.InvariantCulture);
				}
				else
				{
					avgLabs = calculateAvgLabsMark(stats.MarkList);

					if (stats.AverageTestMark != null)
						avgTests = double.Parse(stats.AverageTestMark, CultureInfo.InvariantCulture);
				}
			}
			else
			{
				if (Servers.Current == Servers.EduCatsAddress)
				{
					int countOfPractStudents = 0;
					foreach (var student in studentTestStatistics.Students)
					{
						avgPract += calculateAvgPractMarks(student.PracticalsMarks);
						countOfPractStudents++;
					}

					avgPract /= countOfPractStudents == 0 ? 1 : countOfPractStudents;

					

					int countOfStudents = 0;
					foreach (var student in studentsPractStatistics.Students)
					{
						avgLabs += calculateAvgLabsMarkTest(student.LabsMarks);
						countOfStudents++;

						if (student.TestMark != null)
							avgTests += double.Parse(student.TestMark, CultureInfo.InvariantCulture);
					}

					avgLabs /= countOfStudents == 0 ? 1 : countOfStudents;

					avgTests /= countOfStudents == 0 ? 1 : countOfStudents;
				}
				else
				{
					avgLabs = calculateAvgLabsMark(stats.MarkList);

					if (stats.AverageTestMark != null)
						avgTests = double.Parse(stats.AverageTestMark, CultureInfo.InvariantCulture);
				}
			}

			averages.Add(avgLabs);
			averages.Add(avgTests);
			averages.Add(avgPract);

			return averages;
		}

		double calculateAvgPractMarks(List<PracticialMarks> practicalsMarks)
		{
			if (practicalsMarks == null)
			{
				return 0;
			}

			var resultCount = 0;
			var resultSummary = 0;
			foreach (var markItem in practicalsMarks)
			{
				var mark = markItem.Mark;
				if (!string.IsNullOrEmpty(mark))
				{
					int.TryParse(mark, out int result);
					resultSummary += result;
					resultCount++;
				}
			}

			if (resultCount == 0)
			{
				return 0;
			}

			return resultSummary / (double)resultCount;
		}



		/// <summary>
		/// Get profile if <see cref="App.getProfileInfo" didn't have time to load./>
		/// </summary>
		/// <returns>Task.</returns>
		async Task getProfile()
		{
			var profile = await DataAccess.GetProfileInfo(PlatformServices.Preferences.UserLogin);
			AppUserData.SetProfileData(PlatformServices, profile);
			IsStudent = AppUserData.UserType == UserTypeEnum.Student;
		}


		StatsPageModel getPage(string text)
		{
			return new StatsPageModel {
				Title = CrossLocalization.Translate(text)
			};
		}


		void setNotEnoughDetails(bool isNotEnough = true)
		{
			IsEnoughDetails = !isNotEnough;
			IsNotEnoughDetails = isNotEnough;
		}

		void checkStudent()
		{
			IsStudent = AppUserData.UserType == UserTypeEnum.Student;
		}

		double calculateAvgLabsMarkTest(List<LabMarks> marks)
		{
			if (marks == null)
			{
				return 0;
			}

			var resultCount = 0;
			var resultSummary = 0;
			foreach (var markItem in marks)
			{
				var mark = markItem.Mark;
				if (!string.IsNullOrEmpty(mark))
				{
					int.TryParse(mark, out int result);
					resultSummary += result;
					resultCount++;
				}
			}

			if (resultCount == 0)
			{
				return 0;
			}

			return resultSummary / (double)resultCount;
		}

		double calculateAvgLabsMark(IList<StatsMarkModel> marks)
		{
			if (marks == null) {
				return 0;
			}

			var resultCount = 0;
			var resultSummary = 0;
			foreach (var markItem in marks) {
				var mark = markItem.Mark;
				if (!string.IsNullOrEmpty(mark)) {
					int.TryParse(mark, out int result);
					resultSummary += result;
					resultCount++;
				}
			}

			if (resultCount == 0) {
				return 0;
			}

			return resultSummary / (double)resultCount;
		}
	
	}
}
