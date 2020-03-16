using System.Collections.Generic;

namespace EduCATS.Helpers.Lists
{
	public static class ListExtensions
	{
		public static void Swap<T>(this List<T> list, int index1, int index2)
		{
			T temp = list[index1];
			list[index1] = list[index2];
			list[index2] = temp;
		}
	}
}
