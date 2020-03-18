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
			IsNew = file.IsNew;
			FileName = file.FileName;
			PathName = file.PathName;
			AttachmentType = file.AttachmentType;
		}

		public int Id { get; set; }
		public bool IsNew { get; set; }
		public string Name { get; set; }
		public string FileName { get; set; }
		public string PathName { get; set; }
		public int AttachmentType { get; set; }

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
