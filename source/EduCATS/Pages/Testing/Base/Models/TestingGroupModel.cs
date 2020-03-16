using System.Collections.Generic;
using EduCATS.Data.Models.Testing.Base;

namespace EduCATS.Pages.Testing.Base.Models
{
	public class TestingGroupModel : List<TestingItemModel>
	{
		public string SectionName { get; set; }
		public List<TestingItemModel> Tests => this;

		public TestingGroupModel(string sectionName, List<TestingItemModel> tests = null)
		{
			SectionName = sectionName;

			if (tests != null) {
				AddRange(tests);
			}
		}
	}
}
