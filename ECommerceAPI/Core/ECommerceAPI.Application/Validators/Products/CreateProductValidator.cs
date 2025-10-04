using ECommerceAPI.Application.ViewModels.Products;
using FluentValidation;

namespace ECommerceAPI.Application.Validators.Products;

public class CreateProductValidator : AbstractValidator<VmCreateProduct>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Product name cannot be empty")
            .MaximumLength(150)
            .MinimumLength(3)
            .WithMessage("Product name length must be between 3 to 150 characters");

        RuleFor(p => p.Stock)
            .NotEmpty()
            .NotNull()
            .WithMessage("Stock cannot be empty!!")
            .Must(s => s >= 0) // Minumum değer için "Must" kullandık
            .WithMessage("Stock cannot be negative!");

        RuleFor(p => p.Price)
            .NotEmpty()
            .NotNull()
            .WithMessage("Price cannot be empty!!")
            .Must(s => s >= 0) 
            .WithMessage("Price cannot be negative!");
    }
}

