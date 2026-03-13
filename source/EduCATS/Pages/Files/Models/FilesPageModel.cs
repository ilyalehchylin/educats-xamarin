using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;
using EduCATS.Data.Models;
using Nyxbull.Plugins.CrossLocalization;

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
			var size = (double)file.Size / 100000;

			if (size > 0) {
				Size = size.ToString("0.00");
				Size = $"{Size} {CrossLocalization.Translate("files_megabytes")}";
			}
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string FileName { get; set; }
		public string PathName { get; set; }
		public bool IsDownloaded { get; set; }
		public string Size { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
