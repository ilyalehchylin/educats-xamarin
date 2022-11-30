using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
using EduCATS.Networking;
using EduCATS.Pages.Pickers;
using Newtonsoft.Json;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Eemc.ViewModels
{
	/// <summary>
	/// EEMC page view model.
	/// </summary>
	public class EemcPageViewModel : SubjectsViewModel
	{
		const string _filepathKey = "filepath";
		WebClient _client;
		object _progressDialog;
		/// <summary>
		/// Test identifier string.
		/// </summary>
		const string _testString = "test";

		/// <summary>
		/// Search ID.
		/// </summary>
		readonly int _searchId;

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
		public EemcPageViewModel(IPlatformServices services, int searchId) : base(services)
		{
			IsRoot = true;
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
			try {
				PlatformServices.Dialogs.ShowLoading();
				await SetupSubjects();
				await setRootConcepts();

				if (_searchId == -1 || _backupRootConceptsWithoutChildren == null) {
					PlatformServices.Dialogs.HideLoading();
					return;
				}

				await setConceptsFromRoot(_backupRootConceptsWithoutChildren[0].Id);
				searchForBook(_searchId);
				IsBackActionPossible = false;
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}

			PlatformServices.Dialogs.HideLoading();
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
				PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage);
			}

			var rootConcepts = root?.Concepts;

			if (rootConcepts != null) {
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
			try {
				if (selectedObject == null || !(selectedObject is ConceptModel)) {
					return;
				}

				SelectedItem = null;
				var concept = selectedObject as ConceptModel;
				var id = concept.Id;

				if (IsRoot) {
					_rootId = id;
					PlatformServices.Dialogs.ShowLoading();
					await setConceptsFromRoot(id);
					PlatformServices.Dialogs.HideLoading();
				} else {
					setOrOpenConcept(concept, id);
				}
			} catch (Exception ex) {
				AppLogs.Log(ex);
				PlatformServices.Dialogs.HideLoading();
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
			ConceptModel conceptTree = null;

			if (Servers.Current == Servers.EduCatsBntuAddress)
				conceptTree = await DataAccess.GetConceptTree(id);
			else
			{
				ConceptModelTest conceptCascade = await DataAccess.GetConceptCascade(id);
				conceptTree = JsonConvert.DeserializeObject<ConceptModel>(conceptCascade.Concept.ToString());
			}

			if (DataAccess.IsError && !DataAccess.IsConnectionError) {
				PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage);
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
			if (Servers.Current == Servers.EduCatsBntuAddress)
				PlatformServices.Device.MainThread(
					async () => await PlatformServices.Device.OpenUri($"{Servers.Current}/{filePath}"));
			else
			{
				PlatformServices.Device.MainThread(
					async () => await downloadAndOpenFile(filePath));
			}
		}


		async Task downloadAndOpenFile(string filePath)
		{
			try
			{
				setDownloading();
				var separatingResult = filePath.Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
				string fileName = separatingResult[1];
				string pathFile = separatingResult[0];
				var storageFilePath = Path.Combine(PlatformServices.Device.GetAppDataDirectory(), fileName);

				if (File.Exists(storageFilePath))
				{
					completeDownload(storageFilePath);
					return;
				}

				var fileUri = new Uri($"{Servers.Current}/api/Upload?fileName={filePath}");
				ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
				_client = new WebClient();
				_client.DownloadProgressChanged += downloadProgressChanged;
				_client.DownloadFileCompleted += downloadCompleted;
				_client.QueryString.Add(_filepathKey, storageFilePath);
				_client.DownloadFileAsync(fileUri, storageFilePath);
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex);
				PlatformServices.Device.MainThread(() => PlatformServices.Dialogs.HideProgress(_progressDialog));
				PlatformServices.Device.MainThread(
					() => PlatformServices.Dialogs.ShowError(
						CrossLocalization.Translate("files_downloading_error")));
			}
		}

		/// <summary>
		/// Download completed.
		/// </summary>
		/// <param name="sender">Web client.</param>
		/// <param name="e">Event arguments.</param>
		private void downloadCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (sender == null)
			{
				return;
			}

			var client = sender as WebClient;
			var pathForFile = client.QueryString[_filepathKey];

			if (e.Cancelled)
			{
				File.Delete(pathForFile);
			}
			else
			{
				completeDownload(pathForFile);
			}
		}

		/// <summary>
		/// Download progress changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void downloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			double bytesIn = double.Parse(e.BytesReceived.ToString());
			double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
			double percentage = bytesIn / totalBytes * 100;
			PlatformServices.Device.MainThread(
				() => PlatformServices.Dialogs.UpdateProgress(_progressDialog, (int)percentage));
		}

		/// <summary>
		/// Set downloading.
		/// </summary>
		void setDownloading()
		{
			PlatformServices.Device.MainThread(() => {
				_progressDialog = PlatformServices.Dialogs.ShowProgress(
					CrossLocalization.Translate("files_downloading"),
					CrossLocalization.Translate("base_cancel"),
					() => abortDownload());
			});
		}

		/// <summary>
		/// Abort downloading process.
		/// </summary>
		void abortDownload()
		{
			if (_client == null || !_client.IsBusy)
			{
				return;
			}

			_client.CancelAsync();
			PlatformServices.Dialogs.HideProgress(_progressDialog);
		}

		/// <summary>
		/// Complete download.
		/// </summary>
		/// <param name="fileName">File name.</param>
		/// <param name="pathForFile">Path for file.</param>
		void completeDownload(string pathForFile)
		{
			PlatformServices.Device.MainThread(() => PlatformServices.Dialogs.HideProgress(_progressDialog));
			PlatformServices.Device.MainThread(() => PlatformServices.Device.LaunchFile(pathForFile));
		}

		/// <summary>
		/// Open test.
		/// </summary>
		/// <param name="id">Test ID.</param>
		void openTest(int id)
		{
			PlatformServices.Device.MainThread(
				async () => await PlatformServices.Navigation.OpenTestPassing(id, true));
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
			try {
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
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}
	}
}
