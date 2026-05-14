using FluentValidation;
using StatusPageServices.RequestDTO.Services;

namespace StatusPageServices.Validation.Creates
{
    public class CreateServiceValidator : AbstractValidator<CreateServiceDto>
    {
        public CreateServiceValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.TargetUrl).NotEmpty().MaximumLength(200).Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithMessage("TargetUrl must be a valid URL.");
            RuleFor(x => x.CategoryId).GreaterThan(0);
        }
    }
}
