using System.Windows.Input;
using EduCATS.Data.Models.Testing.Passing;

namespace EduCATS.Pages.Testing.Passing.Models
{
	public class TestPassingAnswerModel
	{
		public int Id { get; set; }
		public int QuestionType { get; set; }
		public string Content { get; set; }
		public bool IsSelected { get; set; }
		public int CorrectnessIndicator { get; set; }
		public string ContentToAnswer { get; set; }

		public ICommand UpMovableAnswerCommand { get; set; }
		public ICommand DownMovableAnswerCommand { get; set; }

		public TestPassingAnswerModel(TestQuestionAnswersModel answerModel, int questionType)
		{
			Id = answerModel.Id;
			QuestionType = questionType;
			Content = answerModel.Content;
			IsSelected = false;
			CorrectnessIndicator = answerModel.СorrectnessIndicator;
		}
	}
}
