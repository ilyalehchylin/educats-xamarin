using EduCATS.Data.Models;
using EduCATS.Data.User;
using EduCATS.Helpers.Forms;
using Moq;
using NUnit.Framework;

namespace EduCATS.UnitTests
{
	[TestFixture]
	public class AppUserDataTests
	{
		const int _userId = 1;
		const int _groupId = 1;
		const string _name = "Name";
		const string _userType = "1";
		const string _avatar = "Avatar";
		const string _groupName = "Group name";
		const string _username = "TestUsername";

		IPlatformServices _mocked;
		UserProfileModel _profile;

		[SetUp]
		public void SetUp()
		{
			AppUserData.IsProfileLoaded = false;

			_profile = new UserProfileModel {
				Name = _name,
				Avatar = _avatar,
				GroupId = _groupId,
				GroupName = _groupName,
				UserType = _userType
			};

			_mocked = Mock.Of<IPlatformServices>(ps =>
				ps.Preferences.UserId == _userId &&
				ps.Preferences.UserLogin == _username &&
				ps.Preferences.Avatar == _avatar &&
				ps.Preferences.GroupName == _groupName);
		}

		[Test]
		public void SetLoginDataTest()
		{
			AppUserData.SetLoginData(_mocked, _userId, _username);
			Assert.AreEqual(_userId, AppUserData.UserId);
			Assert.AreEqual(_username, AppUserData.Username);
		}

		[Test]
		public void SetProfileDataNullTest()
		{
			AppUserData.SetProfileData(_mocked, null);
			Assert.AreEqual(false, AppUserData.IsProfileLoaded);
		}

		[Test]
		public void SetProfileData()
		{
			AppUserData.SetProfileData(_mocked, _profile);
			Assert.AreEqual(true, AppUserData.IsProfileLoaded);
			Assert.AreEqual(_name, AppUserData.Name);
			Assert.AreEqual(_avatar, AppUserData.Avatar);
			Assert.AreEqual(_groupId, AppUserData.GroupId);
			Assert.AreEqual(_groupName, AppUserData.GroupName);
			Assert.AreEqual(UserTypeEnum.Professor, AppUserData.UserType);
		}

		[Test]
		public void ClearTest()
		{
			AppUserData.SetProfileData(_mocked, _profile);
			AppUserData.Clear();
			Assert.AreEqual(true, AppUserData.IsProfileLoaded);
			Assert.AreEqual(null, AppUserData.Name);
			Assert.AreEqual(null, AppUserData.Avatar);
			Assert.AreEqual(0, AppUserData.GroupId);
			Assert.AreEqual(null, AppUserData.GroupName);
			Assert.AreEqual(UserTypeEnum.Student, AppUserData.UserType);
			Assert.AreEqual(0, AppUserData.UserId);
			Assert.AreEqual(null, AppUserData.Username);
		}
	}
}
