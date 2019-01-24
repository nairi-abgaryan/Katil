using System.Collections.Generic;
using System.Linq;
using Katil.Data.Model;
using Katil.Test.Moqito;
using NUnit.Framework;
using TT = System.Threading.Tasks;

namespace Katil.Business.Services.Tests
{
    [Ignore("InProgress")]
    [TestFixture]
    [Category("ServiceTest")]
    public class UserServiceTest : UserMoqito
    {
        private List<User> Users
        {
            get { return DataInitializer.GetUsers(); }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async TT.Task GetUser(int id)
        {
            var userData = Users.FirstOrDefault(x => x.Id == id);

            var user = await UserService.GetUser(id);

            Assert.IsTrue(user != null);
            Assert.IsTrue(user.IsActive == userData.IsActive);
            Assert.IsTrue(user.FullName == userData.FullName);
            Assert.IsTrue(user.SystemUserRoleId == userData.SystemUserRoleId);
        }

        [Ignore("In development")]
        [Test]
        public async TT.Task GetInternalUsers()
        {
            int count = Users.Where(x => x.SystemUserRoleId == 1).Count();

            var internalUser = await UserService.GetUsers();

            Assert.IsTrue(internalUser != null);
            Assert.IsTrue(internalUser.Count == count);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async TT.Task GetUserWitthRole(int id)
        {
            var role = Users.FirstOrDefault(x => x.Id == id).SystemUserRole;

            var user = await UserService.GetUserWithFullInfo(id);

            Assert.IsTrue(user != null);
            Assert.IsTrue(user.SystemUserRole.RoleDescritption.Equals(role.RoleDescritption));
            Assert.IsTrue(user.SystemUserRole.RoleName.Equals(role.RoleName));
            Assert.IsTrue(user.SystemUserRole.SessionDuration.Equals(role.SessionDuration));
            Assert.IsTrue(user.SystemUserRole.SystemUserRoleId.Equals(role.SystemUserRoleId));
        }
    }
}
