using NUnit.Framework;
using EduCATS.Helpers.Extensions;
using System.Collections.Generic;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class ListExtensionsTests
	{
		[Test]
		public void SwapTest()
		{
			var list = new List<int> { 1, 2, 3 };
			var expected = new List<int> { 3, 2, 1 };
			list.Swap(0, 2);
			Assert.AreEqual(expected, list);
		}
	}
}
