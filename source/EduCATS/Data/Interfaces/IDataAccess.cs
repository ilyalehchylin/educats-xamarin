namespace EduCATS.Data.Interfaces
{
	public interface IDataAccess
	{
		bool IsError { get; set; }
		bool IsConnectionError { get; set; }
		string ErrorMessage { get; set; }
	}
}
