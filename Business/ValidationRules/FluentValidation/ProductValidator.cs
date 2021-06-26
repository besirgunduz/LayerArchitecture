using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
           // RuleFor(p => p.ProductName).NotEmpty().WithMessage("Ürün ismi gereklidir.");
           // RuleFor(p => p.ProductName).MinimumLength(2); //minimum uzunluk 2 olacak
           // RuleFor(p => p.UnitPrice).NotEmpty();   
           // RuleFor(p => p.UnitPrice).GreaterThan(0); //0 dan büyük olacak
            //kategorisi 1 olduğunda, UnitPrice 10 dan büyük veya eşit olacak.
            RuleFor(P => P.UnitPrice).GreaterThanOrEqualTo(10).When(p => p.CategoryId == 1);
          //  RuleFor(p => p.ProductName).Must(StartWithA).WithMessage("Ürünler A harfi ile başlamalı"); //kendimiz bir kural oluşturabiliriz.

        }

        private bool StartWithA(string arg)
        {
            return arg.StartsWith("A"); //A ile başlarsa true döner.
        }
    }
}
