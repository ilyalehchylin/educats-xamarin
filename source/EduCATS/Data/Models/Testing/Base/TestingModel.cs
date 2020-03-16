using System.Collections.Generic;

namespace EduCATS.Data.Models.Testing.Base
{
	public class TestingModel : DataModel
	{
		public IList<TestingItemModel> Tests { get; set; }
	}
}
