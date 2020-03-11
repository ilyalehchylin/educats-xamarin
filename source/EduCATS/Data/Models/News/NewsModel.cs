using System.Collections.Generic;

namespace EduCATS.Data.Models.News
{
	public class NewsModel : DataModel
	{
		public IList<NewsItemModel> News { get; set; }
	}
}