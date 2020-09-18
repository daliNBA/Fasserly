using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fasserly.WebUI
{
    public static class Extension
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password must have at least six characteres")
                .Matches("[A-Z]").WithMessage("Password must have at least an upper characteres")
                .Matches("[a-z]").WithMessage("Password must have at least a lower characteres")
                .Matches("[0-9]").WithMessage("Password must have at least a number")
                .Matches("[^A-Za-z0-9]").WithMessage("Password must have at least special char");
        }
    }
}
