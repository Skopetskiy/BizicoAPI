using Dtos;
using FluentValidation;
using System;

namespace Dtos
{
    public class RequestValidator : AbstractValidator<RequestDto>
    {
        public RequestValidator()
        {
            RuleSet("Create", () => 
            {
                RuleFor(x => x.Id).NotNull();
                RuleFor(x => x.Experience).GreaterThan(2); 
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id).NotNull();
                RuleFor(x => x.Experience).GreaterThan(2);
                RuleFor(x => x.Technology).MaximumLength(6);
            });

        }
    }
}
