using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;
using EduCATS.Data.Models;

namespace EduCATS.Pages.Files.Models
{
	public class FilesPageModel : IRoundedListType
	{
		public FilesPageModel(FileDetailsModel file, bool exists)
		{
			if (file == null) {
				return;
			}

			Id = file.Id;
			Name = file.Name;
			IsDownloaded = exists;
			FileName = file.FileName;
			PathName = file.PathName;
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string FileName { get; set; }
		public string PathName { get; set; }
		public bool IsDownloaded { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
