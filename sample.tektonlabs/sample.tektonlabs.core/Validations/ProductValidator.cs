using FluentValidation;
using sample.tektonlabs.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample.tektonlabs.core.Validations;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Key).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.ExpireOn).GreaterThan(DateTime.Now);
        RuleFor(x => x.Providers).Must(ContainMoreThanOneElement);
    }
    private bool ContainMoreThanOneElement(List<ProductProvider> providers)
    {
        return providers.Count > 0;
    }
}

