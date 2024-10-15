using System.Collections.Generic;
using EduCATS.Data.Models;

namespace EduCATS.Pages.Testing.Base.Models
{
	public class TestingGroupModel : List<TestModel>
	{
		public string SectionName { get; set; }
		public string Comment { get; set; }

		public List<TestModel> Tests => this;

		public TestingGroupModel(string sectionName, string comment, List<TestModel> tests = null, bool isSelfStudy = true)
		{
			SectionName = sectionName;
			if (!isSelfStudy)
			{
				Comment = comment;
			}

			if (tests != null) {
				AddRange(tests);
			}
		}
	}
}
