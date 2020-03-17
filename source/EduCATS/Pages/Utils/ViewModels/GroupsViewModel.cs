using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.Groups;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Settings;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Utils.ViewModels
{
	public class GroupsViewModel : ViewModel
	{
		public readonly int SubjectId;
		public readonly IDialogs DialogService;
		public readonly IAppDevice DeviceService;

		public List<GroupItemModel> CurrentGroups { get; set; }
		public GroupItemModel CurrentGroup { get; set; }

		public delegate void GroupEventHandler(int id, string name);
		public event GroupEventHandler GroupChanged;

		public GroupsViewModel(IDialogs dialogService, IAppDevice deviceService, int subjectId)
		{
			SubjectId = subjectId;
			DialogService = dialogService;
			DeviceService = deviceService;
		}

		string chosenGroup;
		public string ChosenGroup {
			get { return chosenGroup; }
			set { SetProperty(ref chosenGroup, value); }
		}

		Command chooseGroupCommand;
		public Command ChooseGroupCommand {
			get {
				return chooseGroupCommand ?? (
					chooseGroupCommand = new Command(
						async () => await executeChooseGroupCommand()));
			}
		}

		public async Task SetupGroups()
		{
			var groups = await getGroups();

			if (groups == null) {
				return;
			}

			setCurrentGroupsList(groups.ToList());
			setupGroup();
		}

		async Task<IList<GroupItemModel>> getGroups()
		{
			var groups = await DataAccess.GetOnlyGroups(SubjectId);

			if (DataAccess.IsError) {
				DeviceService.MainThread(
					() => DialogService.ShowError(DataAccess.ErrorMessage));
			}

			return groups.GroupsList;
		}

		void setupGroup(string groupName = null)
		{
			if (!checkGroupsList()) {
				return;
			}

			if (string.IsNullOrEmpty(groupName)) {
				var savedSubjectId = AppPrefs.ChosenGroupId;
				var success = setChosenGroup(savedSubjectId);

				if (!success) {
					setChosenGroup(CurrentGroups[0]);
				}

				return;
			}

			setChosenGroup(groupName);
		}

		protected async Task executeChooseGroupCommand()
		{
			if (CurrentGroups == null) {
				return;
			}

			var buttons = CurrentGroups.Select(g => g.GroupName).ToList();
			var name = await DialogService.ShowSheet(
				CrossLocalization.Translate("subjects_choose"), buttons);

			if (string.IsNullOrEmpty(name) ||
				string.Compare(name, CrossLocalization.Translate("common_cancel")) == 0) {
				return;
			}

			var isChosen = setChosenGroup(name);

			if (isChosen) {
				GroupChanged?.Invoke(AppPrefs.GroupId, name);
			}
		}

		bool setChosenGroup(int groupId)
		{
			var group = CurrentGroups.SingleOrDefault(
						g => g.GroupId == groupId);
			return setChosenGroup(group);
		}

		bool setChosenGroup(string groupName)
		{
			var group = CurrentGroups.SingleOrDefault(
						g => string.Compare(g.GroupName, groupName) == 0);
			return setChosenGroup(group);
		}

		bool setChosenGroup(GroupItemModel group)
		{
			if (group != null) {
				CurrentGroup = group;
				ChosenGroup = group.GroupName;
				AppPrefs.ChosenGroupId = group.GroupId;
				return true;
			}

			return false;
		}

		void setCurrentGroupsList(List<GroupItemModel> groups)
		{
			CurrentGroups = groups.OrderBy(g => g.GroupName).ToList();
		}

		bool checkGroupsList()
		{
			if (CurrentGroups != null && CurrentGroups.Count > 0) {
				return true;
			}

			return false;
		}
	}
}
