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
		void setupGroup(string groupName = null)
		{
			if (!checkGroupsList()) {
				return;
			}

			if (string.IsNullOrEmpty(groupName)) {
				var savedSubjectId = PlatformServices.Preferences.ChosenGroupId;
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
			try {
				if (CurrentGroups == null) {
					return;
				}

				var buttons = CurrentGroups.Select(g => g.GroupName).ToList();
				var name = await PlatformServices.Dialogs.ShowSheet(
					CrossLocalization.Translate("subjects_choose"), buttons);

				if (string.IsNullOrEmpty(name) ||
					string.Compare(name, CrossLocalization.Translate("base_cancel")) == 0) {
					return;
				}

				var isChosen = setChosenGroup(name);

				if (isChosen) {
					GroupChanged?.Invoke(PlatformServices.Preferences.GroupId, name);
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
