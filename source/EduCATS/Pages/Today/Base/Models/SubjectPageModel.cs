using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;
using EduCATS.Data.Models.Calendar;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Pages.Today.Base.Models
{
	public class SubjectPageModel : IRoundedListType
	{
		public string Address { get; set; }
		public string Color { get; set; }
		public string Name { get; set; }

		string _teacherFullName;
		public string TeacherFullName {
			get {
				var splitted = _teacherFullName.Split(' ');
				var surname = "";
				var name = "";
				var patronymic = "";
				var nameFirstLetter = "";
				var patronymicFirstLetter = "";

				if (splitted.Length > 2) {
					surname = splitted[0];
					name = splitted[1];
					patronymic = splitted[2];
				} else if (splitted.Length < 3 && splitted.Length > 1) {
					surname = splitted[0];
					name = splitted[1];
				} else if (splitted.Length == 1) {
					surname = splitted[0];
				}

				if (!string.IsNullOrEmpty(name) && name.Length > 0) {
					nameFirstLetter = $"{name[0]}";
				}

				if (!string.IsNullOrEmpty(patronymic) && patronymic.Length > 0) {
					patronymicFirstLetter = $"{patronymic[0]}";
				}

				if (!string.IsNullOrEmpty(surname) &&
					!string.IsNullOrEmpty(nameFirstLetter) &&
					!string.IsNullOrEmpty(patronymicFirstLetter)) {
					return $"{surname} {nameFirstLetter}. {patronymicFirstLetter}.";
				} else if (
					string.IsNullOrEmpty(patronymicFirstLetter) &&
					!string.IsNullOrEmpty(surname) &&
					!string.IsNullOrEmpty(nameFirstLetter)) {
					return $"{surname} {nameFirstLetter}.";
				} else if (
					string.IsNullOrEmpty(nameFirstLetter) &&
					string.IsNullOrEmpty(patronymicFirstLetter) &&
					!string.IsNullOrEmpty(surname)) {
					return surname;
				}

				return string.Empty;
			}
			set {
				_teacherFullName = value;
			}
		}

		public string Date { get; set; }
		public string Type { get; set; }

		public SubjectPageModel(Schedule schedule)
		{
			setDefaultProps(schedule);
		}

		void setDefaultProps(Schedule schedule)
		{
			if (schedule != null) {
				Color = schedule.Color;
				Address = $"{CrossLocalization.Translate("address_building")} {schedule.Building}, {CrossLocalization.Translate("address_room")} {schedule.Audience}";
				Date = schedule.Start + "-" + schedule.End;
				Name = schedule.Name;

				
				switch (schedule.Type) {
					case 0:
						Type = CrossLocalization.Translate("type_activity_lecture");
						break;
					case 1:
						Type = CrossLocalization.Translate("type_activity_practical");
						break;
					case 2:
						Type = CrossLocalization.Translate("type_activity_lab");
						break;
					case 3:
						Type = CrossLocalization.Translate("type_activity_kconsultation");
						break;
					case 4:
						Type = CrossLocalization.Translate("type_activity_dconsultation");
						break;
					default:
						break;
				}

				if (schedule.Teacher != null) {
					TeacherFullName = schedule.Teacher.FullName;
				}
			}
		}

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
