using System.Collections.Generic;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Pages.Learning.Models;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Pages.Learning.ViewModels
{
	public class LearningPageViewModel : ViewModel
	{
		readonly IPages _navigationService;

		public LearningPageViewModel(IPages navigationService)
		{
			_navigationService = navigationService;
			setCardList();
		}

		List<LearningPageModel> _cardsList;
		public List<LearningPageModel> CardsList {
			get { return _cardsList; }
			set { SetProperty(ref _cardsList, value); }
		}

		object _selectedItem;
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);

				if (_selectedItem != null) {
					openPage(_selectedItem);
				}
			}
		}

		void setCardList()
		{
			CardsList = new List<LearningPageModel> {
				getCard(
					CrossLocalization.Translate("learning_card_tests"),
					Theme.Current.LearningCardTestsImage, 0),

				getCard(
					CrossLocalization.Translate("learning_card_eemc"),
					Theme.Current.LearningCardEemcImage, 1),

				getCard(
					CrossLocalization.Translate("learning_card_files"),
					Theme.Current.LearningCardFilesImage, 2),

				getCard(
					CrossLocalization.Translate("learning_card_adaptive"),
					Theme.Current.LearningCardAdaptiveImage, 3),
			};
		}

		LearningPageModel getCard(string title, string image, int id)
		{
			return new LearningPageModel {
				Id = id,
				Title = title,
				Image = image
			};
		}

		void openPage(object selectedObject)
		{
			if (selectedObject == null || !selectedObject.GetType().Equals(typeof(LearningPageModel))) {
				return;
			}

			var page = selectedObject as LearningPageModel;
			SelectedItem = null;
			openPageById(page.Id);
		}

		void openPageById(int id)
		{
			switch (id) {
				case 0:
					_navigationService.OpenTesting(
						CrossLocalization.Translate("learning_card_tests"));
					break;
				case 1:
					_navigationService.OpenEemc(
						CrossLocalization.Translate("learning_card_eemc"));
					break;
				case 2:
					_navigationService.OpenFiles(
						CrossLocalization.Translate("learning_card_files"));
					break;
				case 3:
					break;
			}
		}
	}
}
