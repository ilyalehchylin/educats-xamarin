using EduCATS.Fonts;
using Xamarin.Forms;

namespace EduCATS.Helpers.Styles
{
	public static class AppStyles
	{
		public static Style GetEntryStyle(NamedSize size = NamedSize.Medium, bool bold = false)
		{
			return new Style(typeof(Entry)) {
				Setters = {
					getSetter(Entry.FontSizeProperty, FontSizeController.GetSize(size, typeof(Entry))),
					getSetter(Entry.FontFamilyProperty, FontsController.GetCurrentFont(bold))
				}
			};
		}

		public static Style GetButtonStyle(NamedSize size = NamedSize.Medium, bool bold = false)
		{
			return new Style(typeof(Button)) {
				Setters = {
					getSetter(Button.FontSizeProperty, FontSizeController.GetSize(size, typeof(Button))),
					getSetter(Button.FontFamilyProperty, FontsController.GetCurrentFont(bold))
				}
			};
		}

		public static Style GetLabelStyle(NamedSize size = NamedSize.Medium, bool bold = false)
		{
			return new Style(typeof(Label)) {
				Setters = {
					getSetter(Label.FontSizeProperty, FontSizeController.GetSize(size, typeof(Label))),
					getSetter(Label.FontFamilyProperty, FontsController.GetCurrentFont(bold))
				}
			};
		}

		static Setter getSetter(BindableProperty property, object value)
		{
			return new Setter {
				Property = property,
				Value = value
			};
		}
	}
}
