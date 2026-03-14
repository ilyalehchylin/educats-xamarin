using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using EduCATS.Data.Models;
using EduCATS.Pages.Testing.Base.ViewModels;
using NUnit.Framework;

namespace EduCATS.UnitTests.Pages
{
	[TestFixture]
	public class TestingPageViewModelTests
	{
		static readonly BindingFlags _privateInstance =
			BindingFlags.Instance | BindingFlags.NonPublic;

		[Test]
		public void GetSeparateTestsFiltersForNnLegacyInsAndDuplicates()
		{
			var viewModel = createWithoutConstructor();
			var method = typeof(TestingPageViewModel).GetMethod("getSeparateTests", _privateInstance);
			var source = new List<TestModel> {
				new TestModel { Id = 1, Title = "Regular control test", ForSelfStudy = false, ForNN = false },
				new TestModel { Id = 2, Title = "инс internal", ForSelfStudy = false, ForNN = false },
				new TestModel { Id = 3, Title = "NN path", ForSelfStudy = false, ForNN = true },
				new TestModel { Id = 1, Title = "Regular control test duplicate", ForSelfStudy = false, ForNN = false },
				new TestModel { Id = 4, Title = "Self study test", ForSelfStudy = true, ForNN = false }
			};

			var result = (List<TestModel>)method.Invoke(viewModel, new object[] { source, false });

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(1, result[0].Id);
		}

		[Test]
		public void GetSeparateTestsReturnsOnlySelfStudyGroup()
		{
			var viewModel = createWithoutConstructor();
			var method = typeof(TestingPageViewModel).GetMethod("getSeparateTests", _privateInstance);
			var source = new List<TestModel> {
				new TestModel { Id = 10, Title = "Control test", ForSelfStudy = false, ForNN = false },
				new TestModel { Id = 11, Title = "Self test", ForSelfStudy = true, ForNN = false },
				new TestModel { Id = 12, Title = "Self NN test", ForSelfStudy = true, ForNN = true }
			};

			var result = (List<TestModel>)method.Invoke(viewModel, new object[] { source, true });

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(11, result[0].Id);
		}

		static TestingPageViewModel createWithoutConstructor()
		{
			return (TestingPageViewModel)FormatterServices.GetUninitializedObject(
				typeof(TestingPageViewModel));
		}
	}
}
