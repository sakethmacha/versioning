using ApiVersioning.Dtos;
using FastEndpoints;
using FluentValidation;

namespace ApiVersioning.Validations
{
    public class GetProductValidator : Validator<GetProductRequest>
    {
        public GetProductValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0");
        }
    }
}
