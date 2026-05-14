using FluentValidation;
using StatusPageServices.RequestDTO.Services;

namespace StatusPageServices.Validation.Updates
{
    public class UpdateServiceUpdateValidator : AbstractValidator<UpdateServiceDto>
    {
        public UpdateServiceUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.TargetUrl).NotEmpty().MaximumLength(200).Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithMessage("TargetUrl must be a valid URL.");
            RuleFor(x => x.CategoryId).GreaterThan(0);
        }
    }
}
