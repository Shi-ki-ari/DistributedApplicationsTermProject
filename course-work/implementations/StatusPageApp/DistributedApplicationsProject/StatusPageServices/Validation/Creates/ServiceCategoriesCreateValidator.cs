using FluentValidation;
using StatusPageServices.RequestDTO.ServiceCategories;

namespace StatusPageServices.Validation.Creates
{
    public class ServiceCategoriesCreateValidator : AbstractValidator<CreateServiceCategoryDto>
    {
        public ServiceCategoriesCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }
}
