using FluentValidation;
using StatusPageServices.RequestDTO.Incidents;

namespace StatusPageServices.Validation.Updates
{
    public class IncidentUpdateValidator : AbstractValidator<UpdateIncidentDto>
    {
        public IncidentUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.ServiceId).GreaterThan(0);
            RuleFor(x => x.StartTime).NotEmpty();
            RuleFor(x => x.EndTime)
                .GreaterThanOrEqualTo(x => x.StartTime)
                .When(x => x.EndTime.HasValue);
        }
    }
}
