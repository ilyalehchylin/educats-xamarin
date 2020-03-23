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

namespace EduCATS.Pages.Pickers
{
	/// <summary>
	/// Groups view model.
	/// </summary>
	/// <remarks>Used for Group picker.</remarks>
	public class GroupsViewModel : ViewModel
	{
		public readonly int SubjectId;
		public readonly IDialogs DialogService;
		public readonly IDevice DeviceService;

		public List<GroupItemModel> CurrentGroups { get; set; }
		public GroupItemModel CurrentGroup { get; set; }

		public delegate void GroupEventHandler(int id, string name);
		public event GroupEventHandler GroupChanged;

		public GroupsViewModel(IDialogs dialogService, IDevice deviceService, int subjectId)
		{
			SubjectId = subjectId;
			DialogService = dialogService;
			DeviceService = deviceService;
		}

		string _chosenGroup;
		public string ChosenGroup {
			get { return _chosenGroup; }
			set { SetProperty(ref _chosenGroup, value); }
		}

		Command _chooseGroupCommand;
		public Command ChooseGroupCommand {
			get {
				return _chooseGroupCommand ?? (
					_chooseGroupCommand = new Command(
						async () => await executeChooseGroupCommand()));
			}
		}

		/// <summary>
		/// Setup groups.
		/// </summary>
		/// <returns>Task.</returns>
		public async Task SetupGroups()
		{
			var groups = await getGroups();

			if (groups == null) {
				return;
			}

			setCurrentGroupsList(groups.ToList());
			setupGroup();
		}

		/// <summary>
		/// Fetch groups.
		/// </summary>
		/// <returns>List of groups.</returns>
		async Task<IList<GroupItemModel>> getGroups()
		{
			var groups = await DataAccess.GetOnlyGroups(SubjectId);

			if (DataAccess.IsError) {
				DeviceService.MainThread(
					() => DialogService.ShowError(DataAccess.ErrorMessage));
			}

			return groups.GroupsList;
		}

		/// <summary>
		/// Setup group with group name.
		/// </summary>
		/// <param name="groupName">Group name.</param>
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
				string.Compare(name, CrossLocalization.Translate("base_cancel")) == 0) {
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
