using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;
using Xamarin.Forms;

namespace EduCATS.Controls.RoundedListView.Selectors
{
	public class RoundedListTemplateSelector : DataTemplateSelector
	{
		public DataTemplate NavigationTemplate;
		public DataTemplate CheckboxTemplate;

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			var listType = ((IRoundedListType)item).GetListType();

			switch (listType) {
				case RoundedListTypeEnum.Checkbox:
					return CheckboxTemplate;
				case RoundedListTypeEnum.Navigation:
					return NavigationTemplate;
			}

			return null;
		}
	}
}