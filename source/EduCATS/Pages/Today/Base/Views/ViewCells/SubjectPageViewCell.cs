﻿using EduCATS.Helpers.Forms.Converters;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Themes;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace EduCATS.Pages.Today.Base.Views.ViewCells
{
	class SubjectPageViewCell : ViewCell
	{
		const double _boxViewSize = 10;
		const double _boxViewLayoutSize = 20;
		const double _clockIconSize = 20;
		double _boxViewSizeAngle = _boxViewSize / 2;
		double _boxViewSizeRotate = 0;

		static Thickness _paddingTeacher = new Thickness(10	, 0);
		static Thickness _paddingNote = new Thickness(20, 0);
		static Thickness _framePadding = new Thickness(10, 5, 10, 10);
		static Thickness _paddingItem = new Thickness(25, 0);

		public SubjectPageViewCell()
		{
			var clockIcon = new CachedImage
			{
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = Xamarin.Forms.ImageSource.FromFile(Theme.Current.TodayNewsDateIcon),
				HeightRequest = _clockIconSize,
				Transformations = new List<ITransformation> {
					new TintTransformation {
						EnableSolidColor = true,
						HexColor = Theme.Current.TodayNewsDateColor
					}
				}
			};

			var date = new Label
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.TodayNewsDateColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Small)
			};

			date.SetBinding(Label.TextProperty, "Date");

			var address = new Label
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.TodayNewsDateColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Micro)
			};

			address.SetBinding(Label.TextProperty, "Address");

			var dateLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					clockIcon,
					date,
					address
				}
			};
			var subjectBoxView = new BoxView
			{
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = _boxViewSize,
				WidthRequest = _boxViewSize,
				CornerRadius = _boxViewSizeAngle,
			};
			subjectBoxView.SetBinding(
				BoxView.ColorProperty, "Color", converter: new StringToColorConverter());

			var boxViewLayout = new StackLayout
			{
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = _boxViewLayoutSize,
				WidthRequest = _boxViewLayoutSize,
				Children = {
					subjectBoxView
				}
			};

			var subject = new Label
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.TodayNewsTitleColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Small)
			};

			subject.SetBinding(Label.TextProperty, "Name");

			var subjectLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					boxViewLayout,
					subject
				}
			};

			var type = new Label
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.TodayNewsDateColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Micro)
			};

			type.SetBinding(Label.TextProperty, "Type");

			var teacher = new Label
			{
				Padding = _paddingTeacher,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.TodayNewsDateColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Micro)
			};

			teacher.SetBinding(Label.TextProperty, "TeacherFullName");

			var informationLayout = new StackLayout
			{
				Padding = _paddingItem,
				Orientation = StackOrientation.Horizontal,
				Children = {
					type,
					teacher
				}
			};	
			var note = new Label
			{
				Padding = _paddingItem,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.TodayNewsDateColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Micro)
			};

			note.SetBinding(Label.TextProperty, "Note");

			View = new StackLayout {
				Padding = _framePadding,
				BackgroundColor = Color.FromHex(Theme.Current.TodayNewsItemBackgroundColor),
				Children = {
					dateLayout,
					subjectLayout,		
					informationLayout,
					note,

				}
			};
		}
	}
}
