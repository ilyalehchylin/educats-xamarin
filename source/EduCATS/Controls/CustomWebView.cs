using System.Windows.Input;
using Xamarin.Forms;

namespace EduCATS.Controls
{
	public class CustomWebView : WebView
	{
        const string _httpString = "http://";
        const string _httpsString = "https://";

        public static readonly BindableProperty HttpNavigatingCommandProperty =
			BindableProperty.Create(nameof(HttpNavigatingCommand), typeof(ICommand), typeof(CustomWebView), null);

        public ICommand HttpNavigatingCommand {
            get { return (ICommand)GetValue(HttpNavigatingCommandProperty); }
            set { SetValue(HttpNavigatingCommandProperty, value); }
        }

		public CustomWebView()
		{
            Navigating += (s, e) =>
            {
                if (e.Url.StartsWith(_httpString) || e.Url.StartsWith(_httpsString)) {
                    if (HttpNavigatingCommand?.CanExecute(e) ?? false) {
                        HttpNavigatingCommand.Execute(e.Url);
                    }

                    e.Cancel = true;
                }
            };
        }
    }
}
