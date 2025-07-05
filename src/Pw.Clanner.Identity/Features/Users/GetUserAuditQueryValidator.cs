using FluentValidation;

namespace Pw.Clanner.Identity.Features.Users;

internal sealed class GetUserAuditQueryValidator : AbstractValidator<GetUserAuditQuery>
{
    public GetUserAuditQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId обязательное поле");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber должен быть более или равен 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize должен быть более или равен 1.");
    }
}