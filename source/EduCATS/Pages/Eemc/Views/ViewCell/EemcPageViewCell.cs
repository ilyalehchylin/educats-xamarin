using EduCATS.Data.Models.Eemc;
using EduCATS.Themes;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace EduCATS.Pages.Eemc.Views.ViewCell
{
	public class EemcPageViewCell : ContentView
	{
		const string _testString = "test";

		readonly CachedImage _icon;

		bool _isPublished;

		public EemcPageViewCell()
		{
			_icon = new CachedImage {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = 30
			};

			var title = new Label {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				HorizontalTextAlignment = TextAlignment.Center
			};

			title.SetBinding(Label.TextProperty, "Name");

			Content = new StackLayout {
				Margin = new Thickness(15),
				Children = {
					_icon,
					title
				}
			};
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (!(BindingContext is ConceptModel)) {
				return;
			}

			var concept = BindingContext as ConceptModel;
			_isPublished = concept.Published;

			if (concept.IsGroup) {
				setIcon(Theme.Current.EemcDirectoryActiveIcon, Theme.Current.EemcDirectoryInactiveIcon);
				return;
			}

			if (!concept.Container.Equals(_testString)) {
				setIcon(Theme.Current.EemcDocumentActiveIcon, Theme.Current.EemcDocumentInactiveIcon);
				return;
			}

			setIcon(Theme.Current.EemcDocumentTestActiveIcon, Theme.Current.EemcDocumentTestInactiveIcon);
		}

		void setIcon(string publishedIcon, string unpublishedIcon)
		{
			_icon.Source = _isPublished ?
				ImageSource.FromFile(publishedIcon) :
				ImageSource.FromFile(unpublishedIcon);
		}
	}
}
