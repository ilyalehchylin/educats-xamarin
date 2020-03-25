using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Controls.RoundedListView
{
	/// <summary>
	/// Rounded list view.
	/// </summary>
	public class RoundedListView : ListView
	{
		/// <summary>
		/// Base spacing.
		/// </summary>
		const double _spacing = 0;

		/// <summary>
		/// Base padding.
		/// </summary>
		const double _padding = 0;

		/// <summary>
		/// Rounded header height.
		/// </summary>
		public const double HeaderHeight = 14;

		/// <summary>
		/// Corner radius & sharp layout height.
		/// </summary>
		readonly double _capHeight;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="templateSelector">Template selector.</param>
		/// <param name="header">Header view.</param>
		public RoundedListView(RoundedListTemplateSelector templateSelector, View header = null)
		{
			HasUnevenRows = true;
			ItemTemplate = templateSelector;
			SeparatorVisibility = SeparatorVisibility.None;
			VerticalScrollBarVisibility = ScrollBarVisibility.Never;
			HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			RefreshControlColor = Color.FromHex(Theme.Current.BaseActivityIndicatorColor);

			_capHeight = HeaderHeight / 2;

			Footer = createFooterCap();
			Header = createHeader(header);
		}

		/// <summary>
		/// Create header layout.
		/// </summary>
		/// <param name="view">View to add to header.</param>
		/// <returns>Header layout.</returns>
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

		/// <summary>
		/// Create header cap.
		/// </summary>
		/// <returns>Header cap.</returns>
		Grid createHeaderCap()
		{
			var stackLayout = new StackLayout {
				HeightRequest = _capHeight,
				VerticalOptions = LayoutOptions.End,
				BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor)
			};

			var frame = new Frame {
				HasShadow = false,
				CornerRadius = (float)_capHeight,
				BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor),
			};

			return new Grid {
				HeightRequest = HeaderHeight,
				Children = {
					stackLayout,
					frame
				}
			};
		}

		/// <summary>
		/// Create footer cap.
		/// </summary>
		/// <returns>Footer cap.</returns>
		Grid createFooterCap()
		{
			var stackLayout = new StackLayout {
				HeightRequest = _capHeight,
				VerticalOptions = LayoutOptions.Start,
				BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor)
			};

			var frame = new Frame {
				HasShadow = false,
				CornerRadius = (float)_capHeight,
				BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor)
			};

			return new Grid {
				HeightRequest = HeaderHeight,
				Children = {
					stackLayout,
					frame
				}
			};
		}
	}
}
