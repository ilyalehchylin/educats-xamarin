using System.Collections.Generic;

namespace EduCATS.Data.Models.Testing
{
	public class TestingModel : DataModel
	{
		public IList<TestingItemModel> Tests { get; set; }
	}
}
