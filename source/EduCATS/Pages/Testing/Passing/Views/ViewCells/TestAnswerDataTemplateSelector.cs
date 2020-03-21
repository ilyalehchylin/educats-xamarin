using EduCATS.Pages.Testing.Passing.Models;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Passing.Views.ViewCells
{
	public class TestAnswerDataTemplateSelector : DataTemplateSelector
	{
		public DataTemplate SingleTemplate { get; set; }
		public DataTemplate EditableTemplate { get; set; }
		public DataTemplate MultipleTemplate { get; set; }
		public DataTemplate MovableTemplate { get; set; }

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			var testAnswerModel = (TestPassingAnswerModel)item;

			return testAnswerModel.QuestionType switch
			{
				1 => MultipleTemplate,
				2 => EditableTemplate,
				3 => MovableTemplate,
				_ => SingleTemplate,
			};
		}
	}
}
