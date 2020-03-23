namespace EduCATS.Data.Interfaces
{
	/// <summary>
	/// Data access interface.
	/// </summary>
	public interface IDataAccess
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
	}
}
