using System.Linq;

namespace EduCATS.Helpers.Extensions
{
	public static class StringExtensions
	{
        public static string FirstCharToUpper(this string input)
        {
			return string.IsNullOrEmpty(input) ?
				input : input.First().ToString().ToUpper() + input.Substring(1);
		}
	}
}
