namespace EduCATS.Pages.Statistics.Results.Models
{
	public class Theme
	{
		public Theme(string shortName, string theme)
		{
			ShortName = shortName;
			LabTheme = theme;
		}

		public string ShortName { get; set; }
		public string LabTheme { get; set; }
	}
}
