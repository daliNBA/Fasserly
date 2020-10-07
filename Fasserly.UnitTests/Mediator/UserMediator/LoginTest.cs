using Fasserly.Database;
using Fasserly.Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Fasserly.UnitTests.Mediator.UserMediator
{
    [TestClass]
    public class LoginTest : DatabaseTestsBase
    {
        private Mock<UserManager<UserFasserly>> _userManager;
        private Mock<IMediator> _mediator;

        [TestMethod]
        public async Task GetAllTrainigAsync()
        {
            var user = new UserFasserly
            {
                DisplayName = "Dali",
                Email = "dali.mahdoui@gmail.com",
                UserName = "dalinba"
            };

            _userManager.Setup(u=>u.CreateAsync(user, "Pa$$W0rd"));

            //_mediator.Verify(x => x.Send(new Login.Query { Email = "", Password="" }));
            //var logged = await _mediator.Send(new Login.Query { Email = user.Email, Password = "Pa$$W0rd" });

        }

    }
}
