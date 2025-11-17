using FluentValidation;
using MindTrack.Domain.DTOs.Request;

namespace MindTrack.Domain.Validator
{
    public class MetaCreateRequestValidator : AbstractValidator<MetaCreateRequest>
    {
        public MetaCreateRequestValidator()
        {
            RuleFor(a => a.Titulo)
                .NotEmpty().WithMessage("O titulo da meta é obrigatório.")
                .MinimumLength(3).WithMessage("O Titulo da meta deve ter pelo menos 3 caracteres.");
        }
    }
}
