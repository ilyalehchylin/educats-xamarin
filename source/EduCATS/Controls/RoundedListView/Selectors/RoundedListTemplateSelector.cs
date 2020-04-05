using System;
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
		/// Get template selector.
		/// </summary>
		/// <param name="type">View cell type.</param>
		/// <param name="checkbox">Is template checkbox.</param>
		public RoundedListTemplateSelector(Type type, bool checkbox)
		{
			var dataTemplate = new DataTemplate(type);
			setTemplate(dataTemplate, checkbox);
		}

		/// <summary>
		/// Get template selector.
		/// </summary>
		/// <param name="func">Func.</param>
		/// <param name="checkbox">Is template checkbox.</param>
		public RoundedListTemplateSelector(Func<object> func, bool checkbox)
		{
			var dataTemplate = new DataTemplate(func);
			setTemplate(dataTemplate, checkbox);
		}

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

		/// <summary>
		/// Set template.
		/// </summary>
		/// <param name="dataTemplate">Data template.</param>
		/// <param name="checkbox">Is template checkbox.</param>
		void setTemplate(DataTemplate dataTemplate, bool checkbox)
		{
			if (checkbox) {
				CheckboxTemplate = dataTemplate;
			} else {
				NavigationTemplate = dataTemplate;
			}
		}
	}
}
