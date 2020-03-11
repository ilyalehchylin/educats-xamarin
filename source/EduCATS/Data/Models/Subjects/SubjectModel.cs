using System.Collections.Generic;

namespace EduCATS.Data.Models.Subjects
{
	public class SubjectModel : DataModel
	{
		public IList<SubjectItemModel> SubjectsList { get; set; }
	}
}