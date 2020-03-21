using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;
using Xamarin.Forms;

namespace EduCATS.Controls.RoundedListView.Selectors
{
	public class RoundedListTemplateSelector : DataTemplateSelector
	{
		public DataTemplate CheckboxTemplate;
		public DataTemplate NavigationTemplate;

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			var listType = ((IRoundedListType)item).GetListType();

			return listType switch
			{
				RoundedListTypeEnum.Checkbox => CheckboxTemplate,
				RoundedListTypeEnum.Navigation => NavigationTemplate,
				_ => null,
			};
		}
	}
}
