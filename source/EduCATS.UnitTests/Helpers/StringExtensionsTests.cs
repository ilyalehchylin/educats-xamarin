using NUnit.Framework;
using EduCATS.Helpers.Extensions;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class StringExtensionsTests
	{
		[Test]
		public void StringToDoubleTest()
		{
			var value = "2.05";
			var actual = value.StringToDouble();
			Assert.AreEqual(2.05, actual);
		}

		[Test]
		public void StringInvalidToDoubleTest()
		{
			var value = "invalid_double_string";
			var actual = value.StringToDouble();
			Assert.AreEqual(0, actual);
		}

		[Test]
		public void StringNullToDoubleTest()
		{
			string value = null;
			var actual = value.StringToDouble();
			Assert.AreEqual(0, actual);
		}

		[Test]
		public void FirstCharToUpperTest()
		{
			var value = "the string to test";
			var actual = value.FirstCharToUpper();
			Assert.AreEqual("The string to test", actual);
		}

		[Test]
		public void FirstCharNullToUpperTest()
		{
			string value = null;
			var actual = value.FirstCharToUpper();
			Assert.IsNull(actual);
		}

		[Test]
		public void RemoveHTMLTagsTest()
		{
			var value = "<p>This</p>&#34; is <b>HTML</b> <a href=\"/\">string</a>";
			var actual = value.RemoveHTMLTags();
			Assert.AreEqual("This is HTML string", actual);
		}

		[Test]
		public void RemoveNullHTMLTagsTest()
		{
			string value = null;
			var actual = value.RemoveHTMLTags();
			Assert.IsNull(actual);
		}
	}
}
