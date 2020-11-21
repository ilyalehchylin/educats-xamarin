using EduCATS.Data.Models;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.Registration.ViewModels;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Newtonsoft.Json;
using Nyxbull.Plugins.CrossLocalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Xamarin.Forms;

namespace EduCATS.Pages.Registration.Views
{
	public class RegistrationPageView : ContentPage
	{
		public List<GroupItemModel> groupData = new List<GroupItemModel>();
		public List<String> SecretQuestions = new List<String>()
		{
			CrossLocalization.Translate("mother_last_name"),
			CrossLocalization.Translate("pets_name"),
			CrossLocalization.Translate("hobby"),
		};
		const double _loginFormSpacing = 0;
		readonly string[] _backgrounds = {
			Theme.Current.LoginBackground1Image,
			Theme.Current.LoginBackground2Image,
			Theme.Current.LoginBackground3Image,
		};
		static Thickness _loginFormPadding = new Thickness(20, 0);
		static Thickness _buttonsPadding = new Thickness(0, 0, 0, 10);
		static Thickness _baseSpacing = new Thickness(0, 10, 0, 0);

		const double _controlHeight = 50;
		public RegistrationPageView()
		{
			BindingContext = new RegistrationPageViewModel(new PlatformServices());
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			WebRequest request = WebRequest.Create("http://educats.by/Administration/GetGroupsJson");
			WebResponse response = request.GetResponse();
			string json = "";
			using (Stream stream = response.GetResponseStream())
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					string line = "";
					while ((line = reader.ReadLine()) != null)
					{
						json += line;
					}
				}
			};
			groupData = JsonConvert.DeserializeObject<List<GroupItemModel>>(json);
			createView();
		}
		void createView()
		{
			var backgroundImage = createBackgroundImage();
			var chekInForm = createChekInForm();
			var scrollView = new ScrollView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = new StackLayout
				{
					Children = {
						chekInForm,
					}
				}
			};
			Content = new Grid
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {
					backgroundImage,
					scrollView,
				}
			};
		}
		StackLayout createChekInForm()
		{
			var entryStyle = getEntryStyle();
			var userNameEntry = createUsernameEntry(entryStyle);
			var passwordEntry = createPasswordEntry(entryStyle);
			var confirmPasswordEntry = createConfirmPasswordEntry(entryStyle);
			var nameEntry = createFnameEntry(entryStyle);
			var surnameEntry = createSnameEntry(entryStyle);
			var patronymicEntry = createPatronymicnameEntry(entryStyle);
			var groupPicker = createGroupNumberPicker();
			var questionPicker = createSecretQuestionPicker();
			var answerEntry = createAnswerEntry(Style);
			var chekInButton = createCheckInButton();

			var chekInForm = new StackLayout
			{
				Spacing = _loginFormSpacing,
				Padding = _loginFormPadding,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children =
				{
					userNameEntry,
					passwordEntry,
					confirmPasswordEntry,
					nameEntry,
					surnameEntry,
					passwordEntry,
					patronymicEntry,
					groupPicker,
					questionPicker,
					answerEntry,
					chekInButton,
				}
			};
			return chekInForm;
		}
		Entry createUsernameEntry(Style style)
		{
			var username = new Entry
			{
				Style = style,
				ReturnType = ReturnType.Next,
				Margin = _baseSpacing,
				Placeholder = CrossLocalization.Translate("login_username")
			};

			username.SetBinding(Entry.TextProperty, "UserName");
			return username;
		}
		Entry createFnameEntry(Style style)
		{
			var username = new Entry
			{
				Style = style,
				ReturnType = ReturnType.Next,
				Margin = _baseSpacing,
				Placeholder = CrossLocalization.Translate("name")
			};

			username.SetBinding(Entry.TextProperty, "Name");
			return username;
		}
		Entry createSnameEntry(Style style)
		{
			var username = new Entry
			{
				Style = style,
				ReturnType = ReturnType.Next,
				Margin = _baseSpacing,
				Placeholder = CrossLocalization.Translate("surname")
			};

			username.SetBinding(Entry.TextProperty, "Surname");
			return username;
		}

		Entry createPatronymicnameEntry(Style style)
		{
			var username = new Entry
			{
				Style = style,
				ReturnType = ReturnType.Next,
				Margin = _baseSpacing,
				Placeholder = CrossLocalization.Translate("patronymic")
			};

			username.SetBinding(Entry.TextProperty, "Patronymic");
			return username;
		}

		Entry createPasswordEntry(Style style)
		{
			var password = new Entry
			{
				Style = style,
				IsPassword = true,
				ReturnType = ReturnType.Done,
				Margin = _baseSpacing,
				Placeholder = CrossLocalization.Translate("login_password")
			};

			password.SetBinding(Entry.TextProperty, "Password");
			password.SetBinding(Entry.IsPasswordProperty, "IsPasswordHidden");
			return password;
		}

		Entry createConfirmPasswordEntry(Style style)
		{
			var password = new Entry
			{
				Style = style,
				IsPassword = true,
				ReturnType = ReturnType.Done,
				Margin = _baseSpacing,
				Placeholder = CrossLocalization.Translate("confirm_password")
			};

			password.SetBinding(Entry.TextProperty, "ConfirmPassword");
			password.SetBinding(Entry.IsPasswordProperty, "IsPasswordHidden");
			return password;
		}

		Picker createGroupNumberPicker()
		{
			var groups = new Picker()
			{
				BackgroundColor = Color.White,
				Margin = _baseSpacing,
				Title = CrossLocalization.Translate("choose_group"),
				HeightRequest = 50,
				ItemDisplayBinding = new Binding("Name"),
				ItemsSource = groupData,
			};
			groups.SetBinding(Picker.SelectedItemProperty, new Binding("Group"));
			return groups;
		}
		Picker createSecretQuestionPicker()
		{
			var questions = new Picker()
			{
				BackgroundColor = Color.White,
				Margin = _baseSpacing,
				Title = CrossLocalization.Translate("select_secret_question"),
				HeightRequest = 50,
				ItemsSource = SecretQuestions,
			};

			questions.SetBinding(Picker.SelectedItemProperty, "QuestionId");
			return questions;
		}

		Entry createAnswerEntry(Style style)
		{
			var answer = new Entry
			{
				BackgroundColor = Color.White,
				Style = style,
				ReturnType = ReturnType.Next,
				Margin = _baseSpacing,
				HeightRequest = 50,
				Placeholder = CrossLocalization.Translate("answer_to_secret_question")
			};
			answer.SetBinding(Entry.TextProperty, "AnswerToSecretQuestion");
			return answer;
		}

		Button createCheckInButton()
		{
			var chekInButton = new Button
			{
				Text = CrossLocalization.Translate("chek_In"),
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.FromHex(Theme.Current.LoginButtonTextColor),
				BackgroundColor = Color.FromHex(Theme.Current.LoginButtonBackgroundColor),
				Margin = _baseSpacing,
				HeightRequest = _controlHeight,
				Style = AppStyles.GetButtonStyle(bold: true)
			};
			chekInButton.SetBinding(Button.CommandProperty, "RegisterCommand");
			return chekInButton;
		}
		CachedImage createBackgroundImage()
		{
			return new CachedImage
			{
				Aspect = Aspect.AspectFill,
				Source = ImageSource.FromFile(getRandomBackgroundImage())
			};
		}
		string getRandomBackgroundImage()
		{
			var random = new Random();
			var randomBackgroundIndex = random.Next(0, _backgrounds.Length - 1);
			return _backgrounds[randomBackgroundIndex];
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