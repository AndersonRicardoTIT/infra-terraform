using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Usadosbr.Contas.Core.Common.Result;
using IResult = Usadosbr.Contas.Core.Common.Result.IResult;

namespace Usadosbr.Contas.WebApi.Shared.Result
{
    public class ResultActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if ((context.Result as ObjectResult)?.Value is not IResult result)
                return;

            if (context.Controller is not ControllerBase controller)
                return;

            context.Result = result.Status switch
            {
                ResultStatus.Ok => controller.Ok(result.GetData()),
                ResultStatus.NotFound => controller.NotFound(),
                ResultStatus.Created => CreatedAt(controller, result),
                ResultStatus.Error => ErrorResult(controller, result),
                ResultStatus.Invalid => InvalidResult(controller, result),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static ActionResult CreatedAt(ControllerBase controller, IResult result)
        {
            var actionName = controller.ControllerContext.ActionDescriptor.ActionName;

            return controller.CreatedAtAction(actionName, result.GetData());
        }

        private static ActionResult InvalidResult(ControllerBase controller, IResult result)
        {
            var problem = new ValidationErrorDetails(result.Errors.ToArray())
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Foram encontrados erros de validação",
                Type = "https://httpstatuses.com/400",
                Detail = "Para mais detalhes consulte o campo \"errors\""
            };

            return controller.BadRequest(problem);
        }

        private static ActionResult ErrorResult(ControllerBase controller, IResult result)
        {
            var problem = new ValidationErrorDetails(result.Errors.ToArray())
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Um erro foi encontrado ao processar sua requisição",
                Type = "https://httpstatuses.com/422",
                Detail = "Para mais detalhes consulte o campo \"errors\""
            };

            return controller.UnprocessableEntity(problem);
        }
    }
}