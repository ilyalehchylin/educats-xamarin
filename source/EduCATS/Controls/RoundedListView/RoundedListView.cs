using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Controls.RoundedListView
{
	public class RoundedListView : ListView
	{
		const double _spacing = 0;
		const double _padding = 0;
		const double _roundedHeaderHeight = 14;

		readonly double _cornerRadius;

		public RoundedListView(RoundedListTemplateSelector templateSelector, View header = null)
		{
			HasUnevenRows = true;
			ItemTemplate = templateSelector;
			SeparatorVisibility = SeparatorVisibility.None;
			VerticalScrollBarVisibility = ScrollBarVisibility.Never;
			HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);

			_cornerRadius = _roundedHeaderHeight / 2;

			Footer = createFooterCap();
			Header = createHeader(header);
		}

		StackLayout createHeader(View view)
		{
			var cap = createHeaderCap();

			var header = new StackLayout {
				Padding = _padding,
				Spacing = _spacing
			};

			if (view != null) {
				header.Children.Add(view);
			}

			header.Children.Add(cap);
			return header;
		}

		Grid createHeaderCap()
		{
			var stackLayout = new StackLayout {
				HeightRequest = _cornerRadius,
				VerticalOptions = LayoutOptions.End,
				BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor)
			};

			var frame = new Frame {
				HasShadow = false,
				CornerRadius = (float)_cornerRadius,
				BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor),
			};

			return new Grid {
				HeightRequest = _roundedHeaderHeight,
				Children = {
					stackLayout,
					frame
				}
			};
		}

		Grid createFooterCap()
		{
			var stackLayout = new StackLayout {
				HeightRequest = _cornerRadius,
				VerticalOptions = LayoutOptions.Start,
				BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor)
			};

			var frame = new Frame {
				HasShadow = false,
				CornerRadius = (float)_cornerRadius,
				BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor)
			};

			return new Grid {
				HeightRequest = _roundedHeaderHeight,
				Children = {
					stackLayout,
					frame
				}
			};
		}
	}
}
