using FluentValidation;
using StatusPageServices.RequestDTO.IncidentUpdates;

namespace StatusPageServices.Validation.Updates
{
    public class IncidentUpdatesUpdateValidator : AbstractValidator<UpdateIncidentUpdateDto>
    {
        public IncidentUpdatesUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Message).NotEmpty().MaximumLength(500);
            RuleFor(x => x.UpdateStatus).NotEmpty().MaximumLength(100);
            RuleFor(x => x.IncidentId).GreaterThan(0);
            RuleFor(x => x.EngineerId).GreaterThan(0);
        }
    }
}
