using Newtonsoft.Json;

namespace EduCATS.Data.Models
{
	public class DataModel
	{
		[JsonIgnore]
		public bool IsError { get; set; }

		[JsonIgnore]
		public string ErrorMessage { get; set; }
	}
}