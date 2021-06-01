using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Json;
using EduCATS.Networking;
using EduCATS.Networking.AppServices;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.LabSchedule;
using EduCATS.Pages.Statistics.Marks.Models;
using Newtonsoft.Json;
using Nyxbull.Plugins.CrossLocalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

		public List<VisitingPageModel> _currentLecturesVisitingMarksReady = new List<VisitingPageModel>();
		public List<VisitingPageModel> _currentLabsVisitingMarksReady = new List<VisitingPageModel>();

		public List<VisitingPageModel> _currentLecturesVisitingMarks;
		public List<VisitingPageModel> _currentLabsVisitingMarksSubGroup1;
		public List<VisitingPageModel> _currentLabsVisitingMarksSubGroup2;
		public string _titleOfPage { get; set; }
		public List<string> FullNames { get; set; }
		
		public TakedLabs _takedLabs { get; set; }

		public SaveMarksPageViewModel(IPlatformServices services, int _subjectId, object stat, int groupId, string title)
		{
			_titleOfPage = title;
			_services = services; 
			if (title == CrossLocalization.Translate("stats_page_lectures_visiting"))
			{
				groupData = stat as VisitingLecturesList;
				createLecturesVisitingPage(groupData);
			}
			else if (title == CrossLocalization.Translate("stats_page_labs_visiting"))
			{
				labsVisitingList = stat as LabsVisitingList;
				_takedLabs = new TakedLabs();
				WebRequest request = WebRequest.Create(Links.GetLabsTest + "subjectId=" + _subjectId + "&groupId=" + groupId);
				request.Headers.Add("Authorization", _services.Preferences.AccessToken);
				WebResponse response = request.GetResponse();
				string json = "";
				using (Stream stream = response.GetResponseStream())
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						string line = "";
						while ((line = reader.ReadLine()) != null)
						{
							json += line;
						}
					}
				};
				_takedLabs = JsonConvert.DeserializeObject<TakedLabs>(json);
				createLabsVisitingPage(labsVisitingList);
			}
			else if (title == CrossLocalization.Translate("practiсe_visiting"))
			{
				subjectId = _subjectId;
				practiceVisitingList = stat as LabsVisitingList;
				_takedLabs = new TakedLabs();
				WebRequest request = WebRequest.Create(Links.GetPracticialsTest + "subjectId=" + _subjectId + "&groupId=" + groupId);
				request.Headers.Add("Authorization", _services.Preferences.AccessToken);
				WebResponse response = request.GetResponse();
				string json = "";
				using (Stream stream = response.GetResponseStream())
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						string line = "";
						while ((line = reader.ReadLine()) != null)
						{
							json += line;
						}
					}
				};
				_takedLabs = JsonConvert.DeserializeObject<TakedLabs>(json);
				createPracticialsVisitingPage(practiceVisitingList);
			}
		}

		private void createPracticialsVisitingPage(LabsVisitingList statistics)
		{
			fullVisitingPract = statistics;
			Date = new List<string>();
			FullNames = new List<string>();
			_currentLabsVisitingMarksSubGroup1 = new List<VisitingPageModel>();
			foreach (var group in statistics.Students)
			{
				_currentLabsVisitingMarksSubGroup1.Add(new VisitingPageModel(group.FullName, "", "", false));
				FullNames.Add(group.FullName);
			}
			foreach (var pract in _takedLabs.ScheduleProtectionPracticals)
			{
				Date.Add(pract.Date);
			}
			LecturesMarks = _currentLabsVisitingMarksSubGroup1;
			SelectedDate = Date[Date.Count - 1];
		}

		void createLabsVisitingPage(LabsVisitingList statistics)
		{
			LabsVisitingMarks = new List<VisitingPageModel>();
			Date1 = new List<string>();
			Date2 = new List<string>();
			SubGroup = new List<string>();
			FullNames = new List<string>();
			_currentLabsVisitingMarksSubGroup1 = new List<VisitingPageModel>();
			_currentLabsVisitingMarksSubGroup2 = new List<VisitingPageModel>();
			fullVisitingLabs = statistics;
			foreach (var group in statistics.Students)
			{
				if (group.SubGroup == 1)
				{
					_currentLabsVisitingMarksSubGroup1.Add(new VisitingPageModel(group.FullName, "", "", false));
					FullNames.Add(group.FullName);
				}
				else
				{
					_currentLabsVisitingMarksSubGroup2.Add(new VisitingPageModel(group.FullName, "", "", false));
					FullNames.Add(group.FullName);
				}
			}
			foreach (var lab in _takedLabs.ScheduleProtectionLabs.Where(v => v.SubGroup == 1))
			{
				Date1.Add(lab.Date);
			};
			foreach (var lab in _takedLabs.ScheduleProtectionLabs.Where(v => v.SubGroup == 2))
			{
				Date2.Add(lab.Date);
			};
			foreach(var lab in _takedLabs.ScheduleProtectionLabs)
			{
				if (!SubGroup.Contains(CrossLocalization.Translate("sub_group") + lab.SubGroup.ToString()))
				{
					SubGroup.Add(CrossLocalization.Translate("sub_group") + lab.SubGroup.ToString());
				}
			}
			LabsVisitingMarksSubGroupOne = _currentLabsVisitingMarksSubGroup1;
			LabsVisitingMarksSubGroupTwo = _currentLabsVisitingMarksSubGroup2;
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
							!(_currentLecturesVisitingMarks.Contains(new VisitingPageModel(groupVis.StudentName, "", groupVisit.Mark, false))))
						{
							_currentLecturesVisitingMarks.Add(new VisitingPageModel(groupVis.StudentName, "", groupVisit.Mark, false));
						}
					}
				}
			}
			_currentLecturesVisitingMarksReady = _currentLecturesVisitingMarks;
			SelectedDate = Date[Date.Count-1];
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
					for(int m = 0; m < lect.Marks.Count; m++)
					{
						if (lect.Marks[m].Date == SelectedDate)
						{
							lect.Marks[m].Mark = _currentLecturesVisitingMarksReady[i].Mark;
							lect.Marks[m].Comment = _currentLecturesVisitingMarksReady[i].Comment;
							//lect.Marks[m]. = _currentLecturesVisitingMarksReady[i].ShowForStud;
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
				int dateId = 0;
				foreach (var lab in _takedLabs.ScheduleProtectionLabs.Where(v => v.Date == SelectedLabDate && v.SubGroup == selSubGroup))
				{
					dateId = lab.ScheduleProtectionLabId;
				}
				int i = 0;
				for(int stud = 0; stud < labsVisiting.Count; stud++)
				{
					if (labsVisiting[stud].FullName == LabsVisitingMarksSubGroup[i].Title)
					{
						foreach (var mark in labsVisiting[stud].LabVisitingMark.Where(v => v.ScheduleProtectionLabId == dateId))
						{
							mark.Mark = LabsVisitingMarksSubGroup[i].Mark;
							mark.Comment = LabsVisitingMarksSubGroup[i].Comment;
							labsMarks.comments.Add(LabsVisitingMarksSubGroup[i].Comment);
							labsMarks.Id.Add(mark.LabVisitingMarkId);
							labsMarks.marks.Add(mark.Mark);
							labsMarks.showForStudents.Add(LabsVisitingMarksSubGroup[i].ShowForStud);
							labsMarks.studentsId.Add(labsVisiting[stud].StudentId);
						};
						labsMarks.students.Add(labsVisiting[stud]);
						i++;
						if (i == LabsVisitingMarksSubGroup.Count)
						{
							break;
						}
					};
				};
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
				foreach (var pr in _takedLabs.ScheduleProtectionPracticals.Where(v => v.Date == SelectedDate))
				{
					dateId = pr.ScheduleProtectionPracticalId;
				}
				int i = 0;
				for(int pract = 0; pract < practVisiting.Count; pract++)
				{
					foreach(var mark in practVisiting[pract].PracticalVisitingMark.Where(v => v.ScheduleProtectionPracticalId == dateId))
					{
						mark.Mark = LecturesMarks[i].Mark;
						mark.Comment = LecturesMarks[i].Comment;
						savePracticial.Id.Add(mark.PracticalVisitingMarkId);
						savePracticial.marks.Add(mark.Mark);
						savePracticial.showForStudents.Add(LecturesMarks[i].ShowForStud);
						savePracticial.studentsId.Add(practVisiting[pract].StudentId);
						savePracticial.comments.Add(LecturesMarks[i].Comment);
					}
					savePracticial.students.Add(practVisiting[pract]);
					i++;
				}
				savePracticial.dateId = dateId;
				savePracticial.subjectId = subjectId;
				body = JsonController.ConvertObjectToJson(savePracticial);
			}
			await _services.Navigation.ClosePage(false);
			return await AppServicesController.Request(link, body);
		}

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

		List<VisitingPageModel> _labsVisitingMarksSubGroupTwo;
		public List<VisitingPageModel> LabsVisitingMarksSubGroupTwo
		{
			get { return _labsVisitingMarksSubGroupTwo; }
			set { SetProperty(ref _labsVisitingMarksSubGroupTwo, value); }
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

		List<string> _date2;
		public List<string> Date2
		{
			get { return _date2; }
			set { SetProperty(ref _date2, value); }
		}

		string _selectedDate;
		public string SelectedDate
		{
			get { return _selectedDate; }
			set { SetProperty(ref _selectedDate, value); }
		}

		string _selectedLabDate;
		public string SelectedLabDate
		{
			get { return _selectedLabDate; }
			set { SetProperty(ref _selectedLabDate, value); }
		}

		string _selectedDate1;
		public string SelectedDate1
		{
			get { return _selectedDate1; }
			set { SetProperty(ref _selectedDate1, value); }
		}

		string _selectedDate2;
		public string SelectedDate2
		{
			get { return _selectedDate2; }
			set { SetProperty(ref _selectedDate2, value); }
		}

		string _selectedSubGroup;
		public string SelectedSubGroup
		{
			get
			{
				if (_selectedSubGroup == SubGroup[0])
				{
					LabsVisitingMarksSubGroup = LabsVisitingMarksSubGroupOne;
					DateLabs = Date1;
					selSubGroup = 1;
					SelectedLabDate = DateLabs[DateLabs.Count - 1];
				}
				else if (_selectedSubGroup == SubGroup[1])
				{
					LabsVisitingMarksSubGroup = LabsVisitingMarksSubGroupTwo;
					DateLabs = Date2;
					selSubGroup = 2;
					SelectedLabDate = DateLabs[DateLabs.Count - 1];
				}
				return _selectedSubGroup;
			}
			set 
			{
				SetProperty(ref _selectedSubGroup, value); 
			}
		}
	}
}
