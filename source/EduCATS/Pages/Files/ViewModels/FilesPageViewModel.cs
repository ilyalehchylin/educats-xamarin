using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EduCATS.Data;
using EduCATS.Helpers.Devices.Interfaces;
using EduCATS.Helpers.Dialogs.Interfaces;
using EduCATS.Networking;
using EduCATS.Pages.Files.Models;
using EduCATS.Pages.Utils.ViewModels;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Forms;

namespace EduCATS.Pages.Files.ViewModels
{
	public class FilesPageViewModel : SubjectsViewModel
	{
		const string _filenameKey = "filename";
		const string _filepathKey = "filepath";

		object _progressDialog;

		public FilesPageViewModel(IDialogs dialogs, IAppDevice device) : base(dialogs, device)
		{
			Task.Run(async () => await update());
		}

		List<FilesPageModel> fileList;
		public List<FilesPageModel> FileList {
			get { return fileList; }
			set { SetProperty(ref fileList, value); }
		}

		bool isLoading;
		public bool IsLoading {
			get { return isLoading; }
			set { SetProperty(ref isLoading, value); }
		}

		object selectedItem;
		public object SelectedItem {
			get { return selectedItem; }
			set {
				SetProperty(ref selectedItem, value);
				openFile(selectedItem);
			}
		}

		Command _refreshCommand;
		public Command RefreshCommand {
			get {
				return _refreshCommand ?? (_refreshCommand = new Command(
					async () => await update()));
			}
		}

		async Task update()
		{
			IsLoading = true;
			await SetupSubjects();
			await getFiles();
			IsLoading = false;
		}

		async Task getFiles()
		{
			var filesModel = await DataAccess.GetFiles(CurrentSubject.Id);

			if (DataAccess.IsError && !DataAccess.IsConnectionError) {
				DialogService.ShowError(DataAccess.ErrorMessage);
			}

			var files = filesModel.Lectures?.Select(f => new FilesPageModel(f));

			if (files != null) {
				FileList = new List<FilesPageModel>(files);
			}
		}

		void openFile(object selectedObject)
		{
			if (selectedObject == null || !(selectedObject is FilesPageModel)) {
				return;
			}

			setDownloading();
			SelectedItem = null;

			var file = selectedObject as FilesPageModel;
			var storageFilePath = Path.Combine(DeviceService.GetAppDataDirectory(), file.Name);

			if (File.Exists(storageFilePath)) {
				completeDownload(file.Name, storageFilePath);
				return;
			}

			var fileUri = new Uri($"{Links.GetFile}?fileName={file.PathName}/{file.FileName}");

			try {
				var client = new WebClient();
				client.DownloadProgressChanged += downloadProgressChanged;
				client.DownloadFileCompleted += downloadCompleted;
				client.QueryString.Add(_filenameKey, file.Name);
				client.QueryString.Add(_filepathKey, storageFilePath);
				client.DownloadFileAsync(fileUri, storageFilePath);
			} catch (Exception) {
				hideDownloading();
				DeviceService.MainThread(
					() => DialogService.ShowError(
						CrossLocalization.Translate("files_downloading_error")));
			}
		}

		private void downloadCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (sender == null) {
				return;
			}

			var client = sender as WebClient;
			var fileName = client.QueryString[_filenameKey];
			var pathForFile = client.QueryString[_filepathKey];
			completeDownload(fileName, pathForFile);
		}

		void completeDownload(string fileName, string pathForFile)
		{
			hideDownloading();
			DeviceService.MainThread(() => DeviceService.ShareFile(fileName, pathForFile));
		}

		private void downloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			double bytesIn = double.Parse(e.BytesReceived.ToString());
			double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
			double percentage = bytesIn / totalBytes * 100;
			updateDownloadingProgress(percentage);
		}

		void hideDownloading()
		{
			DeviceService.MainThread(() => DialogService.HideProgress(_progressDialog));
		}

		void updateDownloadingProgress(double percentage)
		{
			DeviceService.MainThread(() => DialogService.UpdateProgress(_progressDialog, (int)percentage));
		}

		void setDownloading()
		{
			DeviceService.MainThread(
				() => _progressDialog = DialogService.ShowProgress(
					CrossLocalization.Translate("files_downloading"),
					CrossLocalization.Translate("common_cancel"),
					() => DialogService.HideProgress(_progressDialog)));
		}
	}
}
