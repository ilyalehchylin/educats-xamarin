using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Helpers.Pages.Interfaces;
using EduCATS.Networking;
using EduCATS.Pages.Pickers;
using Xamarin.Forms;

namespace EduCATS.Pages.Eemc.ViewModels
{
	/// <summary>
	/// EEMC page view model.
	/// </summary>
	public class EemcPageViewModel : SubjectsViewModel
	{
		/// <summary>
		/// Test identifier string.
		/// </summary>
		const string _testString = "test";

		/// <summary>
		/// Search ID.
		/// </summary>
		readonly int _searchId;

		/// <summary>
		/// Pages navigation.
		/// </summary>
		readonly IPages _navigation;

		/// <summary>
		/// Previous concepts.
		/// </summary>
		readonly Stack<ConceptModel> _previousConcepts;

		/// <summary>
		/// Root ID.
		/// </summary>
		int _rootId;

		/// <summary>
		/// Backup root concepts with children.
		/// </summary>
		ConceptModel _backupRootConceptsWithChildren;

		/// <summary>
		/// Backup root concepts without children.
		/// </summary>
		List<ConceptModel> _backupRootConceptsWithoutChildren;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dialogs">App dialogs.</param>
		/// <param name="device">App device.</param>
		/// <param name="navigation">Pages navigation.</param>
		/// <param name="searchId">Search ID.</param>
		public EemcPageViewModel(
			IDialogs dialogs, IDevice device, IPages navigation, int searchId) : base(dialogs, device)
		{
			IsRoot = true;
			_navigation = navigation;
			_searchId = searchId;
			_previousConcepts = new Stack<ConceptModel>();

			Task.Run(async () => await update());
			SubjectChanged += async (id, name) => await update();
		}

		List<ConceptModel> _concepts;

		/// <summary>
		/// Concepts.
		/// </summary>
		public List<ConceptModel> Concepts {
			get { return _concepts; }
			set { SetProperty(ref _concepts, value); }
		}

		bool _isBackActionPossible;

		/// <summary>
		/// Is back action possible.
		/// </summary>
		public bool IsBackActionPossible {
			get { return _isBackActionPossible; }
			set { SetProperty(ref _isBackActionPossible, value); }
		}

		bool _isRoot;

		/// <summary>
		/// Is root directory.
		/// </summary>
		public bool IsRoot {
			get { return _isRoot; }
			set { SetProperty(ref _isRoot, value); }
		}

		object _selectedItem;

		/// <summary>
		/// Selected item.
		/// </summary>
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);
				Task.Run(async () => await openConcepts(_selectedItem));
			}
		}

		Command _backCommand;

		/// <summary>
		/// Back command.
		/// </summary>
		public Command BackCommand {
			get {
				return _backCommand ?? (_backCommand = new Command(goBack));
			}
		}

		/// <summary>
		/// Refresh data.
		/// </summary>
		/// <returns>Task.</returns>
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

		/// <summary>
		/// Set root concepts.
		/// </summary>
		/// <returns>Task.</returns>
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

		/// <summary>
		/// Open concepts.
		/// </summary>
		/// <param name="selectedObject">Selected object.</param>
		/// <returns>Task.</returns>
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

		/// <summary>
		/// Search for book.
		/// </summary>
		/// <param name="id">Concept ID.</param>
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

		/// <summary>
		/// Get concept.
		/// </summary>
		/// <param name="concepts">Concepts.</param>
		/// <param name="id">Concept ID.</param>
		/// <returns>Concept.</returns>
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

		/// <summary>
		/// Set concepts from root.
		/// </summary>
		/// <param name="id">Element ID.</param>
		/// <returns>Task.</returns>
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

		/// <summary>
		/// Set or open concept.
		/// </summary>
		/// <param name="selectedConcept">Selected object.</param>
		/// <param name="id">Concept ID.</param>
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

		/// <summary>
		/// Open file.
		/// </summary>
		/// <param name="filePath">File path.</param>
		void openFile(string filePath)
		{
			DeviceService.MainThread(
				async () => await DeviceService.OpenUri($"{Servers.Current}/{filePath}"));
		}

		/// <summary>
		/// Open test.
		/// </summary>
		/// <param name="id">Test ID.</param>
		void openTest(int id)
		{
			DeviceService.MainThread(
				async () => await _navigation.OpenTestPassing(id, true));
		}

		/// <summary>
		/// Open concept.
		/// </summary>
		/// <param name="conceptToCheck">Concept to check.</param>
		/// <param name="conceptToPush">Concept to push.</param>
		void openConcept(ConceptModel conceptToCheck, ConceptModel conceptToPush = null)
		{
			if (conceptToCheck.IsGroup) {
				_previousConcepts.Push(conceptToPush ?? conceptToCheck);
				Concepts = new List<ConceptModel>(conceptToCheck.Children);
			} else if (conceptToCheck.Container.Equals(_testString)) {
				openTest(conceptToCheck.Id);
			}
		}

		/// <summary>
		/// Open previous directory.
		/// </summary>
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
