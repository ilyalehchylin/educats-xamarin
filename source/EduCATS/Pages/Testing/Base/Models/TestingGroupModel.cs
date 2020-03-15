using System.Collections.Generic;
using EduCATS.Data.Models.Testing;

namespace EduCATS.Pages.Testing.Base.Models
{
	public class TestingGroupModel : List<TestingItemModel>
	{
		public string SectionName { get; set; }
		public List<TestingItemModel> Tests => this;

		public TestingGroupModel(string sectionName)
		{
			SectionName = sectionName;
		}
	}
}
