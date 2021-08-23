using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Usadosbr.Contas.Core.Common;

namespace Usadosbr.Contas.Core.Features.Usuarios.Queries.BuscaExemploQuery
{
    public record BuscaExemploQuery(string? Nome) : IRequest<List<ExemploReadModel>>, IReadRequest
    {
        internal class Handler : IRequestHandler<BuscaExemploQuery, List<ExemploReadModel>>
        {
            private readonly IDbConnection _connection;

            public Handler(IDbConnection connection)
            {
                _connection = connection;
            }

            public Task<List<ExemploReadModel>> Handle(BuscaExemploQuery request, CancellationToken ct)
            {
                return Task.FromResult(new List<ExemploReadModel>());
            }
        }
    }
}