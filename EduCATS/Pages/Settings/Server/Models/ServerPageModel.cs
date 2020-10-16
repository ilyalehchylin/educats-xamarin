using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;

namespace EduCATS.Pages.Settings.Server.Models
{
	public class ServerPageModel : IRoundedListType
	{
		public string Title { get; set; }
		public bool IsChecked { get; set; }
		public string Address { get; set; }
		public string Description { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Checkbox;
		}
	}
}
