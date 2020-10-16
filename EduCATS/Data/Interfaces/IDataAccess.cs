using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduCATS.Data.Interfaces
{
	/// <summary>
	/// Data access interface.
	/// </summary>
	public interface IDataAccess<T>
	{
		/// <summary>
		/// Is error occurred during API call.
		/// </summary>
		bool IsError { get; set; }

		/// <summary>
		/// Is error referred to connection issues.
		/// </summary>
		bool IsConnectionError { get; set; }

		/// <summary>
		/// Error message localization key.
		/// </summary>
		string ErrorMessageKey { get; set; }

		Task<T> GetSingle();

		Task<List<T>> GetList();
	}
}
