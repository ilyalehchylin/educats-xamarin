using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models.Eemc;
using EduCATS.Data.User;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Networking;
using EduCATS.Pages.Pickers;
using Xamarin.Forms;

namespace EduCATS.Pages.Eemc.ViewModels
{
	public class EemcPageViewModel : SubjectsViewModel
	{
		const string _testString = "test";

		readonly int _searchId;
		readonly IPages _navigation;
		readonly Stack<ConceptModel> _previousConcepts;

		int _rootId;
		ConceptModel _backupRootConceptsWithChildren;
		List<ConceptModel> _backupRootConceptsWithoutChildren;

		public EemcPageViewModel(
			IDialogs dialogs, IDevice device,
			IPages navigation, int searchId) : base(dialogs, device)
		{
			IsRoot = true;
			_navigation = navigation;
			_searchId = searchId;
			_previousConcepts = new Stack<ConceptModel>();

			Task.Run(async () => await update());
			SubjectChanged += async (id, name) => await update();
		}

		List<ConceptModel> _concepts;
		public List<ConceptModel> Concepts {
			get { return _concepts; }
			set { SetProperty(ref _concepts, value); }
		}

		bool _isBackActionPossible;
		public bool IsBackActionPossible {
			get { return _isBackActionPossible; }
			set { SetProperty(ref _isBackActionPossible, value); }
		}

		bool _isRoot;
		public bool IsRoot {
			get { return _isRoot; }
			set { SetProperty(ref _isRoot, value); }
		}

		object _selectedItem;
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);
				Task.Run(async () => await openConcepts(_selectedItem));
			}
		}

		Command _backCommand;
		public Command BackCommand {
			get {
				return _backCommand ?? (_backCommand = new Command(goBack));
			}
		}

		async Task update()
		{
			await SetupSubjects();
			await setRootConcepts();

			if (_searchId == -1 || _backupRootConceptsWithoutChildren == null) {
				return;
			}

			await setConceptsFromRoot(_backupRootConceptsWithoutChildren[0].Id);
			searchForBook(_searchId);
			IsBackActionPossible = false;
		}

		async Task setRootConcepts()
		{
			var userId = AppUserData.UserId.ToString();
			var subjectId = CurrentSubject.Id.ToString();
			var root = await DataAccess.GetRootConcepts(userId, subjectId);

			if (DataAccess.IsError && !DataAccess.IsConnectionError) {
				DialogService.ShowError(DataAccess.ErrorMessage);
			}

			var rootConcepts = root?.Concepts;

			if (rootConcepts != null && rootConcepts.Count > 0) {
				_backupRootConceptsWithoutChildren = new List<ConceptModel>(rootConcepts);
				Concepts = new List<ConceptModel>(rootConcepts);
			}
		}

		async Task openConcepts(object selectedObject)
		{
			if (selectedObject == null || !(selectedObject is ConceptModel)) {
				return;
			}

			SelectedItem = null;
			var concept = selectedObject as ConceptModel;
			var id = concept.Id;

			if (IsRoot) {
				_rootId = id;
				await setConceptsFromRoot(id);
			} else {
				setOrOpenConcept(concept, id);
			}
		}

		void searchForBook(int id)
		{
			if (Concepts == null) {
				return;
			}

			var concept = getConcept(Concepts, id);

			if (concept != null) {
				openConcept(concept);
			}
		}

		ConceptModel getConcept(List<ConceptModel> concepts, int id)
		{
			var item = concepts.FirstOrDefault(c => c.Id == id);

			if (item != null) {
				return item;
			}

			item = concepts.FirstOrDefault(c => {
				var concept = getConcept(c.Children, id);
				return concept != null;
			});

			return item;
		}

		async Task setConceptsFromRoot(int id)
		{
			var conceptTree = await DataAccess.GetConceptTree(id);

			if (DataAccess.IsError && !DataAccess.IsConnectionError) {
				DialogService.ShowError(DataAccess.ErrorMessage);
			}

			var concepts = conceptTree?.Children;

			if (concepts == null) {
				return;
			}

			IsRoot = false;
			_previousConcepts.Push(conceptTree);
			_backupRootConceptsWithChildren = conceptTree;
			Concepts = new List<ConceptModel>(concepts);
			IsBackActionPossible = true;
		}

		void setOrOpenConcept(ConceptModel selectedConcept, int id)
		{
			if (Concepts == null) {
				return;
			}

			var concept = Concepts.FirstOrDefault(c => c.Id == id);

			if (concept == null) {
				return;
			}

			if (concept.HasData && !string.IsNullOrEmpty(concept.FilePath)) {
				openFile(concept.FilePath);
				return;
			}

			openConcept(concept, selectedConcept);
		}

		void openFile(string filePath)
		{
			DeviceService.MainThread(() => DeviceService.OpenUri($"{Servers.Current}/{filePath}"));
		}

		void openTest(int id)
		{
			DeviceService.MainThread(
				async () => await _navigation.OpenTestPassing(id, true));
		}

		void openConcept(ConceptModel conceptToCheck, ConceptModel conceptToPush = null)
		{
			if (conceptToCheck.IsGroup) {
				_previousConcepts.Push(conceptToPush ?? conceptToCheck);
				Concepts = new List<ConceptModel>(conceptToCheck.Children);
			} else if (conceptToCheck.Container.Equals(_testString)) {
				openTest(conceptToCheck.Id);
			}
		}

		void goBack()
		{
			if (_previousConcepts.Count == 0) {
				return;
			}

			var previousConcept = _previousConcepts.Pop();

			if (previousConcept.Id == _rootId &&
				_backupRootConceptsWithoutChildren != null &&
				_backupRootConceptsWithoutChildren.Count > 0) {
				IsRoot = true;
				IsBackActionPossible = false;
				Concepts = new List<ConceptModel>(_backupRootConceptsWithoutChildren);
				return;
			}

			if (_previousConcepts.Count > 0) {
				var earlierConcept = _previousConcepts.Peek();
				Concepts = new List<ConceptModel>(earlierConcept.Children);
			} else {
				Concepts = new List<ConceptModel>(_backupRootConceptsWithChildren.Children);
			}
		}
	}
}
