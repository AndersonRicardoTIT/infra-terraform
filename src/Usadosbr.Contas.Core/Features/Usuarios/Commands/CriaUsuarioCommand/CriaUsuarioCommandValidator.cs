using System.Linq;
using FluentValidation;

namespace Usadosbr.Contas.Core.Features.Usuarios.Commands.CriaUsuarioCommand
{
    public class CriaUsuarioCommandValidator : AbstractValidator<CriaUsuarioCommand>
    {
        public CriaUsuarioCommandValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("O e-mail passado é inválido");

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("{PropertyName} não pode ser vazia")
                .MaximumLength(200)
                .WithMessage("O {PropertyName} não pode passar de {MaxLength} caracteres");

            RuleFor(x => x.Telefone)
                .Length(11, 20)
                .WithMessage("O telefone deve incluir o DDD e estar entre {MinLength} - {MaxLength} dígitos")
                .Must(t => t.All(c => c is >= '0' and <= '9'))
                .WithMessage("O telefone só pode conter dígitos");
        }
    }
}