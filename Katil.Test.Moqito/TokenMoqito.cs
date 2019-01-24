using System.Linq;
using System.Threading;
using Katil.Business.Services.Mapping;
using Katil.Business.Services.TokenServices;
using Katil.Data.Model;
using Katil.Data.Repositories.AppUser;
using Katil.Data.Repositories.Token;
using Katil.Data.Repositories.UnitOfWork;
using Katil.UserResolverService;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Katil.Test.Moqito
{
    public abstract class TokenMoqito
    {
        protected ITokenService TokenService { get; set; }

        protected UserToken Token { get; set; }

        private IUnitOfWork UnitOfWork { get; set; }

        private User User { get; set; }

        private TokenRepository TokenRepository { get; set; }

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
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(s => s.TokenRepository).Returns(TokenRepository);

            unitOfWork.SetupGet(s => s.UserRepository).Returns(UserRepository);

            UnitOfWork = unitOfWork.Object;
        }

        /// <summary>
        /// Tears down each test data
        /// </summary>
        [TearDown]
        public virtual void DisposeTest()
        {
            TokenService = null;
            UnitOfWork = null;
            TokenRepository = null;
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
    }
}
