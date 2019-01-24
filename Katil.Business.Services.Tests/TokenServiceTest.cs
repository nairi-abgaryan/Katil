using System.Linq;
using Katil.Data.Model;
using Katil.Test.Moqito;
using NUnit.Framework;
using TT = System.Threading.Tasks;

namespace Katil.Business.Services.Tests
{
    [Ignore("In development")]
    [TestFixture]
    [Category("ServiceTest")]
    public class TokenServiceTest : TokenMoqito
    {
        private IQueryable<UserToken> Tokens
        {
            get { return DataInitializer.GetTokens(); }
        }

        [Test]
        [TestCase(500, 1)]
        public async TT.Task GenerateToken(int duration, int id)
        {
            var token = await TokenService.GenerateToken(duration, id);

            Assert.IsTrue(token != null);
            Assert.IsTrue(!string.IsNullOrEmpty(token.AuthToken));
            Assert.IsTrue(!string.IsNullOrEmpty(token.IssuedOn.ToString()));
            Assert.IsTrue(token.IssuedOn.AddSeconds(duration).Equals(token.ExpiresOn));
        }

        [Test]
        public async TT.Task ValidateToken()
        {
            var res = await TokenService.ValidateToken(Token.AuthToken, true);
            Assert.IsTrue(res);
        }

        [Ignore("In development")]
        [Test]
        public void KillToken()
        {
        }

        [Ignore("In development")]
        [Test]
        public void GetUserId()
        {
        }

        [Ignore("In development")]
        [Test]
        public void GetSessionDuration()
        {
        }
    }
}
