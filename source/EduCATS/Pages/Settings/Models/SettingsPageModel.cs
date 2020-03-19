using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;

namespace EduCATS.Pages.Settings.Base.Models
{
	public class SettingsPageModel : IRoundedListType
	{
		public string Icon { get; set; }
		public string Title { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
