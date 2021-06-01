using EduCATS.Controls.Pickers;
using EduCATS.Controls.RoundedListView;
using EduCATS.Fonts;
using EduCATS.Helpers.Forms;
using EduCATS.Networking;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.LabSchedule;
using EduCATS.Networking.Models.SaveMarks.Practicals;
using EduCATS.Pages.SaveLabsAndPracticeMarks.Views;
using EduCATS.Pages.Statistics.Marks.Views.ViewCells;
using EduCATS.Pages.Statistics.Students.Models;
using EduCATS.Pages.Statistics.Students.Views.ViewCells;
using EduCATS.Themes;
using Newtonsoft.Json;
using Nyxbull.Plugins.CrossLocalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EduCATS.Pages.SaveLabsAndPracticeMarks.ViewModels
{
	public class SavePracticeAndLabsPageView : ContentPage
	{
		static Thickness _padding = new Thickness(10, 1);
		static Thickness _headerPadding = new Thickness(0, 10, 0, 10);

		public PlatformServices services = new PlatformServices();
		public string _title { get; set; }
		public LabsVisitingList practicMarksList;
		public LabsVisitingList labMarksList;

		public SavePracticeAndLabsPageView(string title, int subjectId, int groupId)
		{
			_title = title;
			var httpContent = new StringContent("", Encoding.UTF8, "application/json");
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Padding = _padding;
			NavigationPage.SetHasNavigationBar(this, false);
			if (_title == CrossLocalization.Translate("practice_mark"))
			{
				string link = Links.GetParticialsMarks;
				var groupItems = new GroupAndSubjModel();
				groupItems.groupId = groupId;
				groupItems.subjectId = subjectId;
				var body = JsonConvert.SerializeObject(groupItems);
				httpContent = new StringContent(body, Encoding.UTF8, "application/json");
				var obj = requestDataAsync(link, httpContent);
				practicMarksList = JsonConvert.DeserializeObject<LabsVisitingList>(obj.Result.ToString());
				BindingContext = new SavePracticeAndLabsPageViewModel(new PlatformServices(), subjectId, practicMarksList, groupId, title);
				createViews();
			}
			else if (_title == CrossLocalization.Translate("stats_page_labs_rating"))
			{
				string link = Links.GetLabsCalendarData + "subjectId=" + subjectId + "&groupId=" + groupId;
				var obj = requestDataAsync(link, httpContent);
				labMarksList = JsonConvert.DeserializeObject<LabsVisitingList>(obj.Result.ToString());
				BindingContext = new SavePracticeAndLabsPageViewModel(new PlatformServices(), subjectId, labMarksList, groupId, title);
				createLabsMarks();
			}
			
		}

		void createLabsMarks()
		{
			var stackLayout = new StackLayout();
			var resultsListViewSubGroup = new RoundedListView(typeof(StudentsPageViewCell));
			var subGroup = subGroupPicker();
			resultsListViewSubGroup = new RoundedListView(typeof(StudentsPageViewCell))
			{
				IsPullToRefreshEnabled = false,
			};
			resultsListViewSubGroup.ItemTapped += (sender, e) => ((RoundedListView)sender).SelectedItem = null;
			resultsListViewSubGroup.ItemsSource = labMarksList.Students;
			resultsListViewSubGroup.SetBinding(RoundedListView.SelectedItemProperty, "SelectedItem");
			resultsListViewSubGroup.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "LabsVisitingMarksSubGroup");
			stackLayout = new StackLayout
			{
				VerticalOptions = LayoutOptions.Center,
				Padding = _headerPadding,
				Children =
				{
						subGroup,
						resultsListViewSubGroup,
				}
			};

			Content = stackLayout;
		}

		void createViews()
		{
			var roundedListView = createRoundedListView();
			Content = roundedListView;
		}

		private async Task<object> requestDataAsync(string link, HttpContent _postContent)
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(services.Preferences.AccessToken);
			if (_title == CrossLocalization.Translate("practice_mark"))
			{
				var responce = client.PostAsync("https://educats.by" + link, _postContent).Result;
				var result = await responce.Content.ReadAsStringAsync();
				return result;
			}
			else
			{
				var responce = client.GetAsync("https://educats.by" + link).Result;
				var result = await responce.Content.ReadAsStringAsync();
				return result;
			}
		}

		Picker subGroupPicker()
		{
			var subGroupPicker = new Picker
			{
				BackgroundColor = Color.White,
			};
			subGroupPicker.SetBinding(Picker.ItemsSourceProperty, "SubGroup");
			subGroupPicker.SetBinding(Picker.SelectedItemProperty, new Binding("SelectedSubGroup"));
			return subGroupPicker;
		}

		RoundedListView createRoundedListView()
		{
			var roundedListView = new RoundedListView(typeof(StudentsPageViewCell))
			{
				IsPullToRefreshEnabled = false
			};
			roundedListView.ItemTapped += (sender, e) => ((RoundedListView)sender).SelectedItem = null;
			roundedListView.SetBinding(RoundedListView.SelectedItemProperty, "SelectedItem");
			roundedListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "Students");
			return roundedListView;
		}
	}
}
