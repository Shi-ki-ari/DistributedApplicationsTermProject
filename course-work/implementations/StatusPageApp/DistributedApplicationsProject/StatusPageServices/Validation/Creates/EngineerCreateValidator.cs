using FluentValidation;
using StatusPageServices.RequestDTO.Engineers;

namespace StatusPageServices.Validation.Creates
{
    public class EngineerCreateValidator : AbstractValidator<CreateEngineerDto>
    {
        public EngineerCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
            RuleFor(x => x.HourlyRate).GreaterThanOrEqualTo(0);
        }
    }
}
