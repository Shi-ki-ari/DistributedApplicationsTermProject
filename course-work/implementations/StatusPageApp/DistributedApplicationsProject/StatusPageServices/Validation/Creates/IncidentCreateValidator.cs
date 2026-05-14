using FluentValidation;
using StatusPageServices.RequestDTO.Incidents;

namespace StatusPageServices.Validation.Creates
{
    public class IncidentCreateValidator : AbstractValidator<CreateIncidentDto>
    {
        public IncidentCreateValidator()
        {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.ServiceId).GreaterThan(0);
            RuleFor(x => x.StartTime).NotEmpty();
            RuleFor(x => x.EndTime)
                .GreaterThanOrEqualTo(x => x.StartTime)
                .When(x => x.EndTime.HasValue);
        }
    }
}
