using EduCATS.Fonts;
using Xamarin.Forms;

namespace EduCATS.Helpers.Styles
{
	public static class AppStyles
	{
		public static Style GetEntryStyle(NamedSize size = NamedSize.Medium)
		{
			return new Style(typeof(Entry)) {
				Setters = {
					new Setter {
						Property = Entry.FontSizeProperty,
						Value = FontSizeController.GetSize(size, typeof(Entry))
					},

					new Setter {
						Property = Entry.FontFamilyProperty,
						Value = FontsController.GetCurrentFont()
					}
				}
			};
		}

		public static Style GetButtonStyle(NamedSize size = NamedSize.Medium)
		{
			return new Style(typeof(Button)) {
				Setters = {
					new Setter {
						Property = Button.FontSizeProperty,
						Value = FontSizeController.GetSize(size, typeof(Button))
					},

					new Setter {
						Property = Button.FontFamilyProperty,
						Value = FontsController.GetCurrentFont()
					}
				}
			};
		}

		public static Style GetLabelStyle(NamedSize size = NamedSize.Medium)
		{
			return new Style(typeof(Label)) {
				Setters = {
					new Setter {
						Property = Label.FontSizeProperty,
						Value = FontSizeController.GetSize(size, typeof(Label))
					},

					new Setter {
						Property = Label.FontFamilyProperty,
						Value = FontsController.GetCurrentFont()
					}
				}
			};
		}
	}
}
