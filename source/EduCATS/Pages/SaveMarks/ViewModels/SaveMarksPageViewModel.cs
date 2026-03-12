using EduCATS.Data;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Json;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
using EduCATS.Networking.AppServices;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.LabSchedule;
using EduCATS.Pages.Statistics.Marks.Models;
using Newtonsoft.Json;
using Nyxbull.Plugins.CrossLocalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EduCATS.Pages.SaveMarks.ViewModels
{
	public class SaveMarksPageViewModel : ViewModel
	{
		public readonly IPlatformServices _services;

		public VisitingLecturesList fullVisitingLectures = new VisitingLecturesList();
		public LabsVisitingList fullVisitingLabs = new LabsVisitingList();
		public LabsVisitingList fullVisitingPract = new LabsVisitingList();

		public int selSubGroup { get; set; }

		public VisitingLecturesList groupData = new VisitingLecturesList();
		public LabsVisitingList labsVisitingList = new LabsVisitingList();
		public LabsVisitingList practiceVisitingList = new LabsVisitingList();
		public int subjectId { get; set; }
		public int labVisId = 0;
		public int prVisId = 0;
		public List<VisitingPageModel> _currentLabsVisitingMarksReady = new List<VisitingPageModel>();

		public List<VisitingPageModel> _currentLecturesVisitingMarks;
		public List<VisitingPageModel> _currentLabsVisitingMarksSubGroup1;
		public string _titleOfPage { get; set; }
		public List<string> FullNames { get; set; }

		public TakedLabs _takedLabs { get; set; }

		public SaveMarksPageViewModel(IPlatformServices services, int subjectId, int groupId, string title)
		{
			ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };

			_titleOfPage = title;
			_services = services;
			this.subjectId = subjectId;
			_services.Device.MainThread(async () =>
			{
				try
				{
					_services.Dialogs.ShowLoading();
					await initializeData(subjectId, groupId);
					showDataAccessErrorIfNeeded();
				}
				catch (Exception ex)
				{
					AppLogs.Log(ex);
				}
				finally
				{
					_services.Dialogs.HideLoading();
				}
			});
		}

		async Task initializeData(int subjectId, int groupId)
		{
			if (_titleOfPage == CrossLocalization.Translate("stats_page_lectures_visiting"))
			{
				groupData = await getLecturesVisiting(subjectId, groupId)
					?? new VisitingLecturesList();
				createLecturesVisitingPage(groupData);
			}
			else if (_titleOfPage == CrossLocalization.Translate("stats_page_labs_visiting"))
			{
				labsVisitingList = await DataAccess.GetStatistics(subjectId, groupId)
					?? new LabsVisitingList();
				_takedLabs = await DataAccess.GetLabsTest(subjectId, groupId)
					?? new TakedLabs();
				createLabsVisitingPage(labsVisitingList);
			}
			else if (_titleOfPage == CrossLocalization.Translate("practiсe_visiting"))
			{
				practiceVisitingList = await DataAccess.GetTestPracticialStatistics(subjectId, groupId)
					?? new LabsVisitingList();
				_takedLabs = await DataAccess.GetPractTest(subjectId, groupId)
					?? new TakedLabs();
				createPracticialsVisitingPage(practiceVisitingList);
			}
		}

		async Task<VisitingLecturesList> getLecturesVisiting(int subjectId, int groupId)
		{
			var client = new HttpClient
			{
				Timeout = TimeSpan.FromSeconds(RequestController.RequestTimeoutSeconds)
			};
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_services.Preferences.AccessToken);

			var link = $"{Servers.Current + Links.GetLecturesCalendarData}subjectId={subjectId}&groupId={groupId}";
			var response = await client.GetAsync(link);
			var result = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<VisitingLecturesList>(result);
		}

		void showDataAccessErrorIfNeeded()
		{
			if (DataAccess.IsError && !DataAccess.IsConnectionError)
			{
				_services.Dialogs.ShowError(DataAccess.ErrorMessage);
			}
		}

		private void createPracticialsVisitingPage(LabsVisitingList statistics)
		{
			LecturesMarks = new List<VisitingPageModel>();
			fullVisitingPract = statistics;
			Date = new List<string>();
			FullNames = new List<string>();
			_currentLecturesVisitingMarks = new List<VisitingPageModel>();
			foreach (var pract in _takedLabs.ScheduleProtectionPracticals)
			{
				Date.Add(pract.Date);
			}
			selDateForSave = Date[Date.Count - 1];
			SelectedPracDate = selDateForSave;
		}

		void createLabsVisitingPage(LabsVisitingList statistics)
		{
			LabsVisitingMarks = new List<VisitingPageModel>();
			SubGroup = new List<string>();
			FullNames = new List<string>();
			fullVisitingLabs = statistics;
			foreach (var subGroup in _takedLabs.Labs)
			{
				if (!currentSubGroups.Contains(subGroup.SubGroup))
				{
					currentSubGroups.Add(subGroup.SubGroup);
				}
			}
			foreach (var subGruop in currentSubGroups)
			{
				SubGroup.Add(CrossLocalization.Translate("sub_group") + subGruop.ToString());
			}
			selSubGroup = currentSubGroups[0];
			SelectedSubGroup = SubGroup.FirstOrDefault();
		}

		void createLecturesVisitingPage(VisitingLecturesList statistics)
		{
			_currentLecturesVisitingMarks = new List<VisitingPageModel>();
			LecturesMarks = new List<VisitingPageModel>();
			Date = new List<string>();
			foreach (var group in statistics.GroupsVisiting)
			{
				fullVisitingLectures = statistics;
				foreach (var groupVis in group.LecturesMarksVisiting)
				{
					foreach (var groupVisit in groupVis.Marks)
					{
						if (!Date.Contains(groupVisit.Date))
						{
							Date.Add(groupVisit.Date);
						}
						if (Date[0] == groupVisit.Date &&
							!(_currentLecturesVisitingMarks.Contains(new VisitingPageModel(groupVis.StudentName, groupVisit.Comment,
							groupVisit.Mark, groupVisit.ShowForStudent))))
						{
							_currentLecturesVisitingMarks.Add(new VisitingPageModel(groupVis.StudentName, groupVisit.Comment,
							groupVisit.Mark, groupVisit.ShowForStudent));
						}
					}
				}
			}
			SelectedDate = Date[Date.Count - 1];
			LecturesMarks = _currentLecturesVisitingMarks;
		}

		Command _saveMarksCommand;
		public Command SaveMarksCommand
		{
			get
			{
				return _saveMarksCommand ?? (_saveMarksCommand = new Command(
					async () => await saveMarks()));
			}
		}

		private async Task<KeyValuePair<string, HttpStatusCode>> saveMarks()
		{
			string link = "";
			string body = "";
			if (_titleOfPage == CrossLocalization.Translate("stats_page_lectures_visiting"))
			{
				var lecturesVisiting = fullVisitingLectures.GroupsVisiting[0];
				link = Links.SaveLecturesCalendarData;
				var i = 0;
				foreach (var lect in lecturesVisiting.LecturesMarksVisiting)
				{
					for (int m = 0; m < lect.Marks.Count; m++)
					{
						if (lect.Marks[m].Date == selDateForSave)
						{
							lect.Marks[m].Mark = _currentLecturesVisitingMarks[i].Mark;
							lect.Marks[m].Comment = _currentLecturesVisitingMarks[i].Comment;
							lect.Marks[m].ShowForStudent = _currentLecturesVisitingMarks[i].ShowForStud;
							i++;
						};
					};
				};
				SaveLectures lecturesMarks = new SaveLectures();
				lecturesMarks.lecturesMarks = lecturesVisiting.LecturesMarksVisiting;
				body = JsonController.ConvertObjectToJson(lecturesMarks);
			}
			else if (_titleOfPage == CrossLocalization.Translate("stats_page_labs_visiting"))
			{
				var labsVisiting = fullVisitingLabs.Students;
				SaveLabs labsMarks = new SaveLabs();
				var selectedDate = string.IsNullOrEmpty(selLabDateForSave) ? _selectedLabDate : selLabDateForSave;
				int dateId = 0;
				foreach (var lab in _takedLabs.ScheduleProtectionLabs.Where(v => v.Date == selectedDate && v.SubGroup == selSubGroup))
				{
					dateId = lab.ScheduleProtectionLabId;
				}

				var selectedStudents = labsVisiting.Where(v => v.SubGroup == selSubGroup).ToList();
				var selectedMarks = LabsVisitingMarksSubGroup ?? new List<VisitingPageModel>();
				var countToSave = Math.Min(selectedStudents.Count, selectedMarks.Count);

				for (int i = 0; i < countToSave; i++)
				{
					var currentStudent = selectedStudents[i];
					var currentMark = selectedMarks[i];
					var markForSelectedDate = currentStudent.LabVisitingMark?
						.FirstOrDefault(v => v.ScheduleProtectionLabId == dateId);

					if (markForSelectedDate != null)
					{
						markForSelectedDate.Mark = currentMark.Mark;
						markForSelectedDate.Comment = currentMark.Comment;
						markForSelectedDate.ShowForStudent = currentMark.ShowForStud;
					}

					labsMarks.comments.Add(currentMark.Comment ?? string.Empty);
					labsMarks.Id.Add(markForSelectedDate?.LabVisitingMarkId ?? 0);
					labsMarks.marks.Add(currentMark.Mark ?? string.Empty);
					labsMarks.showForStudents.Add(currentMark.ShowForStud);
					labsMarks.studentsId.Add(currentStudent.StudentId);
					labsMarks.students.Add(currentStudent);
				}

				link = Links.SaveLabsMark;
				labsMarks.dateId = dateId;
				body = JsonController.ConvertObjectToJson(labsMarks);
			}
			else if (_titleOfPage == CrossLocalization.Translate("practiсe_visiting"))
			{
				var dateId = 0;
				SavePracticial savePracticial = new SavePracticial();
				var practVisiting = fullVisitingPract.Students;
				link = Links.SaveStudentPracticalMark;
				foreach (var pr in _takedLabs.ScheduleProtectionPracticals.Where(v => v.Date == selDateForSave))
				{
					dateId = pr.ScheduleProtectionPracticalId;
				}
				int i = 0;
				for (int pract = 0; pract < practVisiting.Count; pract++)
				{
					foreach (var mark in practVisiting[pract].PracticalVisitingMark.Where(v => v.ScheduleProtectionPracticalId == dateId))
					{
						mark.Mark = LecturesMarks[i].Mark;
						mark.Comment = LecturesMarks[i].Comment;
						mark.ShowForStudent = LecturesMarks[i].ShowForStud;
						savePracticial.Id.Add(mark.PracticalVisitingMarkId);
						savePracticial.marks.Add(mark.Mark);
						savePracticial.showForStudents.Add(LecturesMarks[i].ShowForStud);
						savePracticial.studentsId.Add(practVisiting[pract].StudentId);
						savePracticial.Comments.Add(LecturesMarks[i].Comment);
					}
					savePracticial.students.Add(practVisiting[pract]);
					i++;
				}
				savePracticial.dateId = dateId;
				savePracticial.subjectId = subjectId;
				body = JsonController.ConvertObjectToJson(savePracticial);
			}
			var response = await AppServicesController.Request(link, body);
			await _services.Navigation.ClosePage(false);
			return response;
		}

		public List<int> currentSubGroups = new List<int>();

		List<string> _subGroup;
		public List<string> SubGroup
		{
			get { return _subGroup; }
			set { SetProperty(ref _subGroup, value); }
		}

		List<VisitingPageModel> _labsVisitingMarksSubGroup;
		public List<VisitingPageModel> LabsVisitingMarksSubGroup
		{
			get { return _labsVisitingMarksSubGroup; }
			set { SetProperty(ref _labsVisitingMarksSubGroup, value); }
		}

		List<VisitingPageModel> _labsVisitingMarksSubGroupOne;
		public List<VisitingPageModel> LabsVisitingMarksSubGroupOne
		{
			get { return _labsVisitingMarksSubGroupOne; }
			set { SetProperty(ref _labsVisitingMarksSubGroupOne, value); }
		}

		List<ListSaveMarksVisiting> _listSaveMarks;
		public List<ListSaveMarksVisiting> ListSaveMarks
		{
			get { return _listSaveMarks; }
			set { SetProperty(ref _listSaveMarks, value); }
		}

		List<VisitingPageModel> _labsVisitingMarks;
		public List<VisitingPageModel> LabsVisitingMarks
		{
			get { return _labsVisitingMarks; }
			set { SetProperty(ref _labsVisitingMarks, value); }
		}

		List<VisitingPageModel> _lecturesMarks;
		public List<VisitingPageModel> LecturesMarks
		{
			get { return _lecturesMarks; }
			set { SetProperty(ref _lecturesMarks, value); }
		}

		List<string> _datelabs;
		public List<string> DateLabs
		{
			get { return _datelabs; }
			set { SetProperty(ref _datelabs, value); }
		}

		List<string> _date;
		public List<string> Date
		{
			get { return _date; }
			set { SetProperty(ref _date, value); }
		}

		List<string> _date1;
		public List<string> Date1
		{
			get { return _date1; }
			set { SetProperty(ref _date1, value); }
		}

		public string selDateForSave { get; set; }

		string _selectedDate;
		public string SelectedDate
		{
			get
			{
				_currentLecturesVisitingMarks = new List<VisitingPageModel>();
				foreach (var group in fullVisitingLectures.GroupsVisiting)
				{
					foreach (var groupVis in group.LecturesMarksVisiting)
					{
						foreach (var groupVisit in groupVis.Marks)
						{
							if (_selectedDate == groupVisit.Date &&
								!(_currentLecturesVisitingMarks.Contains(new VisitingPageModel(groupVis.StudentName, groupVisit.Comment,
								groupVisit.Mark, groupVisit.ShowForStudent))))
							{
								_currentLecturesVisitingMarks.Add(new VisitingPageModel(groupVis.StudentName, groupVisit.Comment,
								groupVisit.Mark, groupVisit.ShowForStudent));
							}
						}
					}
				}
				selDateForSave = _selectedDate;
				LecturesMarks = _currentLecturesVisitingMarks;
				return _selectedDate;
			}
			set
			{
				SetProperty(ref _selectedDate, value);
			}
		}

		string _selectedPracDate;
		public string SelectedPracDate
		{
			get
			{
				_currentLecturesVisitingMarks = new List<VisitingPageModel>();
				foreach (var pract in _takedLabs.ScheduleProtectionPracticals.Where(v => v.Date == selDateForSave))
				{
					prVisId = pract.ScheduleProtectionPracticalId;
					selDateForSave = _selectedPracDate;
				}
				foreach (var group in fullVisitingPract.Students)
				{
					foreach (var stud in group.PracticalVisitingMark.Where(v => v.ScheduleProtectionPracticalId == prVisId))
					{
						_currentLecturesVisitingMarks.Add(new VisitingPageModel(
							group.FullName, stud.Comment, stud.Mark, stud.ShowForStudent));
					}
				}
				LecturesMarks = _currentLecturesVisitingMarks;
				return _selectedPracDate;
			}
			set
			{
				SetProperty(ref _selectedPracDate, value);
			}
		}

		public string selLabDateForSave { get; set; }
		string _selectedLabDate;
		public string SelectedLabDate
		{
			get
			{
				return _selectedLabDate;
			}
			set
			{
				if (SetProperty(ref _selectedLabDate, value))
				{
					updateLabsVisitingForSelectedDate();
				}
			}
		}

		void updateLabsVisitingForSelectedDate()
		{
			_currentLabsVisitingMarksSubGroup1 = new List<VisitingPageModel>();
			selLabDateForSave = _selectedLabDate;

			if (string.IsNullOrEmpty(_selectedLabDate) || _takedLabs?.ScheduleProtectionLabs == null)
			{
				LabsVisitingMarksSubGroup = _currentLabsVisitingMarksSubGroup1;
				return;
			}

			var selectedSchedule = _takedLabs.ScheduleProtectionLabs
				.FirstOrDefault(v => v.Date == _selectedLabDate && v.SubGroup == selSubGroup);
			labVisId = selectedSchedule?.ScheduleProtectionLabId ?? 0;

			if (fullVisitingLabs?.Students == null)
			{
				LabsVisitingMarksSubGroup = _currentLabsVisitingMarksSubGroup1;
				return;
			}

			foreach (var subGroupStudent in fullVisitingLabs.Students.Where(v => v.SubGroup == selSubGroup))
			{
				var currentMark = subGroupStudent.LabVisitingMark
					.FirstOrDefault(v => v.ScheduleProtectionLabId == labVisId);

				_currentLabsVisitingMarksSubGroup1.Add(new VisitingPageModel(
					subGroupStudent.FullName,
					currentMark?.Comment ?? string.Empty,
					currentMark?.Mark ?? string.Empty,
					currentMark?.ShowForStudent ?? false));
			}

			LabsVisitingMarksSubGroup = _currentLabsVisitingMarksSubGroup1;
		}

		string _selectedSubGroup;
		public string SelectedSubGroup
		{
			get
			{
				Date1 = new List<string>();
				_currentLabsVisitingMarksSubGroup1 = new List<VisitingPageModel>();
				var subgroupDigits = new string((_selectedSubGroup ?? string.Empty).Where(char.IsDigit).ToArray());
				if (!int.TryParse(subgroupDigits, out var parsedSubGroup))
				{
					parsedSubGroup = currentSubGroups.FirstOrDefault();
				}
				selSubGroup = parsedSubGroup;
				foreach (var labDate in _takedLabs.ScheduleProtectionLabs.Where(v => v.SubGroup == selSubGroup))
				{
					Date1.Add(labDate.Date);
					labVisId = labDate.ScheduleProtectionLabId;
				}
				foreach (var subGroupStudent in fullVisitingLabs.Students.Where(v => v.SubGroup == selSubGroup))
				{
					foreach (var stud in subGroupStudent.LabVisitingMark.Where(v => v.ScheduleProtectionLabId == labVisId))
					{
						_currentLabsVisitingMarksSubGroup1.Add(new VisitingPageModel(subGroupStudent.FullName,
							stud.Comment, stud.Mark, stud.ShowForStudent));
					}
				}
				DateLabs = Date1;
				LabsVisitingMarksSubGroup = _currentLabsVisitingMarksSubGroup1;

				if (DateLabs.Count != 0)
					SelectedLabDate = DateLabs[DateLabs.Count - 1];

				return _selectedSubGroup;
			}
			set
			{
				SetProperty(ref _selectedSubGroup, value);
			}
		}
	}
}
