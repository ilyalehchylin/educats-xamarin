﻿using EduCATS.Controls.RoundedListView.Enums;
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
		public string TeacherFullName { get; set; }
		public string Date { get; set; }
		public string Type { get; set; }

		public SubjectPageModel(Schedule schedule)
		{
			setDefaultProps(schedule);
		}

		void setDefaultProps(Schedule schedule)
		{
			if (schedule != null)
			{
				Color = schedule.Color;
				Address = $"{CrossLocalization.Translate("address_building")} {schedule.Building}, {CrossLocalization.Translate("address_room")} {schedule.Audience}";
				Date = schedule.Start + "-" + schedule.End;
				Name = schedule.Name;

				switch (schedule.Type)
				{
					case 0:
						Type = CrossLocalization.Translate("type_activity_lecture");
						break;
					case 1:
						Type = CrossLocalization.Translate("type_activity_practical");
						break;
					case 2:
						Type = CrossLocalization.Translate("type_activity_lab");
						break;

					default:
						break;
				}

				if (schedule.Teacher != null)
				{
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
