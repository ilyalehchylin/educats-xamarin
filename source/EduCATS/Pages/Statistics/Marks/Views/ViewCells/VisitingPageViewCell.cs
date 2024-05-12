﻿using EduCATS.Helpers.Forms.Styles;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EduCATS.Pages.Statistics.Marks.Views.ViewCells
{
	public class VisitingPageViewCell : ViewCell
	{
		static Thickness _gridPadding = new Thickness(15);

		const double _controlHeight = 50;

		public List<string> listOfMarks = new List<string> { "1", "2", "3", "4" };

		public BindableProperty HeightRequestProperty { get; private set; }
		public BindableProperty BackgroundColorProperty { get; private set; }

		public VisitingPageViewCell()
		{
			var entryStyle = getEntryStyle();
			var inicials = new Label
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsTitleColor),
				Style = AppStyles.GetLabelStyle()
			};

			inicials.SetBinding(Label.TextProperty, "Title");
			inicials.SetBinding(VisualElement.IsVisibleProperty, "IsTitle");

			var markLabel = new Label
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsTitleColor),
				Style = AppStyles.GetLabelStyle(),
				Text = CrossLocalization.Translate("skipped_hours"),
			};

			var commentLabel = new Label
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsTitleColor),
				Style = AppStyles.GetLabelStyle(),
				Text = CrossLocalization.Translate("comment"),
			};

			var showCommentLabel = new Label
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsTitleColor),
				Style = AppStyles.GetLabelStyle(),
				HeightRequest = 60,
				Text = CrossLocalization.Translate("show_for_student"),
			};

			var showComment = new Switch
			{
				IsToggled = false,
			};

			showComment.SetBinding(Switch.IsToggledProperty, "ShowForStud");

			var markPicker = new Picker
			{
				BackgroundColor = Color.White,
				HeightRequest = 40,
				ItemsSource = listOfMarks,
				HorizontalTextAlignment = TextAlignment.Center
			};
			markPicker.SetBinding(Picker.SelectedIndexProperty, "Mark");
			markPicker.SetBinding(Picker.SelectedItemProperty, "Mark");

			var commentEntry = new Entry
			{
				Style = entryStyle,
				ReturnType = ReturnType.Done,
				HeightRequest = 40
			};

			commentEntry.SetBinding(Entry.TextProperty, "Comment");

			var gridLayout = new Grid
			{
				Padding = _gridPadding,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
			gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
			gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Auto) });
			gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Auto) });
			gridLayout.Children.Add(inicials, 0, 0);
			gridLayout.Children.Add(markLabel, 0, 1);
			gridLayout.Children.Add(markPicker, 2, 1);
			gridLayout.Children.Add(commentLabel, 0, 2);
			gridLayout.Children.Add(commentEntry, 2, 2);
			gridLayout.Children.Add(showCommentLabel, 0, 3);
			gridLayout.Children.Add(showComment, 2, 3);
			Grid.SetColumnSpan(inicials, 3);
			Grid.SetColumnSpan(showCommentLabel, 2);
			Grid.SetColumnSpan(commentLabel, 2);
			Grid.SetColumnSpan(markLabel, 2);

			View = new StackLayout
			{
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Children = {
					gridLayout
				}
			};


		}

		Style getEntryStyle()
		{
			var style = AppStyles.GetEntryStyle();

			style.Setters.Add(new Setter
			{
				Property = HeightRequestProperty,
				Value = _controlHeight
			});

			style.Setters.Add(new Setter
			{
				Property = BackgroundColorProperty,
				Value = Theme.Current.LoginEntryBackgroundColor
			});

			return style;
		}
	}
}
