using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
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

		/// <summary>
		/// Platform services.
		/// </summary>
		public readonly IPlatformServices PlatformServices;

		public List<GroupItemModel> CurrentGroups { get; set; }
		public GroupItemModel CurrentGroup { get; set; }

		public delegate void GroupEventHandler(int id, string name);
		public event GroupEventHandler GroupChanged;

		public GroupsViewModel(IPlatformServices platformServices, int subjectId)
		{
			SubjectId = subjectId;
			PlatformServices = platformServices;
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
					_chooseGroupCommand = new Command(chooseGroup));
			}
		}

		Command _groupSelectedCommand;
		public Command GroupSelectedCommand {
			get {
				return _groupSelectedCommand ?? (_groupSelectedCommand = new Command(subjectChosen));
			}
		}

		/// <summary>
		/// Setup groups.
		/// </summary>
		/// <returns>Task.</returns>
		public async Task SetupGroups()
		{
			try {
				var groups = await getGroups();

				if (groups == null) {
					return;
				}

				setCurrentGroupsList(groups.ToList());
				setupGroup();
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		/// <summary>
		/// Fetch groups.
		/// </summary>
		/// <returns>List of groups.</returns>
		async Task<IList<GroupItemModel>> getGroups()
		{
			var groups = await DataAccess.GetOnlyGroups(SubjectId);

			if (DataAccess.IsError) {
				PlatformServices.Device.MainThread(
					() => PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage));
			}

			return groups.GroupsList;
		}

		/// <summary>
		/// Setup group with group name.
		/// </summary>
		/// <param name="groupName">Group name.</param>
		void setupGroup()
		{
			if (!checkGroupsList()) {
				return;
			}

			var savedSubjectId = PlatformServices.Preferences.ChosenGroupId;
			var success = setChosenGroup(savedSubjectId);

			if (!success) {
				setChosenGroup(CurrentGroups[0]);
			}
		}

		protected void chooseGroup()
		{
			try {
				if (CurrentGroups == null) {
					return;
				}

				var buttons = new Dictionary<int, string>();
				foreach (var group in CurrentGroups) {
					buttons.Add(group.GroupId, group.GroupName);
				}

				PlatformServices.Dialogs.ShowSheet(
					CrossLocalization.Translate("groups_choose"), buttons, GroupSelectedCommand);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		protected void subjectChosen(object chosenObject)
		{
			try {
				var id = Convert.ToInt32(chosenObject);

				if (id == -1) {
					return;
				}

				var group = CurrentGroups.SingleOrDefault(s => s.GroupId == id);
				var isChosen = setChosenGroup(group);

				if (isChosen) {
					GroupChanged?.Invoke(PlatformServices.Preferences.GroupId, group.GroupName);
				}
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		bool setChosenGroup(int groupId)
		{
			var group = CurrentGroups.SingleOrDefault(
						g => g.GroupId == groupId);
			return setChosenGroup(group);
		}

		bool setChosenGroup(GroupItemModel group)
		{
			if (group != null) {
				CurrentGroup = group;
				ChosenGroup = group.GroupName;
				PlatformServices.Preferences.ChosenGroupId = group.GroupId;
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
