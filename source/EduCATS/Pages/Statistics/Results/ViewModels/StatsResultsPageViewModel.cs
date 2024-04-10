﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.LabSchedule;
using EduCATS.Pages.Statistics.Enums;
using EduCATS.Pages.Statistics.Results.Models;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Results.ViewModels
{
	public class StatsResultsPageViewModel : ViewModel
	{
		readonly int _currentSubjectId;
		readonly int _currentGroupId;
		readonly string _currentUserLogin;
		readonly string _currentUserName;
		readonly StatsPageEnum _statisticsPage;
		readonly IPlatformServices _services;

		List<StatsPageLabsVisitingModel> _currentLabsVisitingList;
		List<StatsPageLabsRatingModel> _currentLabsMarksList;
		List<StatsPageLabsRatingModel> _currentPractMarksList;
		List<StatsPageLabsVisitingModel> _currentPractVisitingList;
		List<TakedLab> _takedLabs;

		const string _emptyRatingString = "-";
		const string _doubleStringFormat = "0.0";

		public StatsResultsPageViewModel(
			IPlatformServices services, string userLogin,
			int subjectId, int groupId, StatsPageEnum statisticsPage, string name)
		{
			_services = services;
			_currentUserLogin = userLogin;
			_currentSubjectId = subjectId;
			_currentGroupId = groupId;
			_statisticsPage = statisticsPage;
			_currentUserName = name;
			_services.Device.MainThread(async () => {
				_services.Dialogs.ShowLoading();
				await getData();
				_services.Dialogs.HideLoading();
			});
		}

		List<StatsResultsPageModel> _marks;
		public List<StatsResultsPageModel> Marks
		{
			get { return _marks; }
			set { SetProperty(ref _marks, value); }
		}

		bool _isLoading;
		public bool IsLoading
		{
			get { return _isLoading; }
			set { SetProperty(ref _isLoading, value); }
		}

		string _summary;
		public string Summary
		{
			get { return _summary; }
			set { SetProperty(ref _summary, value); }
		}

		Command refreshCommand;
		public Command RefreshCommand
		{
			get
			{
				return refreshCommand ?? (
					refreshCommand = new Command(async () => await executeRefreshCommand()));
			}
		}

		private async Task executeRefreshCommand()
		{
			//IsLoading = true;
			_services.Device.MainThread(() => _services.Dialogs.ShowLoading());
			await getData();
			_services.Device.MainThread(() => _services.Dialogs.HideLoading());
			IsLoading = false;
		}

		async Task getData()
		{
			try
			{
				switch (_statisticsPage)
				{
					case StatsPageEnum.LabsRating:
						await getLabs(false);
						await getLabsMarksAndVisiting();
						break;
					case StatsPageEnum.LabsVisiting:
						await getLabs(true);
						await getLabsMarksAndVisiting();
						break;
					case StatsPageEnum.PractiseMarks:
						await getPracticials(false);
						await getPractMarkAndVisiting();
						break;
					case StatsPageEnum.PractiseVisiting:
						await getPracticials(true);
						await getPractMarkAndVisiting();
						break;
					case StatsPageEnum.LecturesVisiting:
						await getLecturesVisiting();
						break;
				}

				calculateSummary();

				if (DataAccess.IsError)
				{
					_services.Device.MainThread(
						() => _services.Dialogs.ShowError(DataAccess.ErrorMessage));
				}
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}

		async Task getLabsMarksAndVisiting()
		{
			StatsModel stats = new StatsModel();
			LabsVisitingList statsTest = new LabsVisitingList();
			if (Servers.Current == Servers.EduCatsAddress)
			{
				statsTest = await DataAccess.GetTestStatistics(_currentSubjectId, _currentGroupId);
			}
			else
			{
				stats = await DataAccess.GetStatistics(_currentSubjectId, _currentGroupId);
			}

			var student = stats?.Students?.SingleOrDefault(
				s => string.Compare(s.Name?.ToLower(), _currentUserName?.ToLower()) == 0);
			var studentTest = statsTest?.Students?.SingleOrDefault(
				s => string.Compare(s.FullName?.ToLower(), _currentUserName?.ToLower()) == 0);

			if (_statisticsPage == StatsPageEnum.LabsRating)
			{
				setMarks(student, studentTest);
			}
			else
			{
				setLabsVisiting(student, studentTest);
			}
		}

		async Task getPractMarkAndVisiting()
		{
			StatsModel stats = new StatsModel();
			LabsVisitingList statsTest = new LabsVisitingList();
			statsTest = await DataAccess.GetTestPracticialStatistics(_currentSubjectId, _currentGroupId);
			stats = await DataAccess.GetStatistics(_currentSubjectId, _currentGroupId);
			var student = stats?.Students?.SingleOrDefault(
				s => string.Compare(s.Name?.ToLower(), _currentUserName?.ToLower()) == 0);
			var studentTest = statsTest?.Students?.SingleOrDefault(
				s => string.Compare(s.FullName?.ToLower(), _currentUserName?.ToLower()) == 0);
			if (_statisticsPage == StatsPageEnum.LabsRating)
			{
				setMarks(student, studentTest);
			}
			else if (_statisticsPage == StatsPageEnum.LabsVisiting)
			{
				setLabsVisiting(student, studentTest);
			}
			else if (_statisticsPage == StatsPageEnum.PractiseMarks)
			{
				setMarks(student, studentTest);
			}
			else
			{
				setLabsVisiting(student, studentTest);
			}
		}

		async Task getPracticials(bool isVisiting)
		{
			var dataTestPract = await DataAccess.GetPractTest(_currentSubjectId, _currentGroupId);
			if (isVisiting)
			{
				setVisitingPractStatistics(dataTestPract);
			}
			else
			{
				setRatingPractStatistics(dataTestPract);
			}
		}

		void setRatingPractStatistics(TakedLabs dataTestPract)
		{
			var marksTestPractList = dataTestPract.Practicals?.Select(
					pract => new StatsPageLabsRatingModel(
						pract.PracticalId, pract.ShortName, pract.Theme, pract.SubGroup));
			_currentPractMarksList = new List<StatsPageLabsRatingModel>(marksTestPractList);
		}

		void setVisitingPractStatistics(TakedLabs dataTestPract)
		{
			var practVisitingStatus = dataTestPract.ScheduleProtectionPracticals?.Select(
					pract => new StatsPageLabsVisitingModel(
						pract.ScheduleProtectionPracticalId, pract.Date));
			_currentPractVisitingList = new List<StatsPageLabsVisitingModel>(practVisitingStatus);
		}

		async Task getLabs(bool isVisiting)
		{
			var dataLabs = new LabsModel();
			var dataTestLabs = new TakedLabs();
			if (Servers.Current == Servers.EduCatsAddress)
			{
				dataTestLabs = await DataAccess.GetLabsTest(_currentSubjectId, _currentGroupId);
			}
			else
			{
				dataLabs = await DataAccess.GetLabs(_currentSubjectId, _currentGroupId);
			}

			if (dataTestLabs == null)
			{
				return;
			}

			if (dataLabs == null)
			{
				return;
			}

			if (isVisiting)
			{
				setVisitingLabsStatistics(dataLabs, dataTestLabs);
			}
			else
			{
				setRatingLabsStatistics(dataLabs, dataTestLabs);
			}
		}

		void setVisitingLabsStatistics(LabsModel dataLabs, TakedLabs takedLabs)
		{
			_takedLabs = takedLabs.Labs;

			var labsTetsVisitingStatus = takedLabs.ScheduleProtectionLabs?.Select(labs =>
				new StatsPageLabsVisitingModel(labs.ScheduleProtectionLabId, labs.Date));

			var labsVisitingStatus = dataLabs.ProtectionLabs?.Select(
					labs => new StatsPageLabsVisitingModel(
						labs.ProtectionLabId, labs.Date));

			var practVisitingStatus = takedLabs.ScheduleProtectionPracticals?.Select(
				pract => new StatsPageLabsVisitingModel(
					pract.ScheduleProtectionPracticalId, pract.Date));


			if (_statisticsPage == StatsPageEnum.LabsVisiting)
			{
				if (Servers.Current == Servers.EduCatsAddress)
				{
					_currentLabsVisitingList = new List<StatsPageLabsVisitingModel>(labsTetsVisitingStatus);
				}
				else
				{
					_currentLabsVisitingList = new List<StatsPageLabsVisitingModel>(labsVisitingStatus);
				}
			}
			else
			{
				_currentPractVisitingList = new List<StatsPageLabsVisitingModel>(practVisitingStatus);
			}
		}

		void setRatingLabsStatistics(LabsModel dataLabs, TakedLabs takedLabs)
		{
			var marksTestLabsList = takedLabs.Labs?.Select(
					labs => new StatsPageLabsRatingModel(
						labs.LabId, labs.ShortName, labs.Theme, labs.SubGroup));
			var marksLabsList = dataLabs.Labs?.Select(
					labs => new StatsPageLabsRatingModel(
						labs.LabId, labs.ShortName, labs.Theme, labs.SubGroup));
			if (Servers.Current == Servers.EduCatsAddress)
			{
				_currentLabsMarksList = new List<StatsPageLabsRatingModel>(marksTestLabsList);
			}
			else
			{
				_currentLabsMarksList = new List<StatsPageLabsRatingModel>(marksLabsList);
			}

		}

		async Task getLecturesVisiting()
		{
			LecturesModel visitingData = new LecturesModel();

			var listLectures = await DataAccess.GetInfoLectures(_currentSubjectId);

			Queue<string> queueTheme = new Queue<string>();

			foreach (var lecture in listLectures.Lectures)
			{
				for (int i = 0; i < lecture.Duration / 2; i++)
				{
					queueTheme.Enqueue(lecture.Theme);
				}
			}

			if (Servers.Current == Servers.EduCatsAddress)
			{
				visitingData = await DataAccess.GetLecturesTest(_currentSubjectId, _currentGroupId);
			}
			else
			{
				visitingData = await DataAccess.GetLectures(_currentSubjectId, _currentGroupId);
			}

			var groupVisiting = visitingData?.GroupsVisiting?[0];

			var userLecturesVisiting = groupVisiting?.LecturesVisiting
				.SingleOrDefault(v => string.Compare(v.StudentName?.ToLower(), _currentUserName?.ToLower()) == 0);

			var stats = userLecturesVisiting?.VisitingList?.Select(
				u =>
				{
					string theme = null;
					if (queueTheme.Count > 0)
					{
						theme = queueTheme.Dequeue();
					}
					return new StatsResultsPageModel(
					 theme, u.Date, u.Comment, string.IsNullOrEmpty(u.Mark) ? _emptyRatingString : u.Mark);
				});

			var statsList = stats.ToList();

			if (statsList == null)
			{
				return;
			}

			Marks = new List<StatsResultsPageModel>(statsList);
		}

		void setMarks(StatsStudentModel student, LaboratoryWorksModel labs)
		{
			if (_statisticsPage == StatsPageEnum.LabsRating)
			{
				if (Servers.Current == Servers.EduCatsAddress)
				{
					List<StatsResultsPageModel> marksTestResults = new List<StatsResultsPageModel>();

					foreach (var l in _currentLabsMarksList)
					{
						var lab = labs.LabsMarks?.FirstOrDefault(m => l.LabId == m.LabId && l.SubGroup == labs.SubGroup);
						if (lab != null)
						{
							var labTitle = lab == null ? null : $"{l.ShortName}. {l.Theme}";
							var result = string.IsNullOrEmpty(lab.Mark) ? _emptyRatingString : lab.Mark;
							marksTestResults.Add(new StatsResultsPageModel(labTitle, lab.Date, setCommentByRole(lab.Comment, lab.ShowForStudent), result));
						}
					}

					Marks = new List<StatsResultsPageModel>(marksTestResults);
				}
				else
				{
					var marksResults = _currentLabsMarksList?.Select(l =>
					{
						var lab = student.MarkList?.FirstOrDefault(m => l.LabId == m.LabId);
						var labTitle = lab == null ? null : $"{l.ShortName}. {l.Theme}";
						var result = string.IsNullOrEmpty(lab.Mark) ? _emptyRatingString : lab.Mark;
						return new StatsResultsPageModel(labTitle, lab.Date, setCommentByRole(lab.Comment, true), result);
					});

					Marks = new List<StatsResultsPageModel>(marksResults);
				}
			}
			else
			{
				var practMarkResults = _currentPractMarksList?.Select(l =>
				{
					var lab = labs.PracticalsMarks?.FirstOrDefault(m => l.LabId == m.PracticalId);
					var labTitle = lab == null ? null : $"{l.ShortName}. {l.Theme}";
					var result = string.IsNullOrEmpty(lab.Mark) ? _emptyRatingString : lab.Mark;
					return new StatsResultsPageModel(labTitle, lab.Date, setCommentByRole(lab.Comment, lab.ShowForStudent), result);
				});

				Marks = new List<StatsResultsPageModel>(practMarkResults);
			}
		}

		void setLabsVisiting(StatsStudentModel student, LaboratoryWorksModel labs)
		{
			if (_statisticsPage == StatsPageEnum.LabsVisiting)
			{
				if (Servers.Current == Servers.EduCatsAddress)
				{
					Queue<Theme> queueTheme = new Queue<Theme>();

					foreach (var lecture in _takedLabs.FindAll(l => l.SubGroup == labs.SubGroup))
					{
						for (int i = 0; i < lecture.Duration / 2; i++)
						{
							queueTheme.Enqueue(new Theme(lecture.ShortName, lecture.Theme));
						}
					}

					var labsVisitingList = _currentLabsVisitingList.OrderBy(vis => DateTime.Parse(vis.Date)).ToList();

					List<StatsResultsPageModel> temp = new List<StatsResultsPageModel>();

					foreach (var vis in labsVisitingList)
					{
						var lab = labs.LabVisitingMark.FirstOrDefault(l => l.ScheduleProtectionLabId == vis.ProtectionLabId);

						if (lab != null)
						{
							Theme theme = null;
							if (queueTheme.Count > 0)
							{
								theme = queueTheme.Dequeue();
							}

							var labTitle = lab == null ? null : $"{theme.ShortName}. {theme.LabTheme}";
							var result = string.IsNullOrEmpty(lab.Mark) ? _emptyRatingString : lab.Mark;

							temp.Add(new StatsResultsPageModel(labTitle, vis?.Date, setCommentByRole(lab.Comment, lab.ShowForStudent), result));
						}
					}
;
					Marks = new List<StatsResultsPageModel>(temp);
				}
				else
				{
					var visitingLabsResult = student.VisitingList.Select(v =>
					{
						var lab = _currentLabsVisitingList.FirstOrDefault(
							l => l.ProtectionLabId == v.ScheduleProtectionLabId);
						var result = string.IsNullOrEmpty(v.Mark) ? _emptyRatingString : v.Mark;
						return new StatsResultsPageModel(null, lab?.Date, setCommentByRole(v.Comment, true), result);
					});
					Marks = new List<StatsResultsPageModel>(visitingLabsResult);
				}
			}
			else
			{
				var practVisitingTestResult = labs.PracticalVisitingMark.Select(v =>
				{
					var lab = _currentPractVisitingList.FirstOrDefault(
						l => l.ProtectionLabId == v.ScheduleProtectionPracticalId);
					var result = string.IsNullOrEmpty(v.Mark) ? _emptyRatingString : v.Mark;
					return new StatsResultsPageModel(null, lab?.Date, setCommentByRole(v.Comment, v.ShowForStudent), result);
				});
				Marks = new List<StatsResultsPageModel>(practVisitingTestResult);
			}

		}


		void calculateSummary()
		{
			if (Marks == null)
			{
				setSummary(_emptyRatingString);
				return;
			}

			var resultCount = 0;
			var resultSummary = 0;
			foreach (var mark in Marks)
			{
				if (!string.IsNullOrEmpty(mark.Result) && !mark.Result.Equals(_emptyRatingString))
				{
					int.TryParse(mark.Result, out int result);
					resultSummary += result;
					resultCount++;
				}
			}

			if (resultCount == 0)
			{
				setSummary(_emptyRatingString);
				return;
			}

			var avgSummary = resultSummary / (double)resultCount;
			setSummary(_statisticsPage == StatsPageEnum.LabsRating || _statisticsPage == StatsPageEnum.PractiseMarks ?
				avgSummary.ToString(_doubleStringFormat, CultureInfo.InvariantCulture) :
				resultSummary.ToString());
		}

		void setSummary(string summary)
		{
			_services.Device.MainThread(() => Summary = summary);
		}

		string setCommentByRole(string comment, bool show)
		{
			return show ? comment : null;
		}
	}
}
