using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Demo;
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
			var appDataDirectory = PlatformServices.Device.GetAppDataDirectory();

			var filesModel = await DataAccess.GetFilesTest(CurrentSubject.Id);
			if (DataAccess.IsError && !DataAccess.IsConnectionError)
			{
				PlatformServices.Dialogs.ShowError(DataAccess.ErrorMessage);
			}

			var files = filesModel?.Files?.Select(f =>
			{
				var file = Path.Combine(appDataDirectory, f.Name);
				var exists = File.Exists(file);
				return new FilesPageModel(f, exists);
			}).ToList();

			if (files == null || files.Count == 0)
			{
				FileList = new List<FilesPageModel>();
				return;
			}

			var valuesForDetails = files
				.Select(file => $"{file.Name}/{file.Id}/{file.PathName}/{file.FileName}")
				.ToList();

			var filesDetails = await DataAccess.GetDetailsFilesTest(valuesForDetails);
			if (filesDetails != null)
			{
				files.ForEach(file =>
				{
					var detail = filesDetails.FirstOrDefault(item => item.Id == file.Id);
					if (detail == null) {
						return;
					}

					if (long.TryParse(detail.Size, out var size))
					{
						file.Size = ConverterSize.FormatSize(size);
					}

					if (!string.IsNullOrEmpty(detail.Url))
					{
						file.Url = detail.Url;
					}
				});
			}

			FileList = new List<FilesPageModel>(files);
		}

		/// <summary>
		/// Open file.
		/// </summary>
		/// <param name="selectedObject">Selected object.</param>
		void openFile(object selectedObject)
		{
			try {
				logOpenFile($"openFile called. Selected object type: {selectedObject?.GetType().Name ?? "null"}");

				if (AppDemo.Instance.IsDemoAccount) {
					logOpenFile("Demo account detected. Download is disabled.");
					PlatformServices.Device.MainThread(
						() => PlatformServices.Dialogs.ShowError(
							CrossLocalization.Translate("demo_files_download_error")));
					return;
				}

				if (selectedObject == null || !(selectedObject is FilesPageModel)) {
					logOpenFile("Selected object is null or not FilesPageModel. Aborting.");
					return;
				}

				_lastSelectedObject = selectedObject;
				setDownloading();
				SelectedItem = null;
				logOpenFile("Download dialog shown and selected item reset.");

				var file = selectedObject as FilesPageModel;
				logOpenFile(
					$"Preparing file: Id={file.Id}, Name='{file.Name}', PathName='{file.PathName}', FileName='{file.FileName}', Url='{file.Url}'.");
				var storageFilePath = Path.Combine(PlatformServices.Device.GetAppDataDirectory(), file.Name);
				logOpenFile($"Storage path: '{storageFilePath}'.");

				if (File.Exists(storageFilePath) && new FileInfo(storageFilePath).Length != 0) {
					logOpenFile("File already exists locally. Launching cached file.");
					completeDownload(file.Name, storageFilePath);
					return;
				}

				var fileUri = getFileUri(file);
				if (fileUri == null) {
					logOpenFile("Failed to resolve file URI. Aborting download.");
					hideDownloading();
					PlatformServices.Device.MainThread(
						() => PlatformServices.Dialogs.ShowError(
							CrossLocalization.Translate("files_downloading_error")));
					return;
				}
				logOpenFile($"Resolved file URI: '{fileUri}'.");

				totalBytes = bytesIn = 0;
				_client = new WebClient();
				logOpenFile("WebClient created. Subscribing to progress and completion events.");
				_client.DownloadProgressChanged += downloadProgressChanged;
				_client.DownloadFileCompleted += downloadCompleted;
				if (!string.IsNullOrEmpty(PlatformServices.Preferences.AccessToken))
				{
					_client.Headers[HttpRequestHeader.Authorization] =
						PlatformServices.Preferences.AccessToken;
					logOpenFile("Authorization header set for file download request.");
				}
				else
				{
					logOpenFile("Authorization header is empty for file download request.");
				}
				_client.QueryString.Add(_filenameKey, file.Name);
				_client.QueryString.Add(_filepathKey, storageFilePath);
				logOpenFile("Starting async file download.");
				_client.DownloadFileAsync(fileUri, storageFilePath);
			} catch (Exception ex) {
				AppLogs.Log(ex);
				logOpenFile($"openFile failed with exception: '{ex.Message}'.");
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

		/// <summary>
		/// Build file download URI.
		/// </summary>
		/// <param name="file">Selected file.</param>
		/// <returns>File URI.</returns>
		Uri getFileUri(FilesPageModel file)
		{
			if (file == null) {
				logOpenFile("getFileUri: file is null.");
				return null;
			}

			if (!string.IsNullOrEmpty(file.Url))
			{
				if (Uri.TryCreate(file.Url, UriKind.Absolute, out var fullUrl) &&
					(fullUrl.Scheme == Uri.UriSchemeHttp || fullUrl.Scheme == Uri.UriSchemeHttps)) {
					logOpenFile($"getFileUri: absolute URL from metadata: '{fullUrl}'.");
					return fullUrl;
				}

				var relativeUrl = file.Url.StartsWith("/") ? file.Url : $"/{file.Url}";
				var metadataUrl = new Uri($"{Servers.Current}{relativeUrl}");
				logOpenFile($"getFileUri: relative metadata URL resolved to: '{metadataUrl}'.");
				return metadataUrl;
			}

			if (string.IsNullOrEmpty(file.PathName) || string.IsNullOrEmpty(file.FileName)) {
				logOpenFile("getFileUri: fallback failed because PathName/FileName is empty.");
				return null;
			}

			var fileNameParam = $"{file.PathName}//{file.FileName}";
			var fallbackUrl = new Uri($"{Links.GetFile}?filename={fileNameParam}");
			logOpenFile($"getFileUri: fallback URL generated: '{fallbackUrl}'.");
			return fallbackUrl;
		}

		/// <summary>
		/// Log openFile related step.
		/// </summary>
		/// <param name="message">Message.</param>
		void logOpenFile(string message)
		{
			try {
				AppLogs.Log($"[FilesPageViewModel.openFile] {message}", nameof(openFile));
			} catch {
				// Keep file flow functional even if logging is unavailable.
			}
		}
	}
}
