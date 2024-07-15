using FluentValidation;
using Service.ViewModels.AccountToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validate
{
    public class AccountTokenValidator : AbstractValidator<AccountTokenRequest>
    {
        public AccountTokenValidator()
        {
            RuleFor(at => at.AccessToken)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("{PropertyName} is not null.")
                .NotEmpty().WithMessage("{PropertyName} is not empty.");

            RuleFor(at => at.RefreshToken)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("{PropertyName} is not null.")
                .NotEmpty().WithMessage("{PropertyName} is not empty.");
        }
    }
}
