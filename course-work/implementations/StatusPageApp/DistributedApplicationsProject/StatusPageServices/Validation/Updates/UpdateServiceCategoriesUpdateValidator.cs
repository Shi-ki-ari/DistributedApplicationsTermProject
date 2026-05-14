using FluentValidation;
using StatusPageServices.RequestDTO.ServiceCategories;

namespace StatusPageServices.Validation.Updates
{
    public class UpdateServiceCategoriesUpdateValidator : AbstractValidator<UpdateServiceCategoryDto>
    {
        public UpdateServiceCategoriesUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }
}
