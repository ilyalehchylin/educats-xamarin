using EduCATS.Pages.Testing.Passing.Models;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Passing.Views.ViewCells
{
	public class TestingAnswerDataTemplateSelector : DataTemplateSelector
	{
		public DataTemplate SingleTemplate { get; set; }
		public DataTemplate EditableTemplate { get; set; }
		public DataTemplate MultipleTemplate { get; set; }
		public DataTemplate MovableTemplate { get; set; }

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			var testAnswerModel = (TestPassingAnswerModel)item;

			switch (testAnswerModel.QuestionType) {
				case 1:
					return MultipleTemplate;
				case 2:
					return EditableTemplate;
				case 3:
					return MovableTemplate;
				default:
					return SingleTemplate;
			}
		}
	}
}
