namespace EduCATS.Networking.Models.Login
{
	/// <summary>
	/// User authorize <c>POST</c> model.
	/// </summary>
	public class UserCredentials
	{
		/// <summary>
		/// User login.
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Password.
		/// </summary>
		public string Password { get; set; }
	}
}
