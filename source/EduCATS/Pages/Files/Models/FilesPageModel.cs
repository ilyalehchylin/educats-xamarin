using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;
using EduCATS.Data.Models.Files;

namespace EduCATS.Pages.Files.Models
{
	public class FilesPageModel : IRoundedListType
	{
		public FilesPageModel(FileDetailsModel file)
		{
			if (file == null) {
				return;
			}

			Id = file.Id;
			Name = file.Name;
			FileName = file.FileName;
			PathName = file.PathName;
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string FileName { get; set; }
		public string PathName { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
