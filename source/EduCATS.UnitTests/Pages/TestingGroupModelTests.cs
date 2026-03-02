using System.Collections.Generic;
using EduCATS.Data.Models;
using EduCATS.Pages.Testing.Base.Models;
using NUnit.Framework;

namespace EduCATS.UnitTests.Pages
{
	[TestFixture]
	public class TestingGroupModelTests
	{
		[Test]
		public void SelfStudyGroupDoesNotExposeComment()
		{
			var group = new TestingGroupModel(
				"Self study",
				"Comment should not be shown",
				new List<TestModel>(),
				isSelfStudy: true);

			Assert.IsNull(group.Comment);
			Assert.IsFalse(group.IsCommentVisible);
		}

		[Test]
		public void KnowledgeControlGroupExposesComment()
		{
			var group = new TestingGroupModel(
				"Knowledge control",
				"Visible comment",
				new List<TestModel>(),
				isSelfStudy: false);

			Assert.AreEqual("Visible comment", group.Comment);
			Assert.IsTrue(group.IsCommentVisible);
		}
	}
}
