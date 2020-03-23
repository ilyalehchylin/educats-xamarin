using System.Collections.Generic;

namespace EduCATS.Helpers.Extensions
{
	/// <summary>
	/// List extensions helper.
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Swap list items.
		/// </summary>
		/// <typeparam name="T">Object's type to swap.</typeparam>
		/// <param name="list">Original list.</param>
		/// <param name="index1">The first item.</param>
		/// <param name="index2">The second item.</param>
		public static void Swap<T>(this List<T> list, int index1, int index2)
		{
			T temp = list[index1];
			list[index1] = list[index2];
			list[index2] = temp;
		}
	}
}
