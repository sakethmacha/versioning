using ApiVersioning.Dtos;
using FastEndpoints;
using FluentValidation;

namespace ApiVersioning.Validations
{
    public class CreateProductValidator : Validator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required")
                .MinimumLength(3).WithMessage("Product name must be at least 3 characters")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0")
                .LessThan(1000000).WithMessage("Price cannot exceed 1,000,000");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Valid category must be selected");

            
        }
    }
}
