using System.Linq;

using Katil.Business.Services.Mapping;
using Katil.Business.Services.TokenServices;
using Katil.Business.Services.UserServices;
using Katil.Data.Model;
using Katil.Data.Repositories.AppUser;
using Katil.Data.Repositories.UnitOfWork;
using Katil.UserResolverService;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Katil.Test.Moqito
{
    public abstract class UserMoqito
    {
        protected IUserService UserService { get; set; }

        private ITokenService TokenService { get; set; }

        private User User { get; set; }

        private IUnitOfWork UnitOfWork { get; set; }

        private UserRepository UserRepository { get; set; }

        private KatilContext ContextEntities { get; set; }

        /// <summary>
        /// Initial setup for tests
        /// </summary>
        [OneTimeSetUp]
        public virtual void Setup()
        {
            MappingProfile.Init();
        }

        /// <summary>
        /// Re-initializes test.
        /// </summary>
        [SetUp]
        public virtual void ReInitializeTest()
        {
            var contextOptions = new DbContextOptions<KatilContext>();
            ContextEntities = new Mock<KatilContext>(contextOptions, new UserResolver(null)).Object;
            UserRepository = SetUpUserRepository();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(s => s.UserRepository).Returns(UserRepository);
            UnitOfWork = unitOfWork.Object;
            UserService = new UserService(UnitOfWork, TokenService);
        }

        /// <summary>
        /// Tears down each test data
        /// </summary>
        [TearDown]
        public virtual void DisposeTest()
        {
            UserService = null;
            UnitOfWork = null;
            UserRepository = null;
            if (ContextEntities != null)
            {
                ContextEntities.Dispose();
            }
        }

        /// <summary>
        /// One Time teardown
        /// </summary>
        [OneTimeTearDown]
        public virtual void DisposeAllObjects()
        {
        }

        /// <summary>
        /// Setup dummy User repository
        /// </summary>
        /// <returns>UserRepository</returns>
        private UserRepository SetUpUserRepository()
        {
            // Arrange
            var userList = DataInitializer.GetUsers().ToAsyncDbSetMock();

            User = userList.Object.FirstOrDefault();

            userList.Setup(m => m.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => userList.Object.FirstOrDefaultAsync(d => d.Id == (int)ids[0]));

            var contextOptions = new DbContextOptions<KatilContext>();
            var customDbContextMock = new Mock<KatilContext>(contextOptions, new UserResolver(null));

            customDbContextMock.Setup(x => x.Users).Returns(userList.Object);
            customDbContextMock.Setup(x => x.Set<User>()).Returns(userList.Object);

            var mockRepository = new UserRepository(customDbContextMock.Object);

            return mockRepository;
        }
    }
}
