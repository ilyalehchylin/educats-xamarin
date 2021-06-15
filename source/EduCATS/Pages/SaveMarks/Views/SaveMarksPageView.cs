using EduCATS.Controls.RoundedListView;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Networking;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.Practicals;
using EduCATS.Pages.SaveMarks.ViewModels;
using EduCATS.Pages.Statistics.Marks.Views.ViewCells;
using EduCATS.Themes;
using Newtonsoft.Json;
using Nyxbull.Plugins.CrossLocalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EduCATS.Pages.SaveMarks.Views
{
	public class SaveMarksPageView : ContentPage 
	{

		public string _title { get; set; }

		static Thickness _padding = new Thickness(10, 1);
		static Thickness _headerPadding = new Thickness(0, 10, 0, 10);

		const double _controlHeight = 50;

		public PlatformServices services = new PlatformServices();

		public VisitingLecturesList groupData;
		public LabsVisitingList labsVisitingList;
		public LabsVisitingList particialVisitingList;

		public SaveMarksPageView(int subjectId, int groupId, string title)
		{
			_title = title;
			var httpContent = new StringContent("", Encoding.UTF8, "application/json");
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			Padding = _padding;
			NavigationPage.SetHasNavigationBar(this, false);
			if (title == CrossLocalization.Translate("stats_page_lectures_visiting"))
			{
				string link = Links.GetLecturesCalendarData + "subjectId=" + subjectId + "&groupId=" + groupId;
				var obj = requestDataAsync(link,httpContent);
				groupData = JsonConvert.DeserializeObject<VisitingLecturesList>(obj.Result.ToString());
				BindingContext = new SaveMarksPageViewModel(new PlatformServices(), subjectId, groupData, groupId, title);
			}
			else if (title == CrossLocalization.Translate("stats_page_labs_visiting"))
			{
				string link = Links.GetLabsCalendarData + "subjectId=" + subjectId + "&groupId=" + groupId;
				var obj = requestDataAsync(link,httpContent);
				labsVisitingList = JsonConvert.DeserializeObject<LabsVisitingList>(obj.Result.ToString());
				BindingContext = new SaveMarksPageViewModel(new PlatformServices(), subjectId, labsVisitingList, groupId, title);
			}
			else if (title == CrossLocalization.Translate("practiсe_visiting"))
			{
				string link = Links.GetParticialsMarks;
				var groupItems = new GroupAndSubjModel();
				groupItems.GroupId = groupId;
				groupItems.SubjectId = subjectId;
				var body = JsonConvert.SerializeObject(groupItems);
				httpContent = new StringContent(body, Encoding.UTF8, "application/json");
				var obj = requestDataAsync(link,httpContent);
				particialVisitingList = JsonConvert.DeserializeObject<LabsVisitingList>(obj.Result.ToString());
				BindingContext = new SaveMarksPageViewModel(new PlatformServices(), subjectId, particialVisitingList, groupId, title);
			}
			createView();
		}

		private async Task<object> requestDataAsync(string link, HttpContent _postContent)
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(services.Preferences.AccessToken);
			if (_title == CrossLocalization.Translate("practiсe_visiting"))
			{
				var responce = client.PostAsync(Servers.EduCatsByAddress + link, _postContent).Result;
				var result = await responce.Content.ReadAsStringAsync();
				return result;
			}
			else
			{
				var responce = client.GetAsync(Servers.EduCatsByAddress + link).Result;
				var result = await responce.Content.ReadAsStringAsync();
				return result;
			}
		}

		void createView()
		{
			var stackLayout = new StackLayout();
			var resultsListView = new RoundedListView(typeof(VisitingPageViewCell));
			var resultsListViewSubGroup = new RoundedListView(typeof(VisitingPageViewCell));
			var saveDate = stackView();
			if (_title == CrossLocalization.Translate("stats_page_lectures_visiting"))
			{
				var dateforLectures = dateLecturesPicker();
				resultsListView = new RoundedListView(typeof(VisitingPageViewCell))
				{
					IsPullToRefreshEnabled = false,
				};
				resultsListView.ItemsSource = groupData.GroupsVisiting;
				resultsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "LecturesMarks");
				stackLayout = new StackLayout
				{
					VerticalOptions = LayoutOptions.Center,
					Padding = _headerPadding,
					Children =
					{
						saveDate,
						dateforLectures,
						resultsListView,
					}
				};
			}
			else if (_title == CrossLocalization.Translate("stats_page_labs_visiting"))
			{
				var dateforLabs = dateLabsPicker();
				var subGroupLabsVisiting = subGroupPicker();
				resultsListViewSubGroup = new RoundedListView(typeof(VisitingPageViewCell))
				{
					IsPullToRefreshEnabled = false,
				};
				resultsListViewSubGroup.ItemsSource = labsVisitingList.Students;
				resultsListViewSubGroup.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "LabsVisitingMarksSubGroup");
				stackLayout = new StackLayout
				{
					VerticalOptions = LayoutOptions.Center,
					Padding = _headerPadding,
					Children =
					{
						saveDate,
						subGroupLabsVisiting,
						dateforLabs,
						resultsListViewSubGroup,
					}
				};
			}
			else if (_title == CrossLocalization.Translate("practiсe_visiting"))
			{
				var dateforPractice = datePractPicker();
				resultsListView = new RoundedListView(typeof(VisitingPageViewCell))
				{
					IsPullToRefreshEnabled = false,
				};
				resultsListView.ItemsSource = particialVisitingList.Students;
				resultsListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, "LecturesMarks");
				stackLayout = new StackLayout
				{
					VerticalOptions = LayoutOptions.Center,
					Padding = _headerPadding,
					Children =
					{
						saveDate,
						dateforPractice,
						resultsListView,
					}
				};
			}
			Content = stackLayout;
		}

		StackLayout stackView()
		{
			
			var save = saveMarksButton();
			var form = new StackLayout
			{
				Padding = _headerPadding,
				VerticalOptions = LayoutOptions.Center,
				Children =
				{
					save, 
				}
			};
			return form;
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

		Picker dateLecturesPicker()
		{
			var datePicker = new Picker
			{
				BackgroundColor = Color.White,
			};
			datePicker.SetBinding(Picker.ItemsSourceProperty, "Date");
			datePicker.SetBinding(Picker.SelectedItemProperty, new Binding("SelectedDate"));
			return datePicker;
		}

		Picker dateLabsPicker()
		{
			var datePicker = new Picker
			{
				BackgroundColor = Color.White,
			};
			datePicker.SetBinding(Picker.ItemsSourceProperty, "DateLabs");
			datePicker.SetBinding(Picker.SelectedItemProperty, new Binding("SelectedLabDate"));
			return datePicker;
		}

		Picker datePractPicker()
		{
			var datePicker = new Picker
			{
				BackgroundColor = Color.White,
			};
			datePicker.SetBinding(Picker.ItemsSourceProperty, "Date");
			datePicker.SetBinding(Picker.SelectedItemProperty, new Binding("SelectedPracDate"));
			return datePicker;
		}

		Button saveMarksButton()
		{
			var saveButton = new Button()
			{
				FontAttributes = FontAttributes.Bold,
				Text = CrossLocalization.Translate("save_marks"),
				TextColor = Color.FromHex(Theme.Current.LoginButtonTextColor),
				BackgroundColor = Color.FromHex(Theme.Current.LoginButtonBackgroundColor),
				HeightRequest = _controlHeight,
				Style = AppStyles.GetButtonStyle(bold: true)
			};
			saveButton.SetBinding(Button.CommandProperty, "SaveMarksCommand");
			return saveButton;
		}

	}
}
