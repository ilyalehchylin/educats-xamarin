﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using EduCATS.Controls.RoundedListView.Selectors;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.Today.Base.Views.ViewCells;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
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
		/// Corner radius & sharp layout height.
		/// </summary>
		readonly double _capHeight;

		/// <summary>
		/// Empty view.
		/// </summary>
		readonly StackLayout _emptyView;

		/// <summary>
		/// Rounded header height.
		/// </summary>
		public const double HeaderHeight = 14;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type">View cell type.</param>
		/// <param name="checkbox">Is template checkbox.</param>
		/// <param name="header">Header view.</param>
		/// <param name="headerTopPadding">Header top padding.</param>
		/// <param name="footerBottomPadding">Footer bottom padding.</param>
		public RoundedListView(
			Type type,
			bool checkbox = false,
			View header = null,
			double headerTopPadding = 0,
			double footerBottomPadding = 0,
			Func<object> func = null,
			IPlatformServices services = null)
		{
			HasUnevenRows = true;

			if ((type == typeof(SubjectPageViewCell) || type == typeof(CalendarSubjectsViewCell)) && services is not null && services.Preferences.IsLargeFont) HasUnevenRows = false;

			ItemTemplate = func == null ?
				new RoundedListTemplateSelector(type, checkbox) :
				new RoundedListTemplateSelector(func, checkbox);

			SeparatorVisibility = SeparatorVisibility.None;
			VerticalScrollBarVisibility = ScrollBarVisibility.Never;
			HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			RefreshControlColor = Color.FromHex(
				Device.RuntimePlatform == Device.Android ?
					Theme.Current.BaseActivityIndicatorColorAndroid :
					Theme.Current.BaseActivityIndicatorColorIOS);

			_capHeight = HeaderHeight / 2;
			_emptyView = createEmptyView();

			Footer = createFooterCap(footerBottomPadding);
			Header = createHeader(header, headerTopPadding);
		}

		/// <summary>
		/// Create header layout.
		/// </summary>
		/// <param name="view">View to add to header.</param>
		/// <param name="topPadding">Header top padding.</param>
		/// <returns>Header layout.</returns>
		StackLayout createHeader(View view, double topPadding = 0)
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
			header.Children.Add(_emptyView);
			header.Padding = new Thickness(0, topPadding, 0, 0);
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
		/// <param name="bottomPadding">Footer bottom padding.</param>
		/// <returns>Footer cap.</returns>
		Grid createFooterCap(double bottomPadding)
		{
			var stackLayout = new StackLayout {
				HeightRequest = _capHeight,
				VerticalOptions = LayoutOptions.Start,
				BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor)
			};

			var frame = new Frame {
				Padding = new Thickness(0, 0, 0, bottomPadding),
				HasShadow = false,
				CornerRadius = (float)_capHeight,
				BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor)
			};

			return new Grid {
				Padding = new Thickness(0, 0, 0, bottomPadding),
				HeightRequest = HeaderHeight,
				Children = {
					stackLayout,
					frame
				}
			};
		}

		/// <summary>
		/// Create empty view.
		/// </summary>
		/// <returns>Empty view layout.</returns>
		StackLayout createEmptyView()
		{
			return new StackLayout {
				Spacing = _spacing,
				BackgroundColor = Color.FromHex(Theme.Current.RoundedListViewBackgroundColor),
				Children = {
					new Label {
						Style = AppStyles.GetLabelStyle(),
						HorizontalTextAlignment = TextAlignment.Center,
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						Text = CrossLocalization.Translate("base_no_data"),
						TextColor = Color.FromHex(Theme.Current.BaseNoDataTextColor)
					}
				}
			};
		}

		/// <summary>
		/// On <c>ItemsSource</c> property changed.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (!propertyName.Equals("ItemsSource")) {
				return;
			}

			try {
				if (ItemsSource == null || ItemsSource.Cast<object>().Count() == 0) {
					_emptyView.IsVisible = true;
					return;
				}
			} catch (Exception) { }

			_emptyView.IsVisible = false;
		}
	}
}
