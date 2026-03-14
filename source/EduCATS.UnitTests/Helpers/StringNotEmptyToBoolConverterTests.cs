using System.Globalization;
using EduCATS.Helpers.Forms.Converters;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class StringNotEmptyToBoolConverterTests
	{
		readonly StringNotEmptyToBoolConverter _converter = new StringNotEmptyToBoolConverter();

		[Test]
		public void ConvertReturnsFalseForNull()
		{
			var result = _converter.Convert(null, null, null, CultureInfo.InvariantCulture);
			Assert.AreEqual(false, result);
		}

		[Test]
		public void ConvertReturnsFalseForWhitespace()
		{
			var result = _converter.Convert("   ", null, null, CultureInfo.InvariantCulture);
			Assert.AreEqual(false, result);
		}

		[Test]
		public void ConvertReturnsTrueForText()
		{
			var result = _converter.Convert("value", null, null, CultureInfo.InvariantCulture);
			Assert.AreEqual(true, result);
		}
	}
}
