using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;

namespace EduCATS.Pages.Settings.Fonts.Models
{
	public class FontsPageModel : IRoundedListType
	{
		public string Title { get; set; }
		public bool IsChecked { get; set; }
		public string Font { get; set; }
		public string FontFamily { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Checkbox;
		}
	}
}
