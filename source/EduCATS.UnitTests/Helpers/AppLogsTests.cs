using System;
using EduCATS.Helpers.Files;
using EduCATS.Helpers.Logs;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class AppLogsTests
	{
		const string _directory = "";
		const string _filePath = "app_logs.txt";
		const string _contents = "contents";
		const string _errorMessage = "error";

		Mock<IFileManager> _mock;

		[SetUp]
		public void SetUp()
		{
			_mock = new Mock<IFileManager>();
			_mock.Setup(m => m.Append($"/{_filePath}", _contents)).Verifiable();
			_mock.Setup(m => m.Create($"/{_filePath}")).Verifiable();
			_mock.Setup(m => m.Delete($"/{_filePath}")).Verifiable();
			_mock.Setup(m => m.Read($"/{_filePath}")).Returns(_contents);
		}

		[Test]
		public void InitializeExistsTest()
		{
			try {
				_mock.Setup(m => m.Exists($"/{_filePath}")).Returns(true);
				AppLogs.FileManager = _mock.Object;
				AppLogs.Initialize(_directory);
				return;
			} catch (Exception ex) {
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void InitializeNotExistsTest()
		{
			try {
				_mock.Setup(m => m.Exists($"/{_filePath}")).Returns(false);
				AppLogs.FileManager = _mock.Object;
				AppLogs.Initialize(_directory);
				return;
			} catch (Exception ex) {
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void LogTest()
		{
			try {
				_mock.Setup(m => m.Exists($"/{_filePath}")).Returns(true);
				AppLogs.FileManager = _mock.Object;
				AppLogs.Initialize(_directory);
				AppLogs.Log(new Exception(_errorMessage));
				return;
			} catch (Exception ex) {
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void ReadLogTest()
		{
			try {
				_mock.Setup(m => m.Exists($"/{_filePath}")).Returns(true);
				AppLogs.FileManager = _mock.Object;
				AppLogs.Initialize(_directory);
				var actual = AppLogs.ReadLog();
				Assert.AreEqual(_contents, actual);
			} catch (Exception ex) {
				Assert.Fail(ex.Message);
			}
		}

		[Test]
		public void DeleteLogTest()
		{
			try {
				_mock.Setup(m => m.Exists($"/{_filePath}")).Returns(true);
				AppLogs.FileManager = _mock.Object;
				AppLogs.Initialize(_directory);
				AppLogs.DeleteLog();
			} catch (Exception ex) {
				Assert.Fail(ex.Message);
			}
		}
	}
}
