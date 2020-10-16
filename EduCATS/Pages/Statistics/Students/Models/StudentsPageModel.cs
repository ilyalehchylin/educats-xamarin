using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;

namespace EduCATS.Pages.Statistics.Students.Models
{
	public class StudentsPageModel : IRoundedListType
	{
		public StudentsPageModel(string username, string name)
		{
			Username = username;
			Name = name;
		}

		public string Username { get; set; }
		public string Name { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
