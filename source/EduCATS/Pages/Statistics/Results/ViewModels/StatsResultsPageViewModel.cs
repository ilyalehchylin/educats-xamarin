using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Constants;
using EduCATS.Data;
using EduCATS.Data.Models.Labs;
using EduCATS.Data.Models.Statistics;
using EduCATS.Data.User;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
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
		readonly StatsPageEnum _statisticsPage;
		readonly IDialogs _dialogs;
		readonly IAppDevice _device;

		List<StatsPageLabsVisitingModel> _currentLabsVisitingList;
		List<StatsPageLabsRatingModel> _currentLabsMarksList;

		public StatsResultsPageViewModel(
			IDialogs dialogs, IAppDevice device, string userLogin,
			int subjectId, int groupId, StatsPageEnum statisticsPage)
		{
			_dialogs = dialogs;
			_device = device;
			_currentUserLogin = userLogin;
			_currentSubjectId = subjectId;
			_currentGroupId = groupId;
			_statisticsPage = statisticsPage;
			Task.Run(async () => await getData());
		}

		List<StatsResultsPageModel> _marks;
		public List<StatsResultsPageModel> Marks {
			get { return _marks; }
			set { SetProperty(ref _marks, value); }
		}

		bool _isLoading;
		public bool IsLoading {
			get { return _isLoading; }
			set { SetProperty(ref _isLoading, value); }
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
			// TODO: error handling
			IsLoading = true;

			switch (_statisticsPage) {
				case StatsPageEnum.LabsRating:
					await getLabs(false);
					await getLabsMarksAndVisiting();
					break;
				case StatsPageEnum.LabsVisiting:
					await getLabs(true);
					await getLabsMarksAndVisiting();
					break;
				case StatsPageEnum.LecturesVisiting:
					await getLecturesVisiting();
					break;
			}

			if (DataAccess.IsError) {
				_device.MainThread(
					() => _dialogs.ShowError(DataAccess.ErrorMessage));
			}

			IsLoading = false;
		}

		async Task getLabs(bool isVisiting)
		{
			var dataLabs = await DataAccess.GetLabs(_currentSubjectId, _currentGroupId);

			if (dataLabs == null) {
				return;
			}

			if (isVisiting) {
				setVisitingLabsStatistics(dataLabs);
			} else {
				setRatingLabsStatistics(dataLabs);
			}
		}

		void setVisitingLabsStatistics(LabsModel dataLabs)
		{
			var labsVisitingStatus = dataLabs.ScheduleProtectionLabs?.Select(
					labs => new StatsPageLabsVisitingModel(
						labs.ScheduleProtectionLabId, labs.Date));

			if (labsVisitingStatus == null) {
				return;
			}

			_currentLabsVisitingList = new List<StatsPageLabsVisitingModel>(labsVisitingStatus);
		}

		void setRatingLabsStatistics(LabsModel dataLabs)
		{
			var marksLabsList = dataLabs.Labs?.Select(
					labs => new StatsPageLabsRatingModel(
						labs.LabId, labs.ShortName, labs.Theme));

			if (marksLabsList == null) {
				return;
			}

			_currentLabsMarksList = new List<StatsPageLabsRatingModel>(marksLabsList);
		}

		async Task getLecturesVisiting()
		{
			var visitingData = await DataAccess.GetLectures(_currentSubjectId, _currentGroupId);
			var groupVisiting = visitingData?.GroupsVisiting?[0];

			var userLecturesVisiting = groupVisiting?.LecturesVisiting
				.SingleOrDefault(v => string.Compare(v.Login?.ToLower(), _currentUserLogin?.ToLower()) == 0);

			var stats = userLecturesVisiting?.VisitingList?.Select(
				u => new StatsResultsPageModel(
					null, u.Date, null, string.IsNullOrEmpty(u.Mark) ? GlobalConsts.EmptyRatingString : u.Mark));

			var statsList = stats?.ToList();

			if (AppUserData.UserType == UserTypeEnum.Student) {
				statsList?.RemoveAll(s => s.Result.Equals(GlobalConsts.EmptyRatingString));
			}

			if (statsList == null) {
				return;
			}

			Marks = new List<StatsResultsPageModel>(statsList);
		}

		void setMarks(StatisticsStudentModel student)
		{
			if (student?.MarkList == null) {
				return;
			}

			var marksResults = student.MarkList?.Select(m => {
				var lab = _currentLabsMarksList?.FirstOrDefault(l => l.LabId == m.LabId);
				var labTitle = lab == null ? null : $"{lab.ShortName}. {lab.Theme}";
				var result = string.IsNullOrEmpty(m.Mark) ? GlobalConsts.EmptyRatingString : m.Mark;
				return new StatsResultsPageModel(labTitle, m.Date, m.Comment, result);
			});

			Marks = new List<StatsResultsPageModel>(marksResults);
		}

		void setLabsVisiting(StatisticsStudentModel student)
		{
			if (student?.VisitingList == null) {
				return;
			}

			var visitingLabsResult = student.VisitingList.Select(v => {
				var lab = _currentLabsVisitingList.FirstOrDefault(
					l => l.ProtectionLabId == v.ScheduleProtectionLabId);
				var result = string.IsNullOrEmpty(v.Mark) ? GlobalConsts.EmptyRatingString : v.Mark;
				return new StatsResultsPageModel(null, lab?.Date, v.Comment, result);
			});

			Marks = new List<StatsResultsPageModel>(visitingLabsResult);
		}

		async Task getLabsMarksAndVisiting()
		{
			var stats = await DataAccess.GetStatistics(_currentSubjectId, _currentGroupId);

			var student = stats?.Students?.SingleOrDefault(
				s => string.Compare(s.Login?.ToLower(), _currentUserLogin?.ToLower()) == 0);

			if (_statisticsPage == StatsPageEnum.LabsRating) {
				setMarks(student);
			} else {
				setLabsVisiting(student);
			}
		}
	}
}
