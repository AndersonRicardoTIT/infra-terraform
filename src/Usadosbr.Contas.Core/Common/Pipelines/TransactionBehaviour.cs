using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;
using Usadosbr.Contas.Core.Persistance;

namespace Usadosbr.Contas.Core.Common.Pipelines
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IDiagnosticContext _diagnostic;
        private readonly UsadosbrAuthContext _context;

        public TransactionBehaviour(IDiagnosticContext diagnostic, UsadosbrAuthContext context)
        {
            _diagnostic = diagnostic;
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (request is not IWriteRequest)
            {
                return await next();
            }

            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                Log.Information("Executando o command {Request} em uma transação", typeof(TRequest).Name);

                var response = await next();

                await transaction.CommitAsync(cancellationToken);

                return response;
            }
            catch (Exception e)
            {
                _diagnostic.Set("RequestProperties", request);
                Log.Error(e, "A transação da request {Request} falhou: {Message}", typeof(TRequest).Name, e.Message);
                throw;
            }
        }
    }
}