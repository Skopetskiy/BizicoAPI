using Dtos;
using FluentValidation;
using System;

namespace Dtos
{
    public class ProjectValidator : AbstractValidator<ProjectDto>
    {
        public ProjectValidator()
        {
            RuleSet("Create", () =>
            {
                RuleFor(x => x.Id).NotNull();
                RuleFor(x => x.Brief).MinimumLength(10);
                RuleFor(x => x.SkillLevel).NotNull();
                RuleFor(x => x.Price).GreaterThan(50);
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id).NotNull();
                RuleFor(x => x.SkillLevel).NotNull();
                RuleFor(x => x.Technology).MaximumLength(6);
            });
        }
    }
}
