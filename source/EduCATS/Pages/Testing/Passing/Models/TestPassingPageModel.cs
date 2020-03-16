namespace EduCATS.Pages.Testing.Passing.Models
{
	public class TestPassingPageModel
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public int QuestionType { get; set; }
		public int QuestionNumber { get; set; }
		public bool IsTimeForEntireTest { get; set; }
		public int TimeForCompletion { get; set; }
	}
}
