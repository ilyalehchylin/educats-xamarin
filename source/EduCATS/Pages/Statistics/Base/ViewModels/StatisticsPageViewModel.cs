using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Pages.Utils.ViewModels;

namespace EduCATS.Pages.Statistics.Base.ViewModels
{
	public class StatisticsPageViewModel : SubjectsViewModel
	{
		public StatisticsPageViewModel(IDialogs dialogService) : base(dialogService)
		{
			SubjectChanged += (id, name) => {
				System.Diagnostics.Debug.WriteLine($"id: {id}, name: {name}");
			};
		}
	}
}