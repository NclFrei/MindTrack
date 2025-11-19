using FluentValidation;
using MindTrack.Domain.DTOs.Request;

namespace MindTrack.Domain.Validator
{
 public class TarefaCreateRequestValidator : AbstractValidator<TarefaCreateRequest>
 {
 public TarefaCreateRequestValidator()
 {
 RuleFor(a => a.Titulo)
 .NotEmpty().WithMessage("O titulo da tarefa é obrigatório.")
 .MinimumLength(3).WithMessage("O Titulo da tarefa deve ter pelo menos 3 caracteres.");
 }
 }
}