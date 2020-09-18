using Fasserly.Database.Entities;
using FluentValidation;

namespace Fasserly.WebUI.ViewModels
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public object Image { get; set; }

        public UserViewModel(UserFasserly user)
        {
            Email = user.Email;
            Password = user.PasswordHash;
        }

    }
    public class UserViewModelDataValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelDataValidator()
        {
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Password).Password();
            RuleFor(x => x.DisplayName).NotNull();
            RuleFor(x => x.UserName).NotNull();
        }
    }
}
