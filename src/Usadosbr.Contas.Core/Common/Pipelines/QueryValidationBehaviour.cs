using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;

namespace Usadosbr.Contas.Core.Common.Pipelines
{
    public class
        QueryValidationBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            if (request is not (IWriteRequest or IReadRequest))
            {
                throw new InvalidOperationException(
                    $"Todas as requests devem implementar ou `{nameof(IWriteRequest)}` ou `{nameof(IReadRequest)}`");
            }

            return Unit.Task;
        }
    }
}