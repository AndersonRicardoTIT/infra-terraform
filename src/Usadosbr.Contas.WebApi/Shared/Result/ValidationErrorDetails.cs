using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Usadosbr.Contas.Core.Common.Result;

namespace Usadosbr.Contas.WebApi.Shared.Result
{
    public class ValidationErrorDetails : ProblemDetails
    {
        public ValidationErrorDetails(IList<ValidationError> errors)
        {
            Errors = errors;
        }

        public IList<ValidationError> Errors { get; }
    }
}