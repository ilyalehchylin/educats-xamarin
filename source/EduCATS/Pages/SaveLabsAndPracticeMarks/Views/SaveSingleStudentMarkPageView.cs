using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.LabSchedule;
using EduCATS.Pages.SaveLabsAndPracticeMarks.ViewModels;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace EduCATS.Pages.SaveLabsAndPracticeMarks.Views
{
	public class SaveSingleStudentMarkPageView : ContentPage
	{
		static Thickness _gridPadding = new Thickness(15);
		const double _controlHeight = 50;
		static Thickness _padding = new Thickness(10, 1);
		public List<int> Marks = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
		public List<string> NameOfLabOrPractice = new List<string>();
		public string _title { get; set; }

		public SaveSingleStudentMarkPageView(string title, string name, LabsVisitingList marks, TakedLabs prOrLabStat, int subGruop)
		{
			_title = title;

			if(title == CrossLocalization.Translate("practice_mark"))
			{
				foreach(var pract in prOrLabStat.Practicals)
				{
					NameOfLabOrPractice.Add(pract.ShortName);
				}
				BindingContext = new SaveSingleStudentMarkPageViewModel(new PlatformServices(), 
					NameOfLabOrPractice.FirstOrDefault(), marks, prOrLabStat, title, name, subGruop);
			}
			else if (title == CrossLocalization.Translate("stats_page_labs_rating"))
			{
				foreach (var lab in prOrLabStat.Labs)
				{
					if (lab.SubGroup == subGruop)
					{
						NameOfLabOrPractice.Add(lab.ShortName);
					}
				}
				BindingContext = new SaveSingleStudentMarkPageViewModel(new PlatformServices(),
						NameOfLabOrPractice.FirstOrDefault(), marks, prOrLabStat, title, name, subGruop);
			}
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Padding = _padding;
			NavigationPage.SetHasNavigationBar(this, false);
			var entryStyle = getEntryStyle();
			var inicials = new Label
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsTitleColor),
				Style = AppStyles.GetLabelStyle(),
				Text = name,
			};

			var nameOfPrOrLb = new Picker
			{
				BackgroundColor = Color.White,
				HeightRequest = 50,
				ItemsSource = NameOfLabOrPractice,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			nameOfPrOrLb.SetBinding(Picker.SelectedItemProperty, "SelectedShortName");

			var markLabel = new Label
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsTitleColor),
				Style = AppStyles.GetLabelStyle(),
				Text = CrossLocalization.Translate("mark"),
			};

			var dateLabel = new Label
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.FromHex(Theme.Current.StatisticsDetailsTitleColor),
				Style = AppStyles.GetLabelStyle(),
				Text = CrossLocalization.Translate("date"),
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
				HeightRequest = 50,
				ItemsSource = Marks,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			markPicker.SetBinding(Picker.SelectedItemProperty, "Mark");

			var datePicker = new Entry
			{
				Style = entryStyle,
				ReturnType = ReturnType.Done,
				Text = DateTime.Today.ToString("dd.MM.yyyy"),
				TextColor = Color.Black,
				IsReadOnly = true,
			};

			var commentEntry = new Entry
			{
				Style = entryStyle,
				ReturnType = ReturnType.Done,
			};

			commentEntry.SetBinding(Entry.TextProperty, "Comment");

			var gridLayout = new Grid
			{
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Padding = _gridPadding,
				VerticalOptions = LayoutOptions.Start,
			};

			var saveBut = new Button
			{
				FontAttributes = FontAttributes.Bold,
				Text = CrossLocalization.Translate("save_marks"),
				TextColor = Color.FromHex(Theme.Current.LoginButtonTextColor),
				BackgroundColor = Color.FromHex(Theme.Current.LoginButtonBackgroundColor),
				HeightRequest = _controlHeight,
				Style = AppStyles.GetButtonStyle(bold: true)
			};

			saveBut.SetBinding(Button.CommandProperty, "SaveMarksButton");

			gridLayout.Children.Add(inicials, 0, 0);
			gridLayout.Children.Add(nameOfPrOrLb, 0, 1);
			gridLayout.Children.Add(markLabel, 0, 2);
			gridLayout.Children.Add(markPicker, 2, 2);
			gridLayout.Children.Add(dateLabel, 0, 3);
			gridLayout.Children.Add(datePicker, 2, 3);
			gridLayout.Children.Add(commentLabel, 0, 4);
			gridLayout.Children.Add(commentEntry, 2, 4);
			gridLayout.Children.Add(showCommentLabel, 0, 5);
			gridLayout.Children.Add(showComment, 2, 5);
			gridLayout.Children.Add(saveBut, 0, 6);
			Grid.SetColumnSpan(inicials, 3);
			Grid.SetColumnSpan(showCommentLabel, 2);
			Grid.SetColumnSpan(commentLabel, 2);
			Grid.SetColumnSpan(markLabel, 2);
			Grid.SetColumnSpan(saveBut, 3);
			Grid.SetColumnSpan(nameOfPrOrLb, 3);
			Content = gridLayout;
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
