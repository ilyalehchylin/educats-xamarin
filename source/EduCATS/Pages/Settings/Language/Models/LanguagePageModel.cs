using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;

namespace EduCATS.Pages.Settings.Language.Models
{
	public class LanguagePageModel : IRoundedListType
	{
		public string Title { get; set; }
		public bool IsChecked { get; set; }
		public string Description { get; set; }
		public string LanguageCode { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Checkbox;
		}
	}
}
