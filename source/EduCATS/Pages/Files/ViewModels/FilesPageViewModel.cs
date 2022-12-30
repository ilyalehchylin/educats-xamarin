using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Helpers;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Logs;
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

		double bytesIn;
		double totalBytes;

		object _progressDialog;
		object _lastSelectedObject;

		WebClient _client;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dialogs">App dialogs.</param>
		/// <param name="device">App device.</param>
		public FilesPageViewModel(IPlatformServices services) : base(services)
		{
			Task.Run(async () => await update(true));
			SubjectChanged += async (s, e) => await update(true);
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
					async () => await update(false)));
			}
		}

		/// <summary>
		/// Update with dialog or pull-to-refresh.
		/// </summary>
		/// <param name="dialog">Is dialog.</param>
		/// <returns>Task.</returns>
		async Task update(bool dialog)
		{
			if (dialog) {
				PlatformServices.Dialogs.ShowLoading();
			} else {
				IsLoading = true;
			}

			await update();

			if (dialog) {
				PlatformServices.Dialogs.HideLoading();
			} else {
				IsLoading = false;
			}
		}

		/// <summary>
		/// Refresh data.
		/// </summary>
		/// <returns>Task.</returns>
		async Task update()
		{
			try {
			await SetupSubjects();
			await getFiles();
			} catch (Exception ex) {
				AppLogs.Log(ex);
			}
		}

		/// <summary>
		/// Get file list.
		/// </summary>
		/// <returns>Task.</returns>
		async Task getFiles()
		{
			IEnumerable<FilesPageModel> files = null;

			var appDataDirectory = PlatformServices.Device.GetAppDataDirectory();

			if (Servers.Current == Servers.EduCatsBntuAddress)
			{
				var filesModel = await DataAccess.GetFiles(CurrentSubject.Id);

				if (DataAccess.IsError && !DataAccess.IsConnectionError)
				{
					PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage);
				}


				files = filesModel.Lectures?.Select(f =>
				{
					var file = Path.Combine(appDataDirectory, f.Name);
					var exists = File.Exists(file);
					return new FilesPageModel(f, exists);
				});

				if (files != null)
				{
					FileList = new List<FilesPageModel>(files);
				}
			}
			else
			{
				var filesModel = await DataAccess.GetFilesTest(CurrentSubject.Id);
				if (DataAccess.IsError && !DataAccess.IsConnectionError)
				{
					PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage);
				}


				files = filesModel.Files?.Select(f =>
				{
					var file = Path.Combine(appDataDirectory, f.Name);
					var exists = File.Exists(file);
					return new FilesPageModel(f, exists);
				});

				if (files != null)
				{

					var filesList = new List<FilesPageModel>(files);
					string URIForDetails = "";

					foreach (var file in filesList)
					{
						URIForDetails += $"\"{file.Name}/{file.Id}/{file.PathName}/{file.FileName}\",";
					}

					URIForDetails = URIForDetails.Remove(URIForDetails.Length - 1);

					var filesDetails = await DataAccess.GetDetailsFilesTest(URIForDetails);

					filesList.ForEach(file =>
					{
						file.Size = ConverterSize.FormatSize(long.Parse(
							filesDetails.FirstOrDefault(detail => file.Id == detail.Id).Size));
					});

					FileList = new List<FilesPageModel>(filesList);
				}
			}
		}

		/// <summary>
		/// Open file.
		/// </summary>
		/// <param name="selectedObject">Selected object.</param>
		void openFile(object selectedObject)
		{
			try {
				if (selectedObject == null || !(selectedObject is FilesPageModel)) {
					return;
				}

				_lastSelectedObject = selectedObject;
				setDownloading();
				SelectedItem = null;

				var file = selectedObject as FilesPageModel;
				var storageFilePath = Path.Combine(PlatformServices.Device.GetAppDataDirectory(), file.Name);

				if (File.Exists(storageFilePath) && new FileInfo(storageFilePath).Length != 0) {
					completeDownload(file.Name, storageFilePath);
					return;
				}

				var fileUri = new Uri($"{Links.GetFile}?fileName={file.PathName}/{file.FileName}");

				totalBytes = bytesIn = 0;
				_client = new WebClient();
				_client.DownloadProgressChanged += downloadProgressChanged;
				_client.DownloadFileCompleted += downloadCompleted;
				_client.QueryString.Add(_filenameKey, file.Name);
				_client.QueryString.Add(_filepathKey, storageFilePath);
				_client.DownloadFileAsync(fileUri, storageFilePath);
			} catch (Exception ex) {
				AppLogs.Log(ex);
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
			} 
			else 
			{
				if (totalBytes != bytesIn || (totalBytes == bytesIn && bytesIn == 0))
				{
					File.Delete(pathForFile);
					hideDownloading();
					PlatformServices.Device.MainThread(() => PlatformServices.Dialogs.ShowError(
						CrossLocalization.Translate("files_downloading_error")));
					return;
				}
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
			updateDownloadedList();
			PlatformServices.Device.MainThread(() => PlatformServices.Device.LaunchFile(pathForFile));
			/*Platf	ormServices.Device.MainThread(
				() => P	latformServices.Device.ShareFile(fileName, pathForFile));*/
		}

					
		/// <summary>
		/// Update downloaded files in list.
		/// </summary>
		void updateDownloadedList()
		{
			if (_lastSelectedObject != null && _lastSelectedObject is FilesPageModel) {
				var fileModel = _lastSelectedObject as FilesPageModel;
				var fileList = FileList.Select(
					f => {
						if (f == fileModel) {
							f.IsDownloaded = true;
						}

						return f;
					}).ToList();

				FileList = new List<FilesPageModel>(fileList);
			}
		}

		/// <summary>
		/// Download progress changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void downloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			bytesIn = double.Parse(e.BytesReceived.ToString());
			totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
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
