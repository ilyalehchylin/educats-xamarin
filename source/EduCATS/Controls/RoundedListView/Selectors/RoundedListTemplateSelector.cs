using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;
using Xamarin.Forms;

namespace EduCATS.Controls.RoundedListView.Selectors
{
	/// <summary>
	/// Rounded list template selector.
	/// </summary>
	public class RoundedListTemplateSelector : DataTemplateSelector
	{
		/// <summary>
		/// Checkbox template.
		/// </summary>
		public DataTemplate CheckboxTemplate;

		/// <summary>
		/// Navigation template.
		/// </summary>
		public DataTemplate NavigationTemplate;

		/// <summary>
		/// Template selection overriding.
		/// </summary>
		/// <param name="item">Selected object.</param>
		/// <param name="container">Bindable object.</param>
		/// <returns>Template.</returns>
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
