using System;
using EduCATS.Helpers.Forms.Styles;
using EduCATS.Themes;
using Xamarin.Forms;

namespace EduCATS.Pages.Testing.Base.Views.ViewCells
{
	public class TestingHeaderViewCell : ViewCell
	{
		static readonly Thickness _padding = new Thickness(10);
		static readonly Thickness _commentPadding = new Thickness(12, 10);
		static readonly Thickness _commentMargin = new Thickness(0, 8, 0, 0);
		readonly StackLayout _root;

		public TestingHeaderViewCell()
		{
			var sectionLabel = new Label
			{
				FontAttributes = FontAttributes.Bold,
				VerticalOptions = LayoutOptions.Start,
				TextColor = Color.FromHex(Theme.Current.BaseSectionTextColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Large, true)
			};

			sectionLabel.SetBinding(Label.TextProperty, "SectionName");

			var commentLabel = new Label
			{
				TextColor = Color.FromHex(Theme.Current.TestingDescriptionColor),
				Style = AppStyles.GetLabelStyle(NamedSize.Small),
				LineBreakMode = LineBreakMode.WordWrap
			};

			commentLabel.SetBinding(Label.TextProperty, "Comment");

			var commentFrame = new Frame
			{
				HasShadow = false,
				Padding = _commentPadding,
				Margin = _commentMargin,
				BackgroundColor = Color.FromHex(Theme.Current.BaseBlockColor),
				Content = commentLabel
			};

			commentFrame.SetBinding(VisualElement.IsVisibleProperty, "IsCommentVisible");

			_root = new StackLayout
			{
				BackgroundColor = Color.FromHex(Theme.Current.AppBackgroundColor),
				Padding = _padding,
				Children = {
					sectionLabel,
					commentFrame
				}
			};

			View = _root;
			_root.SizeChanged += (_, __) => updateHeightIfPossible();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			Device.BeginInvokeOnMainThread(updateHeightIfPossible);
		}

		void updateHeightIfPossible()
		{
			if (_root.Width <= 0)
			{
				return;
			}

			var request = _root.Measure(_root.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
			var newHeight = request.Request.Height;
			if (newHeight <= 0)
			{
				return;
			}

			if (Math.Abs(Height - newHeight) > 0.5)
			{
				Height = newHeight;
				ForceUpdateSize();
			}
		}
	}
}
