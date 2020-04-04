using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;
using EduCATS.Data.Models;

namespace EduCATS.Pages.Recommendations.Models
{
	public class RecommendationsPageModel : IRoundedListType
	{
		public RecommendationsPageModel(RecommendationModel model)
		{
			if (model == null) {
				return;
			}

			Id = model.Id;
			Text = model.Text;
			IsTest = model.IsTest;
		}

		public int Id { get; set; }

		public string Text { get; set; }

		public bool IsTest { get; set; }


		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
