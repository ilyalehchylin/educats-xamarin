using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Pages.ForgotPassword.ViewModels;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Nyxbull.Plugins.CrossLocalization;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace EduCATS.Pages.ForgotPassword.Views
{
	public class ForgotPasswordPageView : ContentPage
	{

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
		static Thickness _baseSpacing = new Thickness(0, 10, 0, 0);
		static Thickness _showPasswordIconMargin = new(0, 10, 5, 0);

		const double _controlHeight = 50;
		const double _showPasswordIconSize = 30;
		public ForgotPasswordPageView()
		{
			BindingContext = new ForgotPasswordPageViewModel(new PlatformServices());
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor);
			createView();
		}

		void createView()
		{
			var backgroundImage = createBackgroundImage();
			var forgotPasswordForm = createForgotPasswordForm();
			var scrollView = new ScrollView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = new StackLayout
				{
					Children = {
						forgotPasswordForm,
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

		StackLayout createForgotPasswordForm()
		{
			var entryStyle = getEntryStyle();
			var userNameEntry = createUsernameEntry(entryStyle);
			var questionPicker = createSecretQuestionPicker();
			var answerEntry = createAnswerEntry(Style);
			var newpasswordEntry = createNewPasswordGrid(entryStyle);
			var confirmPasswordEntry = createConfirmPasswordGrid(entryStyle);
			var resetPasswordButton = createResetPasswordButton();

			var chekInForm = new StackLayout
			{
				Spacing = _loginFormSpacing,
				Padding = _loginFormPadding,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children =
				{
					userNameEntry,
					questionPicker,
					answerEntry,
					newpasswordEntry,
					confirmPasswordEntry,
					resetPasswordButton,
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
				Placeholder = CrossLocalization.Translate("login_username")
			};

			username.SetBinding(Entry.TextProperty, "UserName");
			return username;
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

		Grid createNewPasswordGrid(Style style)
		{
			var passwordEntry = createNewPasswordEntry(style);
			var showPasswordImage = createShowNewPasswordImage();

			return new Grid
			{
				Children = {
					passwordEntry,
					showPasswordImage
				}
			};
        }

		Entry createNewPasswordEntry(Style style)
		{
			var password = new Entry
			{
				Style = style,
				IsPassword = true,
				ReturnType = ReturnType.Done,
				Margin = _baseSpacing,
				Placeholder = CrossLocalization.Translate("new_password")
			};

			password.SetBinding(Entry.TextProperty, "NewPassword");
			password.SetBinding(Entry.IsPasswordProperty, "IsPasswordHidden");
			return password;
		}

		CachedImage createShowNewPasswordImage()
		{
			var showPasswordImage = new CachedImage
            {
				HeightRequest = _showPasswordIconSize,
				Aspect = Aspect.AspectFit,
				Margin = _showPasswordIconMargin,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile(Theme.Current.LoginShowPasswordImage)
			};

			var showPasswordTapGesture = new TapGestureRecognizer();
			showPasswordTapGesture.SetBinding(TapGestureRecognizer.CommandProperty, "HidePasswordCommand");
			showPasswordImage.GestureRecognizers.Add(showPasswordTapGesture);
			return showPasswordImage;
		}

		Grid createConfirmPasswordGrid(Style style)
        {
			var passwordEntry = createConfirmPasswordEntry(style);
			var showPasswordImage = createShowConfirmPasswordImage();
	
	        return new Grid
	        {
			Children = {
				passwordEntry,
				showPasswordImage
				}
		    };
	    }

		CachedImage createShowConfirmPasswordImage()
		{
			var showPasswordImage = new CachedImage
           {
				HeightRequest = _showPasswordIconSize,
				Aspect = Aspect.AspectFit,
				Margin = _showPasswordIconMargin,
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Source = ImageSource.FromFile(Theme.Current.LoginShowPasswordImage)
			};

			var showPasswordTapGesture = new TapGestureRecognizer();
			showPasswordTapGesture.SetBinding(TapGestureRecognizer.CommandProperty, "HideConfirmPasswordCommand");
			showPasswordImage.GestureRecognizers.Add(showPasswordTapGesture);
			return showPasswordImage;
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
			password.SetBinding(Entry.IsPasswordProperty, "IsConfirmPasswordHidden");
			return password;
		}

		Button createResetPasswordButton()
		{
			var chekInButton = new Button
			{
				Text = CrossLocalization.Translate("reset_password"),
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.FromHex(Theme.Current.LoginButtonTextColor),
				BackgroundColor = Color.FromHex(Theme.Current.LoginButtonBackgroundColor),
				Margin = _baseSpacing,
				HeightRequest = _controlHeight,
				Style = AppStyles.GetButtonStyle(bold: true)
			};
			chekInButton.SetBinding(Button.CommandProperty, "ResetPasswordCommand");
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
