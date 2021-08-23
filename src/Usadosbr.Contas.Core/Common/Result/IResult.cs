using System.Collections.Generic;

namespace Usadosbr.Contas.Core.Common.Result
{
    public interface IResult
    {
        object? GetData();

        public IReadOnlyList<ValidationError> Errors { get; }
        
        ResultStatus Status { get; }
    }
}