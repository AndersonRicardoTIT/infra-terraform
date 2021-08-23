using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Destructurama.Attributed;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Usadosbr.Contas.Core.Common;
using Usadosbr.Contas.Core.Common.Result;
using Usadosbr.Contas.Core.Entities;

namespace Usadosbr.Contas.Core.Features.Usuarios.Commands.CriaUsuarioCommand
{
    public record CriaUsuarioCommand : IRequest<Result<Guid>>, IWriteRequest
    {
        public CriaUsuarioCommand(string nome, string email, string senha, string telefone)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            Telefone = telefone;
        }

        public string Nome { get; }

        public string Email { get; }

        [LogMasked] public string Senha { get; }

        [LogMasked] public string Telefone { get; }

        internal class Handler : IRequestHandler<CriaUsuarioCommand, Result<Guid>>
        {
            private readonly UserManager<UsadosbrUser> _manager;

            public Handler(UserManager<UsadosbrUser> manager)
            {
                _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            }

            public async Task<Result<Guid>> Handle(CriaUsuarioCommand request, CancellationToken cancellationToken)
            {
                var user = new UsadosbrUser
                {
                    Nome = request.Nome,
                    NomeNormalizado = request.Nome.ToUpper(),
                    UserName = request.Email,
                    Email = request.Email,
                    Telefone = request.Telefone,
                };

                var result = await _manager.CreateAsync(user, request.Senha);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(error => error.Description);

                    return Result<Guid>.Error(errors.ToArray());
                }

                return Result<Guid>.Created(user.Id);
            }
        }
    }
}