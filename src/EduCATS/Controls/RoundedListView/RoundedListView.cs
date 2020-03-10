using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Controls.RoundedListView
{
	public class RoundedListView : ListView
	{
		const double roundedHeaderHeight = 14;

		public RoundedListView(RoundedListTemplateSelector templateSelector, View header = null)
		{
			HasUnevenRows = true;
			SeparatorVisibility = SeparatorVisibility.None;
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			ItemTemplate = templateSelector;

			var headerGrid = new Grid {
				HeightRequest = roundedHeaderHeight,
				Children = {
					new StackLayout {
						HeightRequest = roundedHeaderHeight / 2,
						BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor),
						VerticalOptions = LayoutOptions.End
					},

					new Frame {
						HasShadow = false,
						BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor),
						CornerRadius = (float)roundedHeaderHeight / 2
					}
				}
			};

			var headerView = new StackLayout {
				Padding = 0,
				Spacing = 0
			};

			if (header != null) {
				headerView.Children.Add(header);
			}

			headerView.Children.Add(headerGrid);

			Header = headerView;

			Footer = new Grid {
				HeightRequest = roundedHeaderHeight,
				Children = {
					new StackLayout {
						VerticalOptions = LayoutOptions.Start,
						HeightRequest = roundedHeaderHeight / 2,
						BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor)
					},

					new Frame {
						HasShadow = false,
						BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor),
						CornerRadius = (float)roundedHeaderHeight / 2
					}
				}
			};
		}
	}
}