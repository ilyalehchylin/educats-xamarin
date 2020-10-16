using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;

namespace EduCATS.Pages.Settings.Themes.Models
{
	public class ThemePageModel : IRoundedListType
	{
		public string Title { get; set; }
		public string Theme { get; set; }
		public bool IsChecked { get; set; }
		public string Description { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Checkbox;
		}
	}
}
