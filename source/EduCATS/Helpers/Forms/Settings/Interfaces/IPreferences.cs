namespace EduCATS.Helpers.Forms.Settings
{
	public interface IPreferences
	{
		string LanguageCode { get; set; }
		string Theme { get; set; }
		string Server { get; set; }
		bool IsLoggedIn { get; set; }
		string UserLogin { get; set; }
		int UserId { get; set; }
		int ChosenSubjectId { get; set; }
		int GroupId { get; set; }
		string GroupName { get; set; }
		string Avatar { get; set; }
		int ChosenGroupId { get; set; }
		string Font { get; set; }
		bool IsLargeFont { get; set; }
		void ResetPrefs();
	}
}
