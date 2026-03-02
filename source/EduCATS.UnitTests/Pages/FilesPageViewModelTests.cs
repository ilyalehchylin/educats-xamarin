using System;
using System.Reflection;
using System.Runtime.Serialization;
using EduCATS.Helpers.Forms;
using EduCATS.Helpers.Forms.Settings;
using EduCATS.Networking;
using EduCATS.Pages.Files.Models;
using EduCATS.Pages.Files.ViewModels;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests.Pages
{
	[TestFixture]
	public class FilesPageViewModelTests
	{
		static readonly BindingFlags _privateInstance = BindingFlags.Instance | BindingFlags.NonPublic;

		[SetUp]
		public void SetUp()
		{
			var preferences = new Mock<IPreferences>();
			preferences.SetupGet(p => p.Server).Returns(Servers.EduCatsBntuAddress);
			var services = new Mock<IPlatformServices>();
			services.SetupGet(s => s.Preferences).Returns(preferences.Object);
			Servers.PlatformServices = services.Object;
		}

		[Test]
		public void GetFileUriTreatsApiPathAsServerRelativeUrl()
		{
			var viewModel = (FilesPageViewModel)FormatterServices
				.GetUninitializedObject(typeof(FilesPageViewModel));
			var method = typeof(FilesPageViewModel).GetMethod("getFileUri", _privateInstance);
			var file = (FilesPageModel)FormatterServices.GetUninitializedObject(typeof(FilesPageModel));
			file.Url = "/api/Upload?filename=P//F.pdf";

			var uri = (Uri)method.Invoke(viewModel, new object[] { file });

			Assert.AreEqual(
				$"{Servers.EduCatsBntuAddress}/api/Upload?filename=P//F.pdf",
				uri.ToString());
		}
	}
}
