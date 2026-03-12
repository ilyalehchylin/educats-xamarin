using EduCATS.Data.Models;
using EduCATS.Data;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.LabSchedule;
using EduCATS.Pages.Statistics.Enums;
using EduCATS.Pages.Statistics.Marks.Models;
using EduCATS.Pages.Statistics.Students.Models;
using Newtonsoft.Json;
using Nyxbull.Plugins.CrossLocalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EduCATS.Pages.SaveLabsAndPracticeMarks.Views
{
	public class SavePracticeAndLabsPageViewModel : ViewModel
	{
		public readonly IPlatformServices _services;
		public string _title { get; set; }
		public TakedLabs _takedLabs { get; set; } = new TakedLabs();
		public LabsVisitingList labsVisitingList;
		public LabsVisitingList practiceVisitingList;
		public List<StudentsPageModel> _currentLabsVisitingMarksSubGroup1;
		public List<StudentsPageModel> _currentLabsVisitingMarksSubGroup2;
		public List<string> FullNames { get; set; }

		public SavePracticeAndLabsPageViewModel(IPlatformServices services, int subjectId, int groupId, string title)
		{
			_services = services;
			_title = title;
			_services.Device.MainThread(async () =>
			{
				try
				{
					_services.Dialogs.ShowLoading();
					await initializeData(subjectId, groupId);
					showDataAccessErrorIfNeeded();
				}
				catch (Exception)
				{
				}
				finally
				{
					_services.Dialogs.HideLoading();
				}
			});
		}

		async Task initializeData(int subjectId, int groupId)
		{
			if (_title == CrossLocalization.Translate("practice_mark"))
			{
				practiceVisitingList = await DataAccess.GetTestPracticialStatistics(subjectId, groupId)
					?? new LabsVisitingList();
				_takedLabs = await DataAccess.GetPractTest(subjectId, groupId)
					?? new TakedLabs();
				createPracticialsMarksPage(practiceVisitingList);
			}
			else if (_title == CrossLocalization.Translate("stats_page_labs_rating"))
			{
				labsVisitingList = await DataAccess.GetTestStatistics(subjectId, groupId)
					?? new LabsVisitingList();
				_takedLabs = await DataAccess.GetLabsTest(subjectId, groupId)
					?? new TakedLabs();
				createLabsMarksPage(labsVisitingList);
			}
		}

		void showDataAccessErrorIfNeeded()
		{
			if (DataAccess.IsError && !DataAccess.IsConnectionError)
			{
				_services.Dialogs.ShowError(DataAccess.ErrorMessage);
			}
		}

		private void createLabsMarksPage(LabsVisitingList labsVisitingList)
		{
			FullNames = new List<string>();
			LabsVisitingMarks = new List<StudentsPageModel>();
			Date1 = new List<string>();
			Date2 = new List<string>();
			SubGroup = new List<string>();
			_currentLabsVisitingMarksSubGroup1 = new List<StudentsPageModel>();
			_currentLabsVisitingMarksSubGroup2 = new List<StudentsPageModel>();
			var currentStudents = new List<StudentsPageModel>();
			foreach (var group in labsVisitingList?.Students ?? new List<LaboratoryWorksModel>())
			{
				if (group.SubGroup == 1)
				{
					_currentLabsVisitingMarksSubGroup1.Add(new StudentsPageModel(group.Login, group.FullName));
					FullNames.Add(group.FullName);
				}
				else
				{
					_currentLabsVisitingMarksSubGroup2.Add(new StudentsPageModel(group.Login, group.FullName));
					FullNames.Add(group.FullName);
				}
			}
			foreach (var lab in _takedLabs?.ScheduleProtectionLabs?.Where(v => v.SubGroup == 1) ??
				Enumerable.Empty<ScheduleProtectionLabs>())
			{
				Date1.Add(lab.Date);
			};
			foreach (var lab in _takedLabs?.ScheduleProtectionLabs?.Where(v => v.SubGroup == 2) ??
				Enumerable.Empty<ScheduleProtectionLabs>())
			{
				Date2.Add(lab.Date);
			};
			foreach (var lab in _takedLabs?.ScheduleProtectionLabs ?? new List<ScheduleProtectionLabs>())
			{
				if (!SubGroup.Contains(CrossLocalization.Translate("sub_group") + lab.SubGroup.ToString()))
				{
					SubGroup.Add(CrossLocalization.Translate("sub_group") + lab.SubGroup.ToString());
				}
			}
			LabsVisitingMarksSubGroup = currentStudents;
			LabsVisitingMarksSubGroupOne = _currentLabsVisitingMarksSubGroup1;
			LabsVisitingMarksSubGroupTwo = _currentLabsVisitingMarksSubGroup2;
			SelectedSubGroup = SubGroup.FirstOrDefault();
		}

		private void createPracticialsMarksPage(LabsVisitingList practiceVisitingList)
		{
			var currentStudents = new List<StudentsPageModel>();
			foreach (var pract in practiceVisitingList.Students)
			{
				currentStudents.Add(new StudentsPageModel(pract.Login, pract.FullName));
			}
			Students = currentStudents;
		}

		List<string> _subGroup;
		public List<string> SubGroup
		{
			get { return _subGroup; }
			set { SetProperty(ref _subGroup, value); }
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

		List<StudentsPageModel> _labsVisitingMarks;
		public List<StudentsPageModel> LabsVisitingMarks
		{
			get { return _labsVisitingMarks; }
			set { SetProperty(ref _labsVisitingMarks, value); }
		}

		List<StudentsPageModel> _labsVisitingMarksSubGroup;
		public List<StudentsPageModel> LabsVisitingMarksSubGroup
		{
			get { return _labsVisitingMarksSubGroup; }
			set { SetProperty(ref _labsVisitingMarksSubGroup, value); }
		}

		List<StudentsPageModel> _labsVisitingMarksSubGroupTwo;
		public List<StudentsPageModel> LabsVisitingMarksSubGroupTwo
		{
			get { return _labsVisitingMarksSubGroupTwo; }
			set { SetProperty(ref _labsVisitingMarksSubGroupTwo, value); }
		}

		List<StudentsPageModel> _labsVisitingMarksSubGroupOne;
		public List<StudentsPageModel> LabsVisitingMarksSubGroupOne
		{
			get { return _labsVisitingMarksSubGroupOne; }
			set { SetProperty(ref _labsVisitingMarksSubGroupOne, value); }
		}

		List<string> _datelabs;
		public List<string> DateLabs
		{
			get { return _datelabs; }
			set { SetProperty(ref _datelabs, value); }
		}

		public int selSubGroup { get; set; }

		object _selectedItem;
		public object SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				SetProperty(ref _selectedItem, value);

				if (_selectedItem != null)
				{
					openPage(_selectedItem);
				}
			}
		}

		List<StudentsPageModel> _students;
		public List<StudentsPageModel> Students
		{
			get { return _students; }
			set { SetProperty(ref _students, value); }
		}

		string _selectedSubGroup;
		public string SelectedSubGroup
		{
			get
			{
				if (SubGroup == null || SubGroup.Count == 0)
				{
					return _selectedSubGroup;
				}

				if (_selectedSubGroup == SubGroup[0])
				{
					LabsVisitingMarksSubGroup = LabsVisitingMarksSubGroupOne;
					DateLabs = Date1;
					selSubGroup = 1;
				}
				else if (SubGroup.Count > 1 && _selectedSubGroup == SubGroup[1])
				{
					LabsVisitingMarksSubGroup = LabsVisitingMarksSubGroupTwo;
					DateLabs = Date2;
					selSubGroup = 2;
				}
				return _selectedSubGroup;
			}
			set
			{
				SetProperty(ref _selectedSubGroup, value);
			}
		}

		void openPage(object selectedObject)
		{
			try
			{
				if (selectedObject == null || selectedObject.GetType() != typeof(StudentsPageModel))
				{
					return;
				}

				var student = selectedObject as StudentsPageModel;

				if (_title == CrossLocalization.Translate("practice_mark"))
				{
					_services.Navigation.OpenAddSingleMark(_title, student.Name, practiceVisitingList, _takedLabs, selSubGroup);
				}
				else if (_title == CrossLocalization.Translate("stats_page_labs_rating"))
				{
					_services.Navigation.OpenAddSingleMark(_title, student.Name, labsVisitingList, _takedLabs, selSubGroup);
				}

			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
			}
		}
	}
}
