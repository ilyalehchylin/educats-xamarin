using EduCATS.Data.Models.News;
using EduCATS.Helpers.Date;
using Xamarin.Forms;

namespace EduCATS.Pages.Today.Base.Models
{
	public class NewsPageModel
	{
		public NewsPageModel(NewsItemModel newsModel, string subjectColor)
		{
			setDefaultProps(newsModel);
			setSubjectColor(subjectColor);
		}

		void setDefaultProps(NewsItemModel newsModel)
		{
			if (newsModel != null) {
				Id = newsModel.Id;
				Title = newsModel.Title;
				Body = newsModel.Body;
				Date = newsModel.EditDate;

				if (newsModel.Subject != null) {
					SubjectName = newsModel.Subject.Name;
				}
			}
		}

		void setSubjectColor(string subjectColor)
		{
			if (subjectColor != null) {
				SubjectColor = subjectColor;
			}
		}

		public int Id { get; set; }

		public string Title { get; set; }

		public string Body { get; set; }

		string date;
		public string Date {
			get {
				var unixDoble = DateHelper.GetUnixFromString(date);
				var unixString = DateHelper.Convert13DigitsUnixToDateTime(unixDoble);
				return unixString.ToString("dd-MM-yyyy hh:mm");
			}
			set {
				date = value;
			}
		}

		public string SubjectName { get; set; }

		public string SubjectColor { get; set; }
	}
}