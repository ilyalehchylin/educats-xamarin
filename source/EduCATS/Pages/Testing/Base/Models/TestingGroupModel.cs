using System.Collections.Generic;
using EduCATS.Data.Models.Testing.Base;

namespace EduCATS.Pages.Testing.Base.Models
{
	public class TestingGroupModel : List<TestModel>
	{
		public string SectionName { get; set; }
		public List<TestModel> Tests => this;

		public TestingGroupModel(string sectionName, List<TestModel> tests = null)
		{
			SectionName = sectionName;

			if (tests != null) {
				AddRange(tests);
			}
		}
	}
}
