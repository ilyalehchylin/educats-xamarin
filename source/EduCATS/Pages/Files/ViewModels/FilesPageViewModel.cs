using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Helpers.Forms;
using EduCATS.Networking;
using EduCATS.Pages.Files.Models;
using EduCATS.Pages.Pickers;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Files.ViewModels
{
	/// <summary>
	/// Files page view model.
	/// </summary>
	public class FilesPageViewModel : SubjectsViewModel
	{
		const string _filenameKey = "filename";
		const string _filepathKey = "filepath";

		object _progressDialog;

		WebClient _client;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dialogs">App dialogs.</param>
		/// <param name="device">App device.</param>
		public FilesPageViewModel(IPlatformServices services) : base(services)
		{
			Task.Run(async () => await update());
			SubjectChanged += async (s, e) => await update();
		}

		List<FilesPageModel> fileList;

		/// <summary>
		/// File list.
		/// </summary>
		public List<FilesPageModel> FileList {
			get { return fileList; }
			set { SetProperty(ref fileList, value); }
		}

		bool _isLoading;

		/// <summary>
		/// Is loading.
		/// </summary>
		public bool IsLoading {
			get { return _isLoading; }
			set { SetProperty(ref _isLoading, value); }
		}

		object _selectedItem;

		/// <summary>
		/// Selected item.
		/// </summary>
		public object SelectedItem {
			get { return _selectedItem; }
			set {
				SetProperty(ref _selectedItem, value);
				openFile(_selectedItem);
			}
		}

		Command _refreshCommand;

		/// <summary>
		/// Refresh command.
		/// </summary>
		public Command RefreshCommand {
			get {
				return _refreshCommand ?? (_refreshCommand = new Command(
					async () => await update()));
			}
		}

		/// <summary>
		/// Refresh data.
		/// </summary>
		/// <returns>Task.</returns>
		async Task update()
		{
			IsLoading = true;
			await SetupSubjects();
			await getFiles();
			IsLoading = false;
		}

		/// <summary>
		/// Get file list.
		/// </summary>
		/// <returns>Task.</returns>
		async Task getFiles()
		{
			var filesModel = await DataAccess.GetFiles(CurrentSubject.Id);

			if (DataAccess.IsError && !DataAccess.IsConnectionError) {
				PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage);
			}

			var files = filesModel.Lectures?.Select(f => new FilesPageModel(f));

			if (files != null) {
				FileList = new List<FilesPageModel>(files);
			}
		}

		/// <summary>
		/// Open file.
		/// </summary>
		/// <param name="selectedObject">Selected object.</param>
		void openFile(object selectedObject)
		{
			if (selectedObject == null || !(selectedObject is FilesPageModel)) {
				return;
			}

			setDownloading();
			SelectedItem = null;

			var file = selectedObject as FilesPageModel;
			var storageFilePath = Path.Combine(PlatformServices.Device.GetAppDataDirectory(), file.Name);

			if (File.Exists(storageFilePath)) {
				completeDownload(file.Name, storageFilePath);
				return;
			}

			var fileUri = new Uri($"{Links.GetFile}?fileName={file.PathName}/{file.FileName}");

			try {
				_client = new WebClient();
				_client.DownloadProgressChanged += downloadProgressChanged;
				_client.DownloadFileCompleted += downloadCompleted;
				_client.QueryString.Add(_filenameKey, file.Name);
				_client.QueryString.Add(_filepathKey, storageFilePath);
				_client.DownloadFileAsync(fileUri, storageFilePath);
			} catch (Exception) {
				hideDownloading();
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
			if (sender == null) {
				return;
			}

			var client = sender as WebClient;
			var fileName = client.QueryString[_filenameKey];
			var pathForFile = client.QueryString[_filepathKey];

			if (e.Cancelled) {
				File.Delete(pathForFile);
			} else {
				completeDownload(fileName, pathForFile);
			}
		}

		/// <summary>
		/// Complete download.
		/// </summary>
		/// <param name="fileName">File name.</param>
		/// <param name="pathForFile">Path for file.</param>
		void completeDownload(string fileName, string pathForFile)
		{
			hideDownloading();
			PlatformServices.Device.MainThread(
				() => PlatformServices.Device.ShareFile(fileName, pathForFile));
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
			updateDownloadingProgress(percentage);
		}

		/// <summary>
		/// Hide downloading.
		/// </summary>
		void hideDownloading()
		{
			PlatformServices.Device.MainThread(() => PlatformServices.Dialogs.HideProgress(_progressDialog));
		}

		/// <summary>
		/// Update downloading progress.
		/// </summary>
		/// <param name="percentage">Percentage.</param>
		void updateDownloadingProgress(double percentage)
		{
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
			if (_client == null || !_client.IsBusy) {
				return;
			}

			_client.CancelAsync();
			PlatformServices.Dialogs.HideProgress(_progressDialog);
		}
	}
}
