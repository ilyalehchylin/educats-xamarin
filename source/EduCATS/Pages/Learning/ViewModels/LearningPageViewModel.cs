using System;
using System.Collections.Generic;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Pages.Learning.Models;
using EduCATS.Themes;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Pages.Learning.ViewModels
{
	/// <summary>
	/// Learning page view model.
	/// </summary>
	public class LearningPageViewModel : ViewModel
	{
		readonly IPlatformServices _services;

		public LearningPageViewModel(IPlatformServices services)
		{
			_services = services;
			setCardList();
		}

		List<LearningPageModel> _cardsList;

		/// <summary>
		/// Cards list.
		/// </summary>
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

		/// <summary>
		/// Set cards list.
		/// </summary>
		void setCardList()
		{
			try {
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
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		/// <summary>
		/// Get card.
		/// </summary>
		/// <param name="title">Card title.</param>
		/// <param name="image">Card image.</param>
		/// <param name="id">Card ID.</param>
		/// <returns></returns>
		LearningPageModel getCard(string title, string image, int id)
		{
			return new LearningPageModel {
				Id = id,
				Title = title,
				Image = image
			};
		}

		/// <summary>
		/// Open page.
		/// </summary>
		/// <param name="selectedObject">Selected object.</param>
		void openPage(object selectedObject)
		{
			try {
				if (selectedObject == null || !selectedObject.GetType().Equals(typeof(LearningPageModel))) {
					return;
				}

				var page = selectedObject as LearningPageModel;
				SelectedItem = null;
				openPageById(page.Id);
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		/// <summary>
		/// Open page by ID.
		/// </summary>
		/// <param name="id">Card ID.</param>
		void openPageById(int id)
		{
			switch (id) {
				case 0:
					_services.Navigation.OpenTesting(
						CrossLocalization.Translate("learning_card_tests"));
					break;
				case 1:
					_services.Navigation.OpenEemc(
						CrossLocalization.Translate("learning_card_eemc"));
					break;
				case 2:
					_services.Navigation.OpenFiles(
						CrossLocalization.Translate("learning_card_files"));
					break;
				case 3:
					_services.Navigation.OpenRecommendations(
						CrossLocalization.Translate("learning_card_adaptive"));
					break;
			}
		}
	}
}
