using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Database.Interface;
using Fasserly.Infrastructure.Error;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.DataAccess
{
    public class UserDataServices
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<UserFasserly> _userManager;
        private readonly SignInManager<UserFasserly> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IUserAccessor _userAccessor;

        public UserDataServices(DatabaseContext context, UserManager<UserFasserly> userManager, SignInManager<UserFasserly> signInManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
            _userAccessor = userAccessor;
        }

        public async Task<UserFasserly> GetUserByEmail(string mail, string password)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized);

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                return new UserFasserly
                {
                    DisplayName = user.DisplayName,
                    Image = null,
                    Token = _jwtGenerator.CreateToken(user),
                };
            }
            //return user;

            throw new RestException(HttpStatusCode.Unauthorized);
        }

        public async Task<UserFasserly> Registre(UserFasserly user)
        {
            if (await _context.Users.AnyAsync(m => m.Email == user.Email))
                throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email is already exists" });

            if (await _context.Users.AnyAsync(m => m.UserName == user.UserName))
                throw new RestException(HttpStatusCode.BadRequest, new { UserName = "UserName is already exists" });

            var userResult = new UserFasserly
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                UserName = user.UserName
            };

            var result = await _userManager.CreateAsync(userResult, "Pa$$w0rd");
            if (result.Succeeded)
            {
                return new UserFasserly
                {
                    UserName = userResult.UserName,
                    Token = _jwtGenerator.CreateToken(userResult),
                    DisplayName = userResult.DisplayName,
                    Image = null,
                };
            }

            throw new Exception("Problem creating user");
        }

        public async Task<UserFasserly> GetCurrentUser()
        {
            var userResult = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUserName());
            return new UserFasserly
            {
                UserName = userResult.UserName,
                Token = _jwtGenerator.CreateToken(userResult),
                DisplayName = userResult.DisplayName,
                Image = null,
            };
        }
    }
}
