using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using EduCATS.Helpers;
namespace EduCATS.UnitTests.Helpers
{
	class ConverterSizeTest
	{
		[Test]
		public void ConvertZeroBytesTest()
		{
			try
			{
				long bytes = 0;
				string expected = "0.0Bytes";
				Assert.AreEqual(expected, ConverterSize.FormatSize(bytes));
				return;
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void ConvertKiloBytesTest()
		{
			try
			{
				long bytes = 1024;
				string expected = "1.0KB";
				Assert.AreEqual(expected, ConverterSize.FormatSize(bytes));
				return;
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void ConvertMegaBytesTest()
		{
			try
			{
				long bytes = 1048576;
				string expected = "1.0MB";
				Assert.AreEqual(expected, ConverterSize.FormatSize(bytes));
				return;
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void ConvertGigaBytesTest()
		{
			try
			{
				long bytes = 1073741824;
				string expected = "1.0GB";
				Assert.AreEqual(expected, ConverterSize.FormatSize(bytes));
				return;
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void ConvertTeraBytesTest()
		{
			try
			{
				long bytes = 1099511627776;
				string expected = "1.0TB";
				Assert.AreEqual(expected, ConverterSize.FormatSize(bytes));
				return;
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void ConvertPetaBytesTest()
		{
			try
			{
				long bytes = 1125899906842624;
				string expected = "1.0PB";
				Assert.AreEqual(expected, ConverterSize.FormatSize(bytes));
				return;
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}
	}
}
