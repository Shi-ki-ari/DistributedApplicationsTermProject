using FluentValidation;
using StatusPageServices.RequestDTO.Engineers;

namespace StatusPageServices.Validation.Updates
{
    public class EngineerUpdateValidator : AbstractValidator<UpdateEngineerDto>
    {
        public EngineerUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
            RuleFor(x => x.HourlyRate).GreaterThanOrEqualTo(0);
        }
    }
}
